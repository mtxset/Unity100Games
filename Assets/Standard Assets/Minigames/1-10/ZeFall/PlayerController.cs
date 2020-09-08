using UnityEngine;
using UnityEngine.InputSystem;

namespace Minigames.ZeFall
{
    class PlayerController : MonoBehaviour
    {
        private enum PlayerMovement
        {
            Idle = 0,
            Left = -1,
            Right = 1,
        }

        private PlayerMovement playerMovement = PlayerMovement.Idle;

        public float MovementSpeed;
        public float MovementOffset;
        public float JumpHeight;

        private MinigameManager gameManager;
        
        private bool playerOnGround;

        private void Start()
        {
            gameManager = GetComponentInParent<MinigameManager>();

            subscribeToEvents();
        }
        private void Update()
        {
            var position = transform.position;
            position = Vector2.MoveTowards(
                    position,
                    new Vector2(position.x + (MovementOffset * (int)playerMovement), position.y),
                    MovementSpeed * Time.deltaTime);
            transform.position = position;
        }

        private void subscribeToEvents()
        {
            gameManager.ButtonEvents.OnUpButtonPressed += HandleUpButtonPressed;
            gameManager.ButtonEvents.OnHorizontalPressed += HandleHorizontalPressed;
        }

        private void unsubscribeToEvents()
        {
            gameManager.ButtonEvents.OnUpButtonPressed -= HandleUpButtonPressed;
            gameManager.ButtonEvents.OnHorizontalPressed -= HandleHorizontalPressed;
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
        private void OnDisable()
        {
            unsubscribeToEvents();    
        }

        private void HandleUpButtonPressed()
        {
            if (!playerOnGround)
                return;

            GetComponent<Rigidbody2D>().AddForce(Vector2.up * JumpHeight, ForceMode2D.Impulse);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            playerOnGround = collision.gameObject.CompareTag("ground");
        }
    }
}
