using System;
using System.Collections;
using Shaders;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Minigames.Cannonizer
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
        private float pressedFireButton;

        private void Start()
        {
            spriteOutline = GetComponent<SpriteOutline>();
            spriteOutline.outlineSize = 9;
            spriteOutline.color = Color.green;

            gameManager = GetComponentInParent<CannonizerManager>();
            targetRotation = transform.rotation;
            ballDirection = Vector2.left;
            canFire = true;

            gameManager.ButtonEvents.OnUpButtonPressed += HandleUpButtonPressed;
            gameManager.ButtonEvents.OnDownButtonPressed += HandleDownButtonPressed;
            gameManager.ButtonEvents.OnLeftButtonPressed += HandleLeftButtonPressed;
            gameManager.ButtonEvents.OnRightButtonPressed += HandleRightButtonPressed;
            gameManager.ButtonEvents.OnActionButtonStateChanged += HandleActionButtonPressed;
        }

        private void OnDisable()
        {
            gameManager.ButtonEvents.OnUpButtonPressed -= HandleUpButtonPressed;
            gameManager.ButtonEvents.OnDownButtonPressed -= HandleDownButtonPressed;
            gameManager.ButtonEvents.OnLeftButtonPressed -= HandleLeftButtonPressed;
            gameManager.ButtonEvents.OnRightButtonPressed -= HandleRightButtonPressed;
            gameManager.ButtonEvents.OnActionButtonStateChanged -= HandleActionButtonPressed;
        }

        private void HandleActionButtonPressed(InputValue inputValue)
        {
            pressedFireButton = inputValue.Get<float>();
        }

        private void Update()
        {
            if (gameManager.GameOver)
            {
                return;
            }

            if (Math.Abs(pressedFireButton - 1) < 0.1)
            {
                shootZeBall();
            }
            
            gameObject.transform.rotation = Quaternion.Lerp(
                transform.rotation,
                targetRotation,
                RotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, targetRotation) <= AngleApproximation && canFire)
            {
                spriteOutline.color = Color.green;
            }
            else
            {
                spriteOutline.color = Color.red;
            }
        }

        private void rotateCannon(float angle)
        {
            targetRotation = Quaternion.Euler(0, 0, angle);
            RotationAudio.Play();

            switch (angle)
            {
                case 0:
                    ballDirection = Vector2.right;
                    break;
                case 180:
                    ballDirection = Vector2.left;
                    break;
                case 270:
                    ballDirection = Vector2.down;
                    break;
                case 90:
                    ballDirection = Vector2.up;
                    break;
            }
        }

        private void shootZeBall()
        {
            // check if ready to fire and not in rotation
            if (!canFire || Quaternion.Angle(transform.rotation, targetRotation) >= AngleApproximation)
            {
                CantShotBallAudio.Play();
                return;
            }

            // Fire
            var ball = Instantiate(BallPrefab, BallSpawnPoint.transform.position, Quaternion.identity);
            ball.transform.SetParent(gameManager.transform);
            ball.GetComponent<Rigidbody2D>().AddForce(ballDirection * BallSpeed);

            canFire = false;
            spriteOutline.color = Color.red;

            ShootBallAudio.Play();

            Destroy(ball, 3.0f);

            StartCoroutine(CannonCooldown());
        }

        private IEnumerator CannonCooldown()
        {
            for (var i = ShootCooldown; i > 0; i--)
            {
                yield return new WaitForSeconds(1);
            }

            canFire = true;
        }

        private void HandleRightButtonPressed()
        {
            rotateCannon(0);
        }

        private void HandleLeftButtonPressed()
        {
            rotateCannon(180);
        }

        private void HandleDownButtonPressed()
        {
            rotateCannon(270);
        }

        private void HandleUpButtonPressed()
        {
            rotateCannon(90);
        }
    }

}
