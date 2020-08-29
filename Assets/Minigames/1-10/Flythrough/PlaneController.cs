using System.Collections.Generic;
using System.Linq;
using Components.UnityComponents;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Minigames.Flythrough
{
    public class PlaneController: BasicControls
    {
        public GameObject[] Lifes;
        public Transform PlaneTransform;
        public float MoveSpeed;


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
            this.gameManager.ButtonEvents.OnHorizontalPressed += HandleHorizontalStateChange;
            this.gameManager.ButtonEvents.OnHorizontalPressed += LocalStateChange;
        }

        private void unsubscribeToEvents()
        {
            this.gameManager.ButtonEvents.OnHorizontalPressed -= HandleHorizontalStateChange;
            this.gameManager.ButtonEvents.OnHorizontalPressed -= LocalStateChange;
        }

        private void LocalStateChange(InputValue obj)
        {
            movePlane();
        }

        private void movePlane()
        {
            
            if (HorizontalState == AxisState.Negative)
            {
                this.gameManager.SoundMove.Play();
                this.animator.SetTrigger(LeftPressed);
                this.targetPosition = new Vector2(
                    -5f,
                    this.targetPosition.y);
            }
            else if (HorizontalState == AxisState.Positive)
            {
                this.gameManager.SoundMove.Play();
                this.animator.SetTrigger(RightPressed);
                this.targetPosition = new Vector2(
                    5f,
                    this.targetPosition.y);
            }
            else
            {
                this.targetPosition = new Vector2(
                    0,
                    this.targetPosition.y);
            }
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

                var explosion = Instantiate(
                    this.ExolosionEffect, collision.transform.position, Quaternion.identity);
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
