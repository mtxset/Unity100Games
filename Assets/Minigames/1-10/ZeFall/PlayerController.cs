using UnityEngine;
using UnityEngine.InputSystem;

namespace Minigames.ZeFall
{
    class PlayerController : MonoBehaviour
    {
        public enum PlayerMovement
        {
            Idle = 0,
            Left = -1,
            Right = 1,
            Jump
        }

        private PlayerMovement playerMovement = PlayerMovement.Idle;

        public float MovementSpeed = 0;
        public float MovementOffset = 0;
        public float JumpHeight = 0;

        private MinigameManager gameManager;
        
        [SerializeField]
        private bool playerOnGround;

        private void Start()
        {
            this.gameManager = this.GetComponentInParent<MinigameManager>();

            this.subscribeToEvents();
        }
        private void Update()
        {
            this.transform.position = Vector2.MoveTowards(
                    this.transform.position,
                    new Vector2(this.transform.position.x + (this.MovementOffset * (int)this.playerMovement), this.transform.position.y),
                    this.MovementSpeed * Time.deltaTime);
        }

        private void subscribeToEvents()
        {
            this.gameManager.ButtonEvents.OnUpButtonPressed += HandleUpButtonPressed;
            this.gameManager.ButtonEvents.OnHorizontalPressed += HandleHorizontalPressed;
        }

        private void unsubscribeToEvents()
        {
            this.gameManager.ButtonEvents.OnUpButtonPressed -= HandleUpButtonPressed;
            this.gameManager.ButtonEvents.OnHorizontalPressed -= HandleHorizontalPressed;
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
            this.unsubscribeToEvents();    
        }

        private void HandleUpButtonPressed()
        {
            if (!this.playerOnGround)
                return;

            this.GetComponent<Rigidbody2D>().AddForce(Vector2.up * this.JumpHeight, ForceMode2D.Impulse);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            this.playerOnGround = collision.gameObject.CompareTag("ground") ? true : false;
        }
    }
}
