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
            this.rigidbody2d = GetComponent<Rigidbody2D>();
            this.spawnPoint = this.transform.position;
            this.gameManager = this.GetComponentInParent<MinigameManager>();
            this.subscribeToEvents();
            rigidbody2d.simulated = false;
        }
        
        private void subscribeToEvents()
        {
            this.gameManager.ButtonEvents.OnActionButtonPressed += HandleActionButtonPressed;
        }

        private void unsubscribeToEvents()
        {
            this.gameManager.ButtonEvents.OnActionButtonPressed -= HandleActionButtonPressed;
        }

        private void HandleActionButtonPressed()
        {
            if (this.gameManager.GameOver)
            {
                return;
            }

            rigidbody2d.simulated = true;
            rigidbody2d.AddForce(Vector2.up * this.ShootPower, ForceMode2D.Impulse);
        }

        private void OnDisable()
        {
            this.unsubscribeToEvents();
        }

        private void resetPosition()
        {
            this.transform.position = this.spawnPoint;
            rigidbody2d.velocity = Vector2.zero;
            rigidbody2d.simulated = false;
        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("scorezone"))
            {
                this.gameManager.Events.EventScored();
            }
            else if (collision.CompareTag("deadzone"))
            {
                this.gameManager.Events.EventHit();
            }

            resetPosition();
        }
    }
}