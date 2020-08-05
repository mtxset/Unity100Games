using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Minigames.InfiniteTunnels
{
    class PlayerController : MonoBehaviour
    {
        public GameObject[] Lifes = null;
        public enum PlayerMovement
        {
            Idle = 0,
            Left = -1,
            Right = 1,
        }

        public float MovementSpeed = 600f;
        public Transform CenterObject = null;

        private MinigameManager gameManager;
        private PlayerMovement playerMovement = PlayerMovement.Idle;
        private List<GameObject> lifes;

        private void Start()
        {
            this.lifes = new List<GameObject>(this.Lifes);
            this.gameManager = this.GetComponentInParent<MinigameManager>();

            this.gameManager.ButtonEvents.OnHorizontalPressed += HandleHorizontalStateChanged;
        }

        private void HandleHorizontalStateChanged(InputValue inputValue)
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

        private void FixedUpdate()
        {
            if (this.gameManager.GameOver)
            { 
                return; 
            }

            this.transform.RotateAround(
                this.CenterObject.position, 
                Vector3.forward,
                this.MovementSpeed * Time.deltaTime * -(int)this.playerMovement);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("deadzone"))
            {
                var lastEntry = this.lifes.Last();
                Destroy(lastEntry);
                this.lifes.Remove(lastEntry);

                collision.gameObject.SetActive(false);
                this.gameManager.SoundCrash.Play();

                if (this.lifes.Count == 0)
                {
                    this.gameManager.SoundDeath.Play();
                    this.gameManager.Events.EventDeath();
                }
            }
        }
    }
}
