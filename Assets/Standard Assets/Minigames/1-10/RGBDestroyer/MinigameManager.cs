using System.Collections.Generic;
using System.Linq;
using Components.UnityComponents.v1;
using UnityEngine;

namespace Minigames.RGBDestroyer
{
    public class MinigameManager : MinigameManagerDefault
    {
        public GameObject[] Lifes;

        public AudioSource SoundSpawn;
        public AudioSource SoundHit;
        public AudioSource SoundDeath;
        public AudioSource SoundScored;

        private List<GameObject> lifes;

        protected override void UnityStart()
        {
            base.UnityStart();

            lifes = new List<GameObject>(Lifes);
        }

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            Events.OnHit += HandleHit;
        }

        protected override void UnsubscribeToEvents()
        {
            base.UnsubscribeToEvents();
            Events.OnHit -= HandleHit;
        }

        private void HandleHit()
        {
            var lastEntry = lifes.Last();
            Destroy(lastEntry);
            lifes.Remove(lastEntry);
            SoundHit.Play();

            if (lifes.Count == 0)
            {
                SoundDeath.Play();
                Events.EventDeath();
            }
        }
    }
}
