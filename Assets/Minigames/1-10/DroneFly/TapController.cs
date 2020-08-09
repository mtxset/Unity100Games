using UnityEngine;

namespace Minigames.DroneFly
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class TapController : MonoBehaviour
    {
        public float TapForce = 10;
        public float TiltSmooth = 5;
        public Vector3 StartPosition;

        public AudioSource TapAudio;
        public AudioSource ScoreAudio;
        public AudioSource dieAudio;

        private Rigidbody2D rigidBody;
        private Quaternion downRotation;
        private Quaternion forwardRotation;

        private MinigameManager gameManager;

        private void OnDisable()
        {
            this.gameManager.ButtonEvents.OnActionButtonPressed -= this.HandleActionButton;
            this.gameManager.DroneEvents.OnGameConfirmed -= this.HandleGameOverConfirmed;
            this.gameManager.DroneEvents.OnGameStarted -= this.HandleGameStarted;
        }

        private void HandleGameOverConfirmed()
        {
            this.transform.localPosition = this.StartPosition;
            this.transform.rotation = Quaternion.identity;
        }

        private void HandleGameStarted()
        {
            this.rigidBody.velocity = Vector3.zero;
            this.rigidBody.simulated = true;
        }

        private void Start()
        {
            this.gameManager = GetComponentInParent<MinigameManager>();

            this.gameManager.DroneEvents.OnGameConfirmed += this.HandleGameOverConfirmed;
            this.gameManager.DroneEvents.OnGameStarted += this.HandleGameStarted;
            this.gameManager.ButtonEvents.OnActionButtonPressed += this.HandleActionButton;

            this.rigidBody = GetComponent<Rigidbody2D>();
            this.downRotation = Quaternion.Euler(0, 0, -75);
            this.forwardRotation = Quaternion.Euler(0, 0, 35);
            this.rigidBody.simulated = false;
        }

        private void HandleActionButton()
        {
            if (gameManager.GameOver) return;

            this.TapAudio.Play();
            this.transform.rotation = forwardRotation;
            this.rigidBody.velocity = Vector3.zero;
            this.rigidBody.AddForce(Vector2.up * this.TapForce, ForceMode2D.Force);
        }

        private void Update()
        {
            if (gameManager.GameOver)
            {
                return;
            }

            this.transform.rotation = Quaternion.Lerp(
                this.transform.rotation,
                this.downRotation,
                this.TiltSmooth * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            switch (collision.gameObject.tag)
            {
                case "scorezone":
                    this.gameManager.DroneEvents.EventScored();
                    this.ScoreAudio.Play();
                    break;
                case "deadzone":
                    this.dieAudio.Play();
                    this.rigidBody.simulated = false;
                    this.gameManager.DroneEvents.EventDroneDeath();
                    break;
            }
        }
    }
}