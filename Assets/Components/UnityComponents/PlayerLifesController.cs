using UnityEngine;

namespace Components.UnityComponents
{
    public class PlayerLifesController : MonoBehaviour
    {
        public GameObject[] Lifes;

        private Lifes lifes;
        private MinigameManagerDefault gameManager;
        private void Start()
        {
            this.gameManager = this.GetComponentInParent<MinigameManagerDefault>();
            this.lifes = new Lifes(this.Lifes);
            
            this.subscribeToEvents();
        }

        private void OnDisable()
        {
            this.unsubscribeToEvents();
        }

        private void subscribeToEvents()
        {
            this.gameManager.Events.OnHit += HandleHit;
        }

        private void unsubscribeToEvents()
        {
            this.gameManager.Events.OnHit -= HandleHit;
        }

        private void HandleHit()
        {
            if (lifes.LoseLife())
            {
                this.gameManager.Events.EventDeath();
            }
        }
    }
}