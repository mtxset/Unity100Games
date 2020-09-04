using Components.UnityComponents;
using UnityEngine;

namespace Minigames.AvoidRocket
{
    internal class SpaceShipController : BasicControls
    {
        public float FlySpeed = 1f;
        private MinigameManager gameManager;
        public GameObject ExolosionEffect;
        private void Start()
        {
            gameManager = GetComponentInParent<MinigameManager>();
            subscribeToEvents();
        }

        private void OnDisable()
        {
            unsubscribeToEvents();
        }

        private void subscribeToEvents()
        {
            gameManager.ButtonEvents.OnHorizontalPressed 
                += HandleHorizontalStateChange;
            gameManager.ButtonEvents.OnVerticalPressed
                += HandleVerticalStateChange;
        }

        private void unsubscribeToEvents()
        {
            gameManager.ButtonEvents.OnHorizontalPressed 
                -= HandleHorizontalStateChange;
            gameManager.ButtonEvents.OnVerticalPressed
                -= HandleVerticalStateChange;
        }

        private void movePlayer()
        {
            transform.Translate(
                (int) HorizontalState * FlySpeed * Time.deltaTime,
                (int) VerticalState * FlySpeed * Time.deltaTime,
                0f);
        }

        private void FixedUpdate()
        {
            if (gameManager.GameOver)
            {
                return;
            }
            
            movePlayer();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("deadzone"))
            {
                var explosion = Instantiate(
                    ExolosionEffect, 
                    other.transform.position, 
                    Quaternion.identity);
                Destroy(explosion, 5.0f);
                gameManager.Events.EventHit();
            }
        }
    }
}
