using UnityEngine;

namespace Minigames.PingPong
{
    public class PaddleController : MonoBehaviour
    {
        public float PaddleMovementSpeed;

        private MinigameManager gameManager;
        private Rigidbody2D rigidBody;

        private void Start()
        {
            rigidBody = GetComponent<Rigidbody2D>();
            gameManager = GetComponentInParent<MinigameManager>();

            gameManager.ButtonEvents.OnUpButtonPressed += HandleUpButtonPressed;
            gameManager.ButtonEvents.OnDownButtonPressed += HandleDownButtonPressed;
        }

        private void HandleDownButtonPressed()
        {
            if (gameManager.GameOver) return;
            //rigidBody.AddForce(-transform.up * PaddleMovementSpeed, ForceMode2D.Impulse);
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, PaddleMovementSpeed * -1);
        }

        private void HandleUpButtonPressed()
        {
            if (gameManager.GameOver) return;
            //rigidBody.AddForce(transform.up * PaddleMovementSpeed, ForceMode2D.Impulse);
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, PaddleMovementSpeed * 1);
        }

        private void OnDisable()
        { 
            gameManager.ButtonEvents.OnUpButtonPressed -= HandleUpButtonPressed;
            gameManager.ButtonEvents.OnDownButtonPressed -= HandleDownButtonPressed;
        }
    }
}
