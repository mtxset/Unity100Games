using Components.MonoBehaviours;
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
            this.gameManager = this.GetComponentInParent<MinigameManager>();
            this.subscribeToEvents();
        }

        private void OnDisable()
        {
            this.unsubscribeToEvents();
        }

        private void subscribeToEvents()
        {
            this.gameManager.ButtonEvents.OnHorizontalPressed 
                += HandleHorizontalStateChange;
            this.gameManager.ButtonEvents.OnVerticalPressed
                += HandleVerticalStateChange;
        }

        private void unsubscribeToEvents()
        {
            this.gameManager.ButtonEvents.OnHorizontalPressed 
                -= HandleHorizontalStateChange;
            this.gameManager.ButtonEvents.OnVerticalPressed
                -= HandleVerticalStateChange;
        }

        private void movePlayer()
        {
            this.transform.Translate(
                (int) this.HorizontalState * this.FlySpeed * Time.deltaTime,
                (int) this.VerticalState * this.FlySpeed * Time.deltaTime,
                0f);
        }

        private void FixedUpdate()
        {
            if (this.gameManager.GameOver)
            {
                return;;
            }
            
            this.movePlayer();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("deadzone"))
            {
                var explosion = Instantiate(
                    this.ExolosionEffect, 
                    other.transform.position, 
                    Quaternion.identity);
                Destroy(explosion, 5.0f);
                this.gameManager.Events.EventHit();
            }
        }
    }
}
