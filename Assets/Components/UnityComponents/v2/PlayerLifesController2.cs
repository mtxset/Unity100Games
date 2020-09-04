using UnityEngine;

namespace Components.UnityComponents.v2
{
    public class PlayerLifesController2 : MonoBehaviour
    {
        public GameObject[] Lifes;

        private Lifes lifes;
        private MinigameManager2 gameManager;
        private void Start()
        {
            gameManager = GetComponentInParent<MinigameManager2>();
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