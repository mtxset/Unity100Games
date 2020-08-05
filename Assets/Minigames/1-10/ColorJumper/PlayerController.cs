using System;
using System.CodeDom;
using UnityEngine;

namespace Assets.Minigames.ColorJumper
{
    class PlayerController : MonoBehaviour
    {
        public float JumpHeight = 10f;

        private MinigameManager gameManager;
        private Rigidbody2D rigidbody2d;
        private SpriteRenderer spriteRenderer;
        private void Start()
        {
            this.spriteRenderer = this.GetComponent<SpriteRenderer>();
            this.rigidbody2d = this.GetComponent<Rigidbody2D>();
            this.gameManager = this.GetComponentInParent<MinigameManager>();

            setPlayerToRandomColor();
            this.subscribeToEvents();
        }

        private void setPlayerToRandomColor()
        {
            var randomColorIndex = UnityEngine.Random.Range(0, this.gameManager.ColorList.Length);
            this.spriteRenderer.color = this.gameManager.ColorList[randomColorIndex];
        }

        private void OnDisable()
        {
            this.unsubscribeToEvents();
        }

        private void subscribeToEvents()
        {
            this.gameManager.ButtonEvents.OnActionButtonPressed += HandleActionButtonPressed;
        }

        private void unsubscribeToEvents()
        {
            this.gameManager.ButtonEvents.OnActionButtonPressed -= HandleActionButtonPressed;

        }

        private void HandleActionButtonPressed()
        {
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
