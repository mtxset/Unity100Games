using Components;
using Components.UnityComponents;
using UnityEngine;

namespace Minigames.PingPong
{
    public class PlayerLifesController : MonoBehaviour
    {
        public GameObject[] Lifes;

        private PlayerLifes playerLifes;
        private MinigameManager gameManager;
        private void Start()
        {
            this.gameManager = this.GetComponentInParent<MinigameManager>();
            this.playerLifes = new PlayerLifes(this.Lifes);
            
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
            if (playerLifes.LoseLife())
            {
                this.gameManager.Events.EventDeath();
            }
        }
    }
}