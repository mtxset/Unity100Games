using Components.UnityComponents;
using UnityEngine;

namespace Minigames.RoadDodger
{
    public class CarController : BasicControls
    {
        public GameObject Explosion;
        public Transform MaxXOffset;
        public float MoveSpeed;

        private MinigameManager gameManager;
        private Rigidbody2D rigidbody2d;

        private void Start()
        {
            rigidbody2d = GetComponent<Rigidbody2D>();
            gameManager = GetComponentInParent<MinigameManager>();
            gameManager.ButtonEvents.OnHorizontalPressed += HandleHorizontalStateChange;
        }

        private void OnDisable()
        {
            gameManager.ButtonEvents.OnHorizontalPressed -= HandleHorizontalStateChange;
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
            var movement = (float) HorizontalState * Time.fixedDeltaTime * MoveSpeed;

            var newPosition = rigidbody2d.position + Vector2.right * movement;

            var bodyOffset = transform.localScale.x;
            var maxX = MaxXOffset.position;
            
            newPosition.x = Mathf.Clamp(
                newPosition.x, 
                -maxX.x + bodyOffset, 
                maxX.x - bodyOffset);
            
            rigidbody2d.MovePosition(newPosition);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("deadzone"))
            {
                Destroy(
                    Instantiate(Explosion, other.transform.position, Quaternion.identity),
                    5f);
                
                Destroy(other.gameObject);
                gameManager.Events.EventHit(); 
            }
        }
    }
}