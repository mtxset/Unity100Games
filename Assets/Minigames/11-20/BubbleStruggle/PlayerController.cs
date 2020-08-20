using System;
using Components.UnityComponents;
using UnityEngine;

namespace Minigames.BubbleStruggle
{
    public class PlayerController : BasicControls
    {
        public float MovementSpeed = 4f;
        
        private MinigameManager gameManager;

        private void Start()
        {
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
            this.transform.Translate(
                (int) this.HorizontalState * this.MovementSpeed * Time.fixedDeltaTime,
                0f,
                0f);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("scorezone"))
            {
                this.gameManager.Events.EventHit();
                this.gameManager.ResetBalls();
            }
        }
    }
}