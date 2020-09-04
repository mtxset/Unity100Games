using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Minigames.FallingRocks
{
    internal class PlayerController : MonoBehaviour
    {
        public GameObject[] Lifes;
        public GameObject ExolosionEffect;


        public float MovementSpeed;
        public float MovementOffset;
        public Camera CurrentCamera;

        private float playerMovement;
        private MinigameManager gameManager;
        private float screenOffsetTeleport;
        private List<GameObject> lifes;
        private Vector3 position;

        private void Start()
        {
            position = transform.position;
            lifes = new List<GameObject>(Lifes);
            screenOffsetTeleport = CurrentCamera.aspect * CurrentCamera.orthographicSize;
            gameManager = GetComponentInParent<MinigameManager>();
            subscribeToEvents();
        }

        private void OnDisable()
        {
            unsubscribeToEvents();
        }

        private void Update()
        {
            if (gameManager.GameOver)
            {
                return;
            }
            movePlayer();
            
        }
        
        private void teleportIfBeyondBounds()
        {
            var halfPlayerWidht = transform.localScale.x / 2f;
            if (transform.position.x < -screenOffsetTeleport - halfPlayerWidht)
            {
                position = new Vector2(screenOffsetTeleport - halfPlayerWidht * 2, transform.position.y);
            }
            else if (transform.position.x > screenOffsetTeleport + halfPlayerWidht)
            {
                position = new Vector2(-screenOffsetTeleport + halfPlayerWidht * 2, transform.position.y);
            }
        }

        private void movePlayer()
        {
            position = Vector2.MoveTowards(
                transform.position,
                new Vector2(position.x + (MovementOffset * (int) playerMovement), position.y),
                MovementSpeed * Time.deltaTime);

            teleportIfBeyondBounds();
            transform.position = position;
        }

        private void subscribeToEvents()
        {
            gameManager.ButtonEvents.OnHorizontalPressed += HandleHorizontalPressed;
        }

        private void unsubscribeToEvents()
        {
            gameManager.ButtonEvents.OnHorizontalPressed -= HandleHorizontalPressed;
        }
        
        private void HandleHorizontalPressed(InputValue inputValue)
        {
            playerMovement = inputValue.Get<float>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("deadzone"))
            {
                var lastEntry = lifes.Last();
                Destroy(lastEntry);
                lifes.Remove(lastEntry);

                collision.gameObject.SetActive(false);
                var explosion = Instantiate(
                    ExolosionEffect, 
                    collision.transform.position, 
                    Quaternion.identity);
                gameManager.SoundHit.Play();
                Destroy(explosion, 5.0f);

                if (lifes.Count == 0)
                {
                    gameManager.SoundDeath.Play();
                    gameManager.Events.EventDeath();
                }
            }
        }

    }
}
