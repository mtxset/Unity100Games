using UnityEngine;

namespace Minigames.DroneFly
{
    public class TapController : MonoBehaviour
    {
        public float TapForce = 10;
        public float TiltSmooth = 5;
        public Vector3 StartPosition;

        public AudioSource TapAudio;
        public AudioSource ScoreAudio;
        public AudioSource DieAudio;

        private Rigidbody2D rigidBody;
        private Quaternion downRotation;
        private Quaternion forwardRotation;

        private MinigameManager gameManager;

        private void OnDisable()
        {
            gameManager.ButtonEvents.OnActionButtonPressed -= HandleActionButton;
            gameManager.DroneEvents.OnGameConfirmed -= HandleGameOverConfirmed;
            gameManager.DroneEvents.OnGameStarted -= HandleGameStarted;
        }

        private void HandleGameOverConfirmed()
        {
            transform.localPosition = StartPosition;
            transform.rotation = Quaternion.identity;
        }

        private void HandleGameStarted()
        {
            rigidBody.velocity = Vector3.zero;
            rigidBody.simulated = true;
        }

        private void Start()
        {
            gameManager = GetComponentInParent<MinigameManager>();

            gameManager.DroneEvents.OnGameConfirmed += HandleGameOverConfirmed;
            gameManager.DroneEvents.OnGameStarted += HandleGameStarted;
            gameManager.ButtonEvents.OnActionButtonPressed += HandleActionButton;

            rigidBody = GetComponent<Rigidbody2D>();
            downRotation = Quaternion.Euler(0, 0, -75);
            forwardRotation = Quaternion.Euler(0, 0, 35);
            rigidBody.simulated = false;
        }

        private void HandleActionButton()
        {
            if (gameManager.GameOver) return;

            TapAudio.Play();
            transform.rotation = forwardRotation;
            rigidBody.velocity = Vector3.zero;
            rigidBody.AddForce(Vector2.up * TapForce, ForceMode2D.Force);
        }

        private void Update()
        {
            if (gameManager.GameOver)
            {
                return;
            }

            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                downRotation,
                TiltSmooth * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            switch (collision.gameObject.tag)
            {
                case "scorezone":
                    gameManager.DroneEvents.EventScored();
                    ScoreAudio.Play();
                    break;
                case "deadzone":
                    DieAudio.Play();
                    rigidBody.simulated = false;
                    gameManager.DroneEvents.EventDroneDeath();
                    break;
            }
        }
    }
}