using UnityEngine;
using Random = UnityEngine.Random;

namespace Minigames.ColorJumper
{
    internal class PlayerController : MonoBehaviour
    {
        public Camera CurrentCamera;
        public float JumpHeight = 10f;

        private MinigameManager gameManager;
        private Rigidbody2D rigidbody2d;
        private SpriteRenderer spriteRenderer;
        private bool playerReady;
        private Transform thisTransform;
        private void Start()
        {
            this.thisTransform = this.transform;
            this.spriteRenderer = this.GetComponent<SpriteRenderer>();
            this.rigidbody2d = this.GetComponent<Rigidbody2D>();
            this.gameManager = this.GetComponentInParent<MinigameManager>();

            setPlayerToRandomColor();
            this.subscribeToEvents();
            this.setPlayerReady(false);
        }
        
        /// <summary>
        /// Sets player to state
        /// </summary>
        /// <param name="readyOrNot">true ready, false not ready</param>
        private void setPlayerReady(bool readyOrNot)
        {
            if (readyOrNot)
            {
                this.playerReady = true;
                this.rigidbody2d.simulated = true;
            }
            else
            {
                this.playerReady = false;
                this.rigidbody2d.simulated = false;
            }
        }

        private void Update()
        {
            this.checkIfPlayerBelowScreen();
        }

        private void checkIfPlayerBelowScreen()
        {
            var playerPostion = this.thisTransform.position;
            var cameraPosition = this.CurrentCamera.transform.position;
            
            if (!(playerPostion.y <
                  cameraPosition.y - this.CurrentCamera.orthographicSize))
            {
                return;
            }

            this.gameManager.Events.EventHit();
            playerPostion = new Vector2(
                cameraPosition.x,
                cameraPosition.y - this.CurrentCamera.orthographicSize + (this.transform.localScale.y * 4f));

            this.thisTransform.position = playerPostion;
            
            this.setPlayerReady(false);
        }
        
        private void setPlayerToRandomColor()
        {
            var randomColorIndex = Random.Range(0, this.gameManager.ColorList.Length);
            this.spriteRenderer.color = this.gameManager.ColorList[randomColorIndex];
        }

        private void OnDisable()
        {
            this.unsubscribeToEvents();
        }

        private void subscribeToEvents()
        {
            this.gameManager.ButtonEvents.OnActionButtonPressed += HandleActionButtonPressed;
            this.gameManager.Events.OnDeath += HandleDeath;
        }

        private void unsubscribeToEvents()
        {
            this.gameManager.ButtonEvents.OnActionButtonPressed -= HandleActionButtonPressed;
            this.gameManager.Events.OnDeath -= HandleDeath;
        }

        private void HandleDeath()
        {
            this.rigidbody2d.simulated = false;
        }

        private void HandleActionButtonPressed()
        {
            if (this.gameManager.GameOver)
            {
                return;
            }
            
            if (!playerReady)
            {
                this.playerReady = true;
                this.rigidbody2d.simulated = true;
            }
            this.rigidbody2d.velocity = Vector2.up * this.JumpHeight;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("scorezone"))
            {
                if (collision.GetComponent<SpriteRenderer>().color
                    == this.spriteRenderer.color)
                {
                    collision.gameObject.SetActive(false);
                    this.gameManager.SoundScored.Play();
                    this.gameManager.Events.EventScored(1);
                }
                else
                {
                    collision.gameObject.transform.parent.gameObject.SetActive(false);
                    this.gameManager.Events.EventHit();
                }
            }
            else if (collision.CompareTag("deadzone"))
            {
                // Change color
                this.gameManager.SoundChangeColor.Play();
                this.setPlayerToRandomColor();
                Destroy(collision.gameObject);
            }
        }
    }
}
