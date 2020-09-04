using UnityEngine;

namespace Minigames.MissTheTarget
{
    public class ThrowController: MonoBehaviour
    {
        public float ShootPower = 10f;

        private Rigidbody2D rigidbody2d;
        private Vector3 spawnPoint;
        private MinigameManager gameManager;
        private void Start()
        {
            rigidbody2d = GetComponent<Rigidbody2D>();
            spawnPoint = transform.position;
            gameManager = GetComponentInParent<MinigameManager>();
            subscribeToEvents();
            rigidbody2d.simulated = false;
        }
        
        private void subscribeToEvents()
        {
            gameManager.ButtonEvents.OnActionButtonPressed += HandleActionButtonPressed;
        }

        private void unsubscribeToEvents()
        {
            gameManager.ButtonEvents.OnActionButtonPressed -= HandleActionButtonPressed;
        }

        private void HandleActionButtonPressed()
        {
            if (gameManager.GameOver)
            {
                return;
            }

            rigidbody2d.simulated = true;
            rigidbody2d.AddForce(Vector2.up * ShootPower, ForceMode2D.Impulse);
        }

        private void OnDisable()
        {
            unsubscribeToEvents();
        }

        private void resetPosition()
        {
            transform.position = spawnPoint;
            rigidbody2d.velocity = Vector2.zero;
            rigidbody2d.simulated = false;
        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("scorezone"))
            {
                gameManager.Events.EventScored();
            }
            else if (collision.CompareTag("deadzone"))
            {
                gameManager.Events.EventHit();
            }

            resetPosition();
        }
    }
}