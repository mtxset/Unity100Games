using UnityEngine;

namespace Minigames.Barreling
{
    public class Barrel : MonoBehaviour
    { 
        public float BarrelMoveSpeed;
        public float LeftOffset = -2.71f;
        public float RightOffset = 2.71f;
        public int PointPerScore = 1;

        private bool canMove;
        private bool ignoreCollision;
        private bool ignoreTrigger;

        private Rigidbody2D rigidbody2d;
        private MinigameManager gameManager;

        public void AccelerateBarrelMovement(float accelerateBy)
        {
            BarrelMoveSpeed += accelerateBy;
        }
        public void SetBarrelStatic()
        {
            if (gameManager.GameOver) return;

            gameManager.FallingAudio.Play();

            transform.SetParent(gameManager.transform);
            canMove = false;
            rigidbody2d.gravityScale = Random.Range(2, 4);
        }

        private void Start()
        {
            gameManager = GetComponentInParent<MinigameManager>();

            canMove = true;
            rigidbody2d = GetComponent<Rigidbody2D>();

            rigidbody2d.gravityScale = 0;
            if (Random.Range(0, 2) > 0)
            {
                BarrelMoveSpeed *= -1f;
            }
        }

        private void FixedUpdate()
        {
            if (!canMove || gameManager.GameOver)
            {
                return;
            }

            var newPostion = transform.position;
            newPostion.x += BarrelMoveSpeed * Time.fixedDeltaTime;

            if (newPostion.x > RightOffset)
            {
                BarrelMoveSpeed *= -1f;
            }
            else if (newPostion.x < LeftOffset)
            {
                BarrelMoveSpeed *= -1f;
            }

            transform.position = newPostion;
        }
        
        private void landed()
        {
            ignoreCollision = true;

            gameManager.Events.EventScored(PointPerScore);
            gameManager.Events.EventLanded();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (ignoreCollision)
            {
                return;
            }

            if (collision.gameObject.CompareTag("scorezone") 
                || collision.gameObject.CompareTag("Barrel"))
            {
                landed();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("deadzone"))
            {
                gameManager.Events.EventDeath(gameObject);
            }
        }
    }
}
