using System;
using System.Threading.Tasks;
using Components.UnityComponents;
using UnityEngine;

namespace Minigames.FallingBlocks
{
    public class PlayerController : BasicControls
    {
        public float MovementSpeed = 1f;
        public Transform MaxXOffset;

        private Rigidbody2D rigidbody2d;
        private MinigameManager gameManager;

        private void Start()
        {
            this.rigidbody2d = this.GetComponent<Rigidbody2D>();
            this.gameManager = this.GetComponentInParent<MinigameManager>();
            this.subscribeToEvents();
        }

        private void OnDisable()
        {
            this.unsubscribeToEvents();
        }
        
        private void subscribeToEvents()
        {
            this.gameManager.ButtonEvents.OnHorizontalPressed 
                += HandleHorizontalStateChange;
        }

        private void unsubscribeToEvents()
        {
            this.gameManager.ButtonEvents.OnHorizontalPressed 
                -= HandleHorizontalStateChange;
        }
        
        private void movePlayer()
        {
            var movement = (float) this.HorizontalState * Time.fixedDeltaTime * this.MovementSpeed;

            var newPosition = this.rigidbody2d.position + Vector2.right * movement;

            var bodyOffset = this.transform.localScale.x;
            var maxX = MaxXOffset.position;
            
            newPosition.x = Mathf.Clamp(
                newPosition.x, 
                -maxX.x + bodyOffset, 
                maxX.x - bodyOffset);
            
            this.rigidbody2d.MovePosition(newPosition);
        }
        
        private void FixedUpdate()
        {
            if (this.gameManager.GameOver)
            {
                return;
            }
            
            this.movePlayer();
        }

        private async void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("deadzone"))
            {
                /*Time.timeScale = 1f / this.Slowness;
                Time.fixedDeltaTime /= Slowness;

                await Task.Delay(TimeSpan.FromSeconds(1));

                Time.timeScale = 1f;
                Time.fixedDeltaTime *= Slowness;*/
                
                this.gameManager.Events.EventHit();
            }
        }
    }
    
}