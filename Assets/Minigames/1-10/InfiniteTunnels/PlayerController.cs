using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Minigames.InfiniteTunnels
{
    internal class PlayerController : MonoBehaviour
    {
        public GameObject[] Lifes;

        private enum PlayerMovement
        {
            Idle = 0,
            Left = -1,
            Right = 1,
        }

        public float MovementSpeed = 600f;
        public Transform CenterObject;

        private MinigameManager gameManager;
        private PlayerMovement playerMovement = PlayerMovement.Idle;
        private List<GameObject> lifes;

        private void Start()
        {
            lifes = new List<GameObject>(Lifes);
            gameManager = GetComponentInParent<MinigameManager>();

            gameManager.ButtonEvents.OnHorizontalPressed += HandleHorizontalStateChanged;
        }

        private void OnDisable()
        {
            gameManager.ButtonEvents.OnHorizontalPressed -= HandleHorizontalStateChanged;
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
            if (gameManager.GameOver)
            { 
                return; 
            }

            transform.RotateAround(
                CenterObject.position, 
                Vector3.forward,
                MovementSpeed * Time.deltaTime * -(int)playerMovement);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("deadzone"))
            {
                var lastEntry = lifes.Last();
                Destroy(lastEntry);
                lifes.Remove(lastEntry);

                collision.gameObject.SetActive(false);
                gameManager.SoundCrash.Play();

                if (lifes.Count == 0)
                {
                    gameManager.SoundDeath.Play();
                    gameManager.Events.EventDeath();
                }
            }
        }
    }
}
