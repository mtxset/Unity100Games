using System;
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
            this.position = this.transform.position;
            this.lifes = new List<GameObject>(this.Lifes);
            this.screenOffsetTeleport = this.CurrentCamera.aspect * this.CurrentCamera.orthographicSize;
            this.gameManager = this.GetComponentInParent<MinigameManager>();
            this.subscribeToEvents();
        }

        private void OnDisable()
        {
            this.unsubscribeToEvents();
        }

        private void Update()
        {
            if (this.gameManager.GameOver)
            {
                return;
            }
            this.movePlayer();
            
        }

        private void FixedUpdate()
        {
            // this.movePlayer();
        }

        private void teleportIfBeyondBounds()
        {
            var halfPlayerWidht = transform.localScale.x / 2f;
            if (this.transform.position.x < -this.screenOffsetTeleport - halfPlayerWidht)
            {
                this.position = new Vector2(this.screenOffsetTeleport - halfPlayerWidht * 2, transform.position.y);
            }
            else if (this.transform.position.x > this.screenOffsetTeleport + halfPlayerWidht)
            {
                this.position = new Vector2(-this.screenOffsetTeleport + halfPlayerWidht * 2, transform.position.y);
            }
        }

        private void movePlayer()
        {
            this.position = Vector2.MoveTowards(
                this.transform.position,
                new Vector2(this.position.x + (this.MovementOffset * (int) this.playerMovement), this.position.y),
                this.MovementSpeed * Time.deltaTime);

            this.teleportIfBeyondBounds();
            this.transform.position = this.position;
        }

        private void subscribeToEvents()
        {
            this.gameManager.ButtonEvents.OnHorizontalPressed += HandleHorizontalPressed;
        }

        private void unsubscribeToEvents()
        {
            this.gameManager.ButtonEvents.OnHorizontalPressed -= HandleHorizontalPressed;
        }
        
        private void HandleHorizontalPressed(InputValue inputValue)
        {
            this.playerMovement = inputValue.Get<float>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("deadzone"))
            {
                var lastEntry = this.lifes.Last();
                Destroy(lastEntry);
                this.lifes.Remove(lastEntry);

                collision.gameObject.SetActive(false);
                var explosion = Instantiate(
                    this.ExolosionEffect, 
                    collision.transform.position, 
                    Quaternion.identity);
                this.gameManager.SoundHit.Play();
                Destroy(explosion, 5.0f);

                if (this.lifes.Count == 0)
                {
                    this.gameManager.SoundDeath.Play();
                    this.gameManager.Events.EventDeath();
                }
            }
        }

    }
}
