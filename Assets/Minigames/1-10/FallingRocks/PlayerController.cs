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

        private enum PlayerMovement
        {
            Idle = 0,
            Left = -1,
            Right = 1
        }

        private PlayerMovement playerMovement = PlayerMovement.Idle;

        public float MovementSpeed;
        public float MovementOffset;
        public Camera CurrentCamera;

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
            
            
            this.position = Vector2.MoveTowards(
                    this.transform.position,
                    new Vector2(this.position.x + (this.MovementOffset * (int)this.playerMovement), this.position.y),
                    this.MovementSpeed * Time.deltaTime);

            var halfPlayerWidht = transform.localScale.x / 2f;
            if (this.transform.position.x < -this.screenOffsetTeleport + halfPlayerWidht)
            {
                this.position = new Vector2(this.screenOffsetTeleport + halfPlayerWidht, transform.position.y);
            }
            else if (this.transform.position.x > this.screenOffsetTeleport + halfPlayerWidht)
            {
                this.position = new Vector2(-this.screenOffsetTeleport + halfPlayerWidht, transform.position.y);
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
