using System;
using UnityEngine;

namespace Minigames.Frogger
{
    public class FrogController : MonoBehaviour
    {
        public AudioSource SoundJump;
        public Camera CurrentCamera;
        
        private MinigameManager gameManager;
        private Rigidbody2D rigidbody2d;
        private Vector2 initialPosition;
        private Vector2 screenHalfSizeWorldUnits;

        private void Start()
        {
            float orthographicSize;
            this.screenHalfSizeWorldUnits = new Vector2(
                CurrentCamera.aspect * (orthographicSize = CurrentCamera.orthographicSize),
                orthographicSize);
            
            this.gameManager = this.GetComponentInParent<MinigameManager>();
            this.rigidbody2d = this.GetComponent<Rigidbody2D>();
            this.initialPosition = this.transform.position;

            this.subscribeToEvents();
        }

        private void subscribeToEvents()
        {
            this.gameManager.ButtonEvents.OnLeftButtonPressed += HandleLeftButtonPressed;
            this.gameManager.ButtonEvents.OnRightButtonPressed += HandleRightButtonPressed;
            this.gameManager.ButtonEvents.OnUpButtonPressed += HandleUpButtonPressed;
        }

        private void OnDisable()
        {
            this.unsubscribeToEvents();
        }

        private void unsubscribeToEvents()
        {
            this.gameManager.ButtonEvents.OnLeftButtonPressed -= HandleLeftButtonPressed;
            this.gameManager.ButtonEvents.OnRightButtonPressed -= HandleRightButtonPressed;
            this.gameManager.ButtonEvents.OnUpButtonPressed -= HandleUpButtonPressed;
        }

        private void HandleUpButtonPressed()
        {
            this.moveFrog(Vector2.up);
        }

        private void HandleRightButtonPressed()
        {
            this.moveFrog(Vector2.right);
        }

        private void HandleLeftButtonPressed()
        {
            this.moveFrog(Vector2.left);
        }

        private void moveFrog(Vector2 direction)
        {
            if (this.gameManager.GameOver)
            {
                return;
            }
            
            Vector2 newPosition;
            if (direction == Vector2.up)
            {
                newPosition = rigidbody2d.position + Vector2.up;
            }
            else
            {
                var offsetX = Mathf.Clamp(
                    this.rigidbody2d.position.x + direction.x,
                    -this.screenHalfSizeWorldUnits.x + this.transform.localScale.x,
                    this.screenHalfSizeWorldUnits.x - this.transform.localScale.x);

                newPosition = new Vector2(offsetX, rigidbody2d.position.y);
            }
            
            this.SoundJump.Play();
            this.rigidbody2d.MovePosition(newPosition);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("deadzone"))
            {
                this.gameManager.Events.EventHit();
                this.transform.position = this.initialPosition;
                Destroy(other.gameObject);
            }
            else if (other.CompareTag("scorezone"))
            {
                this.gameManager.Events.EventScored(10);
                this.transform.position = this.initialPosition;
            }
        }
    }
}