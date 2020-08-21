using System;
using Components.UnityComponents;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Minigames.RoadDodger
{
    public class CarController : BasicControls
    {
        public Transform MaxXOffset;
        public float MoveSpeed;

        private MinigameManager gameManager;
        private Rigidbody2D rigidbody2d;

        private void Start()
        {
            this.rigidbody2d = this.GetComponent<Rigidbody2D>();
            this.gameManager = this.GetComponentInParent<MinigameManager>();
            this.gameManager.ButtonEvents.OnHorizontalPressed += HandleHorizontalStateChange;
        }

        private void OnDisable()
        {
            this.gameManager.ButtonEvents.OnHorizontalPressed -= HandleHorizontalStateChange;
        }

        private void FixedUpdate()
        {
            if (this.gameManager.GameOver)
            {
                return;
            }
            
            this.movePlayer();
        }
        
        private void movePlayer()
        {
            var movement = (float) this.HorizontalState * Time.fixedDeltaTime * this.MoveSpeed;

            var newPosition = this.rigidbody2d.position + Vector2.right * movement;

            var bodyOffset = this.transform.localScale.x;
            var maxX = MaxXOffset.position;
            
            newPosition.x = Mathf.Clamp(
                newPosition.x, 
                -maxX.x + bodyOffset, 
                maxX.x - bodyOffset);
            
            this.rigidbody2d.MovePosition(newPosition);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("deadzone"))
            {
                Destroy(other);
                this.gameManager.Events.EventHit(); 
            }
        }
    }
}