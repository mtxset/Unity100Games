using System;
using Components.UnityComponents;
using Components.UnityComponents.v2;
using UnityEngine;

namespace Minigames.DoubleTrouble
{
    class PlayerController: BasicControls
    {
        public float MovementSpeed = 1f;

        private MinigameManager2 gameManager;

        private Vector2 initialPos;

        private void Start() {
            initialPos = transform.position;
            gameManager = GetComponentInParent<MinigameManager2>();
            subscribeToEvents();
        }

        private void FixedUpdate() {
            transform.Translate((float)HorizontalState * MovementSpeed * Time.fixedDeltaTime, (float)VerticalState * MovementSpeed * Time.fixedDeltaTime, 0f);   
        }

        private void OnDisable()
        {
            unsubscribeToEvents();
        }

        private void subscribeToEvents()
        {
            gameManager.ButtonEvents.OnHorizontalPressed += HandleHorizontalStateChange;
            gameManager.ButtonEvents.OnVerticalPressed += HandleVerticalStateChange;

            gameManager.Events.OnHit += HandleHit;
        }

        private void unsubscribeToEvents()
        {
            gameManager.ButtonEvents.OnHorizontalPressed -= HandleHorizontalStateChange;
            gameManager.ButtonEvents.OnVerticalPressed -= HandleVerticalStateChange;

            gameManager.Events.OnHit -= HandleHit;
        }

        private void HandleHit()
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            transform.position = initialPos;
        }
    }
}