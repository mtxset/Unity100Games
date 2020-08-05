using System;
using UnityEngine;

namespace Assets.Minigames.Barreling
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
            this.BarrelMoveSpeed += accelerateBy;
        }
        public void SetBarrelStatic()
        {
            if (this.gameManager.GameOver) return;

            this.gameManager.FallingAudio.Play();

            this.transform.SetParent(this.gameManager.transform);
            this.canMove = false;
            this.rigidbody2d.gravityScale = UnityEngine.Random.Range(2, 4);
        }

        private void Start()
        {
            this.gameManager = this.GetComponentInParent<MinigameManager>();

            this.canMove = true;
            this.rigidbody2d = this.GetComponent<Rigidbody2D>();

            this.rigidbody2d.gravityScale = 0;
            if (UnityEngine.Random.Range(0, 2) > 0)
            {
                this.BarrelMoveSpeed *= -1f;
            }
        }

        private void Update()
        {
            if (!canMove || this.gameManager.GameOver)
            {
                return;
            }

            var newPostion = this.transform.position;
            newPostion.x += this.BarrelMoveSpeed * Time.deltaTime;

            if (newPostion.x > this.RightOffset)
            {
                this.BarrelMoveSpeed *= -1f;
            }
            else if (newPostion.x < this.LeftOffset)
            {
                this.BarrelMoveSpeed *= -1f;
            }

            this.transform.position = newPostion;
        }
        
        private void landed()
        {
            this.ignoreCollision = true;
            // this.ignoreTrigger = true;

            this.gameManager.Events.EventScored(this.PointPerScore);
            this.gameManager.Events.EventLanded();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (this.ignoreCollision)
            {
                return;
            }

            if (collision.gameObject.CompareTag("scorezone") 
                || collision.gameObject.CompareTag("Barrel"))
            {
                this.landed();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
/*            if (this.ignoreTrigger)
            {
                return;
            }*/

            if (collision.gameObject.CompareTag("deadzone"))
            {
                this.gameManager.Events.EventDeath();
            }
        }
    }
}
