using UnityEngine;

namespace Components.UnityComponents.v2
{
    public class SoundsController2 : MonoBehaviour
    {
        public AudioSource SoundHit;
        public AudioSource SoundDeath;
        public AudioSource SoundScored;

        private MinigameManager2 gameManager;

        private void Start()
        {
            gameManager = GetComponentInParent<MinigameManager2>();
            
            subscribeToEvents();
        }
        
        private void OnDisable()
        {
            unsubscribeToEvents();
        }
        
        private void subscribeToEvents()
        {
            gameManager.Events.OnHit += HandleHit;
            gameManager.Events.OnDeath += HandleDeath;
            gameManager.Events.OnScored += HandleScored;
        }

        private void unsubscribeToEvents()
        {
            gameManager.Events.OnHit -= HandleHit;
            gameManager.Events.OnDeath -= HandleDeath;
            gameManager.Events.OnScored -= HandleScored;
        }

        private void HandleHit()
        {
            SoundHit.Play();
        }

        private void HandleDeath()
        {
            SoundDeath.Play();
        }

        private void HandleScored(int obj)
        {
            SoundScored.Play();
        }
    }
}