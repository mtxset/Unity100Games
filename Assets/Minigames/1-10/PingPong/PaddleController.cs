using System;
using UnityEngine;

namespace Assets.Minigames.PingPong
{
    public class PaddleController : MonoBehaviour
    {
        public float PaddleMovementSpeed;

        private MinigameManager gameManager;
        private Rigidbody2D rigidBody;

        private void Start()
        {
            this.rigidBody = this.GetComponent<Rigidbody2D>();
            this.gameManager = GetComponentInParent<MinigameManager>();

            this.gameManager.ButtonEvents.OnUpButtonPressed += HandleUpButtonPressed;
            this.gameManager.ButtonEvents.OnDownButtonPressed += HandleDownButtonPressed;
        }

        private void HandleDownButtonPressed()
        {
            if (gameManager.GameOver) return;
            //this.rigidBody.AddForce(-transform.up * PaddleMovementSpeed, ForceMode2D.Impulse);
            this.rigidBody.velocity = new Vector2(this.rigidBody.velocity.x, this.PaddleMovementSpeed * -1);
        }

        private void HandleUpButtonPressed()
        {
            if (gameManager.GameOver) return;
            //this.rigidBody.AddForce(transform.up * PaddleMovementSpeed, ForceMode2D.Impulse);
            this.rigidBody.velocity = new Vector2(this.rigidBody.velocity.x, this.PaddleMovementSpeed * 1);
        }

        private void OnDisable()
        { 
            this.gameManager.ButtonEvents.OnUpButtonPressed -= HandleUpButtonPressed;
            this.gameManager.ButtonEvents.OnDownButtonPressed -= HandleDownButtonPressed;
        }
    }
}
