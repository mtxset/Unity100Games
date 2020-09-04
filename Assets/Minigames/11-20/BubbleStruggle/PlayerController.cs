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
            gameManager = GetComponentInParent<MinigameManager>();
            subscribeToEvents();
        }

        private void OnDisable()
        {
            unsubscribeToEvents();
        }

        private void subscribeToEvents()
        {
            gameManager.ButtonEvents.OnHorizontalPressed 
                += HandleHorizontalStateChange;
        }

        private void unsubscribeToEvents()
        {
            gameManager.ButtonEvents.OnHorizontalPressed 
                -= HandleHorizontalStateChange;
        }

        private void FixedUpdate()
        {
            if (gameManager.GameOver)
            {
                return;
            }
            
            movePlayer();
        }

        private void movePlayer()
        {
            transform.Translate(
                (int) HorizontalState * MovementSpeed * Time.fixedDeltaTime,
                0f,
                0f);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("scorezone"))
            {
                gameManager.Events.EventHit();
                gameManager.ResetBalls();
            }
        }
    }
}