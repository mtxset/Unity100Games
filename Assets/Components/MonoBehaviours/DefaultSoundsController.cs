using System;
using UnityEngine;

namespace Components.MonoBehaviours
{
    public class DefaultSoundsController : MonoBehaviour
    {
        public AudioSource SoundHit;
        public AudioSource SoundDeath;
        public AudioSource SoundScored;

        private MinigameManagerDefault gameManager;

        private void Start()
        {
            this.gameManager = this.GetComponentInParent<MinigameManagerDefault>();
            
            this.subscribeToEvents();
        }
        
        private void OnDisable()
        {
            this.unsubscribeToEvents();
        }
        
        private void subscribeToEvents()
        {
            this.gameManager.Events.OnHit += HandleHit;
            this.gameManager.Events.OnDeath += HandleDeath;
            this.gameManager.Events.OnScored += HandleScored;
        }

        private void unsubscribeToEvents()
        {
            this.gameManager.Events.OnHit -= HandleHit;
            this.gameManager.Events.OnDeath -= HandleDeath;
            this.gameManager.Events.OnScored -= HandleScored;
        }

        private void HandleHit()
        {
            this.SoundHit.Play();
        }

        private void HandleDeath()
        {
            this.SoundDeath.Play();
        }

        private void HandleScored(int obj)
        {
            this.SoundScored.Play();
        }
    }
}