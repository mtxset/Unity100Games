using DefaultImplementations;
using UnityEngine;

namespace Minigames.AvoidRocket
{
    class SpaceShipController : BasicControls
    {

        public float FlySpeed = 1f;
        private MinigameManager gameManager;
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

        private void Update()
        {
            this.transform.Translate(
                (int)this.HorizontalState * this.FlySpeed * Time.deltaTime,
                (int)this.VerticalState * this.FlySpeed * Time.deltaTime, 
                0f);
        }
    }
}
