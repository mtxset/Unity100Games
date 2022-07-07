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

        public GameObject ExplosionEffect;

        private MinigameManager gameManager;
        private Animator animator;
        private Vector2 targetPosition;
        private List<GameObject> lifes;
        private static readonly int RightPressed = Animator.StringToHash("rightPressed");
        private static readonly int LeftPressed = Animator.StringToHash("leftPressed");

        private void Start()
        {
            lifes = new List<GameObject>(Lifes);

            gameManager = GetComponentInParent<MinigameManager>();
            animator = GetComponent<Animator>();

            targetPosition = PlaneTransform.position;

            subscribeToEvents();
        }

        private void OnDisable()
        {
            unsubscribeToEvents();
        }

        private void subscribeToEvents()
        {
            gameManager.ButtonEvents.OnHorizontalPressed += HandleHorizontalStateChange;
            gameManager.ButtonEvents.OnHorizontalPressed += LocalStateChange;
        }

        private void unsubscribeToEvents()
        {
            gameManager.ButtonEvents.OnHorizontalPressed -= HandleHorizontalStateChange;
            gameManager.ButtonEvents.OnHorizontalPressed -= LocalStateChange;
        }

        private void LocalStateChange(InputValue obj)
        {
            movePlane();
        }

        private void movePlane()
        {

            if (HorizontalState == AxisState.Negative)
            {
                gameManager.SoundMove.Play();
                animator.SetTrigger(LeftPressed);
                targetPosition = new Vector2(
                    -5f,
                    targetPosition.y);
            }
            else if (HorizontalState == AxisState.Positive)
            {
                gameManager.SoundMove.Play();
                animator.SetTrigger(RightPressed);
                targetPosition = new Vector2(
                    5f,
                    targetPosition.y);
            }
            else
            {
                targetPosition = new Vector2(
                    0,
                    targetPosition.y);
            }
        }

        private void Update()
        {
            if (gameManager.GameOver) return;

            PlaneTransform.transform.position = Vector2.MoveTowards(
                PlaneTransform.transform.position,
                targetPosition,
                MoveSpeed * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("deadzone"))
            {
                var lastEntry = lifes.Last();
                Destroy(lastEntry);
                lifes.Remove(lastEntry);

                var explosion = Instantiate(
                    ExplosionEffect, collision.transform.position, Quaternion.identity);
                Destroy(collision.gameObject);
                Destroy(explosion, 5.0f);
                gameManager.SoundExplosion.Play();
                if (lifes.Count == 0)
                {
                    gameManager.SoundDeath.Play();
                    gameManager.Events.EventDeath();
                }
            }
            else if (collision.CompareTag("scorezone"))
            {
                gameManager.SoundScore.Play();
                gameManager.Events.EventScored(1);
            }
        }
    }
}
