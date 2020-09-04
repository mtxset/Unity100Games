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
            thisTransform = transform;
            spriteRenderer = GetComponent<SpriteRenderer>();
            rigidbody2d = GetComponent<Rigidbody2D>();
            gameManager = GetComponentInParent<MinigameManager>();

            setPlayerToRandomColor();
            subscribeToEvents();
            setPlayerReady(false);
        }
        
        /// <summary>
        /// Sets player to state
        /// </summary>
        /// <param name="readyOrNot">true ready, false not ready</param>
        private void setPlayerReady(bool readyOrNot)
        {
            if (readyOrNot)
            {
                playerReady = true;
                rigidbody2d.simulated = true;
            }
            else
            {
                playerReady = false;
                rigidbody2d.simulated = false;
            }
        }

        private void Update()
        {
            checkIfPlayerBelowScreen();
        }

        private void checkIfPlayerBelowScreen()
        {
            var playerPostion = thisTransform.position;
            var cameraPosition = CurrentCamera.transform.position;
            
            if (!(playerPostion.y <
                  cameraPosition.y - CurrentCamera.orthographicSize))
            {
                return;
            }

            gameManager.Events.EventHit();
            playerPostion = new Vector2(
                cameraPosition.x,
                cameraPosition.y - CurrentCamera.orthographicSize + (transform.localScale.y * 4f));

            thisTransform.position = playerPostion;
            
            setPlayerReady(false);
        }
        
        private void setPlayerToRandomColor()
        {
            var randomColorIndex = Random.Range(0, gameManager.ColorList.Length);
            spriteRenderer.color = gameManager.ColorList[randomColorIndex];
        }

        private void OnDisable()
        {
            unsubscribeToEvents();
        }

        private void subscribeToEvents()
        {
            gameManager.ButtonEvents.OnActionButtonPressed += HandleActionButtonPressed;
            gameManager.Events.OnDeath += HandleDeath;
        }

        private void unsubscribeToEvents()
        {
            gameManager.ButtonEvents.OnActionButtonPressed -= HandleActionButtonPressed;
            gameManager.Events.OnDeath -= HandleDeath;
        }

        private void HandleDeath()
        {
            rigidbody2d.simulated = false;
        }

        private void HandleActionButtonPressed()
        {
            if (gameManager.GameOver)
            {
                return;
            }
            
            if (!playerReady)
            {
                playerReady = true;
                rigidbody2d.simulated = true;
            }
            rigidbody2d.velocity = Vector2.up * JumpHeight;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("scorezone"))
            {
                if (collision.GetComponent<SpriteRenderer>().color
                    == spriteRenderer.color)
                {
                    collision.gameObject.SetActive(false);
                    gameManager.SoundScored.Play();
                    gameManager.Events.EventScored();
                }
                else
                {
                    collision.gameObject.transform.parent.gameObject.SetActive(false);
                    gameManager.Events.EventHit();
                }
            }
            else if (collision.CompareTag("deadzone"))
            {
                // Change color
                gameManager.SoundChangeColor.Play();
                setPlayerToRandomColor();
                Destroy(collision.gameObject);
            }
        }
    }
}
