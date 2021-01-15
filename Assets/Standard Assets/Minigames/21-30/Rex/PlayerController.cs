using Components;
using UnityEngine;

namespace Minigames.Rex {
    public class PlayerController : AddMinigameManager2 {

        public float JumpForce = 4f;

        private bool canJump = true;
        private Rigidbody2D rigidbody2d;

        private void Start() {
            rigidbody2d = GetComponent<Rigidbody2D>();

            MinigameManager.ButtonEvents.OnActionButtonPressed += HandleAction;
            MinigameManager.ButtonEvents.OnUpButtonPressed += HandleAction;
        }

        private void HandleAction() => Jump();

        private void OnCollisionEnter2D(Collision2D other) {
            if (other.collider.CompareTag("scorezone"))
                canJump = true;
            else if (other.collider.CompareTag("deadzone"))
                MinigameManager.Events.EventHit();
        }

        private void OnCollisionExit2D(Collision2D other) {
            if (other.collider.CompareTag("scorezone"))
                canJump = false;    
        }

        public void Jump() {
            if (MinigameManager.GameOver) return;
            if (canJump) rigidbody2d.AddForce(Vector2.up * JumpForce);
        }
    }
}