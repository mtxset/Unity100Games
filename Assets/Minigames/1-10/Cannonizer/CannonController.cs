using Assets.Shaders;
using System.Collections;
using UnityEngine;

namespace Assets.Minigames.Cannonizer
{
    public class CannonController: MonoBehaviour
    {
        public float RotationSpeed;
        public GameObject BallPrefab;
        public float BallSpeed = 10f;

        public AudioSource ShootBallAudio;
        public AudioSource CantShotBallAudio;
        public AudioSource RotationAudio;
        public AudioSource DeadAudio;
        public Transform BallSpawnPoint;
        public float AngleApproximation = 0.1f;
        public int ShootCooldown = 1;

        private CannonizerManager gameManager;
        private Quaternion targetRotation;
        private Vector2 ballDirection;
        private bool canFire;
        private SpriteOutline spriteOutline;

        private void Start()
        {
            this.spriteOutline = GetComponent<SpriteOutline>();
            this.spriteOutline.outlineSize = 9;
            this.spriteOutline.color = Color.green;

            this.gameManager = GetComponentInParent<CannonizerManager>();
            this.targetRotation = this.transform.rotation;
            this.ballDirection = Vector2.left;
            this.canFire = true;

            this.gameManager.ButtonEvents.OnUpButtonPressed += HandleUpButtonPressed;
            this.gameManager.ButtonEvents.OnDownButtonPressed += HandleDownButtonPressed;
            this.gameManager.ButtonEvents.OnLeftButtonPressed += HandleLeftButtonPressed;
            this.gameManager.ButtonEvents.OnRightButtonPressed += HandleRightButtonPressed;
            this.gameManager.ButtonEvents.OnActionButtonPressed += HandleActionButtonPressed;
        }

        private void OnDisable()
        {
            this.gameManager.ButtonEvents.OnUpButtonPressed -= HandleUpButtonPressed;
            this.gameManager.ButtonEvents.OnDownButtonPressed -= HandleDownButtonPressed;
            this.gameManager.ButtonEvents.OnLeftButtonPressed -= HandleLeftButtonPressed;
            this.gameManager.ButtonEvents.OnRightButtonPressed -= HandleRightButtonPressed;
            this.gameManager.ButtonEvents.OnActionButtonPressed -= HandleActionButtonPressed;
        }

        private void Update()
        {
            if (this.targetRotation != null && !gameManager.GameOver)
            {
                this.gameObject.transform.rotation = Quaternion.Lerp(
                    this.transform.rotation,
                    this.targetRotation,
                    this.RotationSpeed * Time.deltaTime);
            }

            if (Quaternion.Angle(this.transform.rotation, this.targetRotation) <= this.AngleApproximation && canFire)
            {
                this.spriteOutline.color = Color.green;
            }
            else
            {
                this.spriteOutline.color = Color.red;
            }
        }

        private void rotateCannon(float angle)
        {
            this.targetRotation = Quaternion.Euler(0, 0, angle);
            this.RotationAudio.Play();

            switch (angle)
            {
                case 0:
                    this.ballDirection = Vector2.right;
                    break;
                case 180:
                    this.ballDirection = Vector2.left;
                    break;
                case 270:
                    this.ballDirection = Vector2.down;
                    break;
                case 90:
                    this.ballDirection = Vector2.up;
                    break;
            }
        }

        private void HandleActionButtonPressed()
        {
            // check if ready to fire and not in rotation
            if (!canFire || Quaternion.Angle(this.transform.rotation, this.targetRotation) >= AngleApproximation)
            {
                this.CantShotBallAudio.Play();
                return;
            }

            // Fire
            var ball = Instantiate(this.BallPrefab, this.BallSpawnPoint.transform.position, Quaternion.identity);
            ball.transform.SetParent(this.gameManager.transform);
            ball.GetComponent<Rigidbody2D>().AddForce(this.ballDirection * this.BallSpeed);

            this.canFire = false;
            this.spriteOutline.color = Color.red;

            this.ShootBallAudio.Play();

            Destroy(ball, 3.0f);

            StartCoroutine(CannonCooldown());
        }

        IEnumerator CannonCooldown()
        {
            for (int i = this.ShootCooldown; i > 0; i--)
            {
                yield return new WaitForSeconds(1);
            }

            this.canFire = true;
        }

        private void HandleRightButtonPressed()
        {
            this.rotateCannon(0);
        }

        private void HandleLeftButtonPressed()
        {
            this.rotateCannon(180);
        }

        private void HandleDownButtonPressed()
        {
            this.rotateCannon(270);
        }

        private void HandleUpButtonPressed()
        {
            this.rotateCannon(90);
        }
    }

}
