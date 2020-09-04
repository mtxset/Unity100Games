using Components;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Minigames.FallForever
{
    public class Player : AddMinigameManager2
    {
        public AudioSource SoundBump;
        public float MovementOffset = 0.1f;
        public float PlatformOffset;
        public float MovementSpeed;
        
        private Rigidbody2D rigidbody2d;
        private Vector3 initialPosition;
        private void Start()
        {
            initialPosition = transform.position; 
            rigidbody2d = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (MinigameManager.GameOver)
            {
                rigidbody2d.simulated = false;
                return;
            }
            
            checkIfFell();
            
            var currentPos = transform.position;
            transform.position = Vector2.MoveTowards(
                transform.position,
                new Vector2(
                    currentPos.x + (MovementOffset * (int) MinigameManager.Controls.HorizontalState) + PlatformOffset, 
                    currentPos.y),
                MovementSpeed * Time.fixedDeltaTime);
        }

        private void checkIfFell()
        {
            if (transform.position.y < 
                -MinigameManager.CurrentCamera.orthographicSize + MinigameManager.transform.position.y)
            {
                rigidbody2d.velocity = Vector2.zero;
                transform.position = initialPosition;
                MinigameManager.Events.EventHit();
            }
        }

        public void Jump(float jumpPower)
        {
            rigidbody2d.velocity = new Vector2(
                rigidbody2d.velocity.x, jumpPower);
        }

        public void RandomJump(float jumpPower, float maxXOffset)
        {
            rigidbody2d.velocity = new Vector2(
                Random.Range(-maxXOffset, maxXOffset), 
                jumpPower);
        }

        private void OnCollisionEnter2D()
        {
            SoundBump.Play();
            MinigameManager.Events.EventScored();
        }

        private void OnCollisionExit2D()
        {
            PlatformOffset = 0;
        }
    }
}