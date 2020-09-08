using UnityEngine;

namespace Components.UnityComponents.v1
{
    public class DefaultSoundsController : MonoBehaviour
    {
        public AudioSource SoundHit;
        public AudioSource SoundDeath;
        public AudioSource SoundScored;

        private MinigameManagerDefault gameManager;

        private void Start()
        {
            gameManager = GetComponentInParent<MinigameManagerDefault>();
            
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