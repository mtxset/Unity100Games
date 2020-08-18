using System;
using UnityEngine;

namespace Minigames.Frogger
{
    public class FrogController : MonoBehaviour
    {
        private MinigameManager gameManager;
        private Rigidbody2D rigidbody2d;
        private Vector2 initialPosition;

        private void Start()
        {
            this.gameManager = this.GetComponentInParent<MinigameManager>();
            this.rigidbody2d = this.GetComponent<Rigidbody2D>();
            this.initialPosition = this.transform.position;

            this.subscribeToEvents();
        }

        private void subscribeToEvents()
        {
            this.gameManager.ButtonEvents.OnLeftButtonPressed += HandleLeftButtonPressed;
            this.gameManager.ButtonEvents.OnRightButtonPressed += HandleRightButtonPressed;
            this.gameManager.ButtonEvents.OnUpButtonPressed += HandleUpButtonPressed;
        }

        private void OnDisable()
        {
            this.unsubscribeToEvents();
        }

        private void unsubscribeToEvents()
        {
            this.gameManager.ButtonEvents.OnLeftButtonPressed -= HandleLeftButtonPressed;
            this.gameManager.ButtonEvents.OnRightButtonPressed -= HandleRightButtonPressed;
            this.gameManager.ButtonEvents.OnUpButtonPressed -= HandleUpButtonPressed;
        }

        private void HandleUpButtonPressed()
        {
            this.rigidbody2d.MovePosition(rigidbody2d.position + Vector2.up);
        }

        private void HandleRightButtonPressed()
        {
            this.rigidbody2d.MovePosition(rigidbody2d.position + Vector2.right);
        }

        private void HandleLeftButtonPressed()
        {
            this.rigidbody2d.MovePosition(rigidbody2d.position + Vector2.left);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("deadzone"))
            {
                this.gameManager.Events.EventHit();
                this.transform.position = this.initialPosition;
                Destroy(other.gameObject);
            }
            else if (other.CompareTag("scorezone"))
            {
                this.gameManager.Events.EventScored(10);
                this.transform.position = this.initialPosition;
            }
        }
    }
}