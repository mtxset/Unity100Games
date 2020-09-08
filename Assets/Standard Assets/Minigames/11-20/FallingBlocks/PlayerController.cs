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
            rigidbody2d = GetComponent<Rigidbody2D>();
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
        
        private void movePlayer()
        {
            var movement = (float) HorizontalState * Time.fixedDeltaTime * MovementSpeed;

            var newPosition = rigidbody2d.position + Vector2.right * movement;

            var bodyOffset = transform.localScale.x;
            var maxX = MaxXOffset.position;
            
            newPosition.x = Mathf.Clamp(
                newPosition.x, 
                -maxX.x + bodyOffset, 
                maxX.x - bodyOffset);
            
            rigidbody2d.MovePosition(newPosition);
        }
        
        private void FixedUpdate()
        {
            if (gameManager.GameOver)
            {
                return;
            }
            
            movePlayer();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("deadzone"))
            {
                gameManager.Events.EventHit();
            }
        }
    }
    
}