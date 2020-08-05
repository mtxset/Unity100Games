using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Minigames.FallingRocks
{
    class PlayerController : MonoBehaviour
    {
        public GameObject[] Lifes = null;
        public GameObject ExolosionEffect;
        public enum PlayerMovement
        {
            Idle = 0,
            Left = -1,
            Right = 1,
            Dodge = 2
        }

        private PlayerMovement playerMovement = PlayerMovement.Idle;

        public float MovementSpeed = 0;
        public float MovementOffset = 0;
        public float DodgeTime = 0;
        public Camera currentCamera;

        private MinigameManager gameManager;
        private float screenOffsetTeleport;
        private List<GameObject> lifes;

        private void Start()
        {
            this.lifes = new List<GameObject>(this.Lifes);
            this.screenOffsetTeleport = this.currentCamera.aspect * this.currentCamera.orthographicSize;
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

            this.transform.position = Vector2.MoveTowards(
                    this.transform.position,
                    new Vector2(this.transform.position.x + (this.MovementOffset * (int)this.playerMovement), this.transform.position.y),
                    this.MovementSpeed * Time.deltaTime);

            var halfPlayerWidht = transform.localScale.x / 2f;
            if (this.transform.position.x < -this.screenOffsetTeleport + halfPlayerWidht)
            {
                this.transform.position = new Vector2(this.screenOffsetTeleport + halfPlayerWidht, transform.position.y);
            }
            else if (this.transform.position.x > this.screenOffsetTeleport + halfPlayerWidht)
            {
                this.transform.position = new Vector2(-this.screenOffsetTeleport + halfPlayerWidht, transform.position.y);
            }
        }

        private void subscribeToEvents()
        {
            this.gameManager.ButtonEvents.OnActionButtonPressed += HandleActionButtonPressed;
            this.gameManager.ButtonEvents.OnHorizontalPressed += HandleHorizontalPressed;
        }

        private void unsubscribeToEvents()
        {
            this.gameManager.ButtonEvents.OnActionButtonPressed -= HandleActionButtonPressed;
            this.gameManager.ButtonEvents.OnHorizontalPressed -= HandleHorizontalPressed;
        }

        private void HandleActionButtonPressed()
        {
            throw new NotImplementedException();
        }

        private void HandleHorizontalPressed(InputValue inputValue)
        {
            switch (inputValue.Get<float>())
            {
                case -1:
                    playerMovement = PlayerMovement.Left;
                    break;
                case 0:
                    playerMovement = PlayerMovement.Idle;
                    break;
                case 1:
                    playerMovement = PlayerMovement.Right;
                    break;
            }
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
