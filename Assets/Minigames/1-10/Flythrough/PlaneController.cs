using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Minigames.Flythrough
{
    public class PlaneController: MonoBehaviour
    {
        public GameObject[] Lifes;
        public float MoveByInX;
        public Transform PlaneTransform;
        public float MoveSpeed;

        public float MaxLeft;
        public float MaxRight;

        public GameObject ExolosionEffect; 

        private MinigameManager gameManager;
        private Animator animator;
        private Vector2 targetPosition;
        private List<GameObject> lifes;
        private static readonly int RightPressed = Animator.StringToHash("rightPressed");
        private static readonly int LeftPressed = Animator.StringToHash("leftPressed");

        private void Start()
        {
            this.lifes = new List<GameObject>(this.Lifes);

            this.gameManager = GetComponentInParent<MinigameManager>();
            this.animator = GetComponent<Animator>();

            this.targetPosition = this.PlaneTransform.position;

            this.subscribeToEvents();
        }

        private void OnDisable()
        {
            this.unsubscribeToEvents();
        }

        private void subscribeToEvents()
        {
            this.gameManager.ButtonEvents.OnLeftButtonPressed += this.HandleLeftButtonPressed;
            this.gameManager.ButtonEvents.OnRightButtonPressed += this.HandleRightButtonPressed;
        }

        private void unsubscribeToEvents()
        {
            this.gameManager.ButtonEvents.OnLeftButtonPressed -= this.HandleLeftButtonPressed;
            this.gameManager.ButtonEvents.OnRightButtonPressed -= this.HandleRightButtonPressed;
        }

        private void HandleRightButtonPressed()
        {
            if (this.PlaneTransform.position.x > this.MaxRight)
                return;

            this.animator.SetTrigger(RightPressed);
            var position = this.PlaneTransform.position;
            this.targetPosition = new Vector2(
                position.x + this.MoveByInX,
                position.y);

            this.gameManager.SoundMove.Play();
        }

        private void HandleLeftButtonPressed()
        {
            if (this.PlaneTransform.position.x < this.MaxLeft)
                return;

            this.animator.SetTrigger(LeftPressed);
            var position = this.PlaneTransform.position;
            this.targetPosition = new Vector2(
                position.x - this.MoveByInX,
                position.y);

            this.gameManager.SoundMove.Play();
        }

        private void Update()
        {
            if (this.gameManager.GameOver) return;

            this.PlaneTransform.transform.position = Vector2.MoveTowards(
                this.PlaneTransform.transform.position,
                this.targetPosition,
                this.MoveSpeed * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("deadzone"))
            {
                var lastEntry = this.lifes.Last();
                Destroy(lastEntry);
                this.lifes.Remove(lastEntry);

                var explosion = Instantiate(this.ExolosionEffect, collision.transform.position, Quaternion.identity);
                Destroy(collision.gameObject);
                Destroy(explosion, 5.0f);
                this.gameManager.SoundExplosion.Play();
                if (this.lifes.Count == 0)
                {
                    this.gameManager.SoundDeath.Play();
                    this.gameManager.Events.EventDeath();
                }
            }
            else if (collision.CompareTag("scorezone"))
            {
                this.gameManager.SoundScore.Play();
                this.gameManager.Events.EventScored(1);
            }
        }
    }
}
