using UnityEngine;

namespace Components.UnityComponents.v1
{
    public class PlayerLifesController : MonoBehaviour
    {
        public GameObject[] Lifes;

        private Lifes lifes;
        private MinigameManagerDefault gameManager;
        private void Start()
        {
            gameManager = GetComponentInParent<MinigameManagerDefault>();
            lifes = new Lifes(Lifes);
            
            subscribeToEvents();
        }

        private void OnDisable()
        {
            unsubscribeToEvents();
        }

        private void subscribeToEvents()
        {
            gameManager.Events.OnHit += HandleHit;
        }

        private void unsubscribeToEvents()
        {
            gameManager.Events.OnHit -= HandleHit;
        }

        private void HandleHit()
        {
            if (lifes.LoseLife())
            {
                gameManager.Events.EventDeath();
            }
        }
    }
}