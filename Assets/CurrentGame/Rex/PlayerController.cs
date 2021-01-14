using UnityEngine;

namespace Minigames.Rex {
    public class PlayerController : MonoBehaviour {

        public float JumpForce = 4f;

        private bool canJump = true;
        private Rigidbody2D rigidbody2d;

        private void Start() {
            rigidbody2d = GetComponent<Rigidbody2D>();
        }

        private void OnCollisionEnter2D(Collision2D other) {
            if (other.collider.CompareTag("scorezone"))
                canJump = true;
        }

        private void OnCollisionExit2D(Collision2D other) {
            if (other.collider.CompareTag("scorezone"))
                canJump = false;    
        }

        public void Jump() {
            if (canJump) rigidbody2d.AddForce(Vector2.up * JumpForce);
        }
    }
}