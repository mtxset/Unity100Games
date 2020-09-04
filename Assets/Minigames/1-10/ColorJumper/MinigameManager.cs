using Components;
using Components.UnityComponents.v1;
using UnityEngine;

namespace Minigames.ColorJumper
{
    public class MinigameManager : MinigameManagerDefault
    {
        public GameObject[] Lifes;

        [Tooltip("MAX 4 colors")]
        public Color[] ColorList;

        public AudioSource SoundChangeColor;
        public AudioSource SoundHit;
        public AudioSource SoundDeath;
        public AudioSource SoundScored;

        private Lifes lifes;

        protected override void UnityStart()
        {
            base.UnityStart();

            lifes = new Lifes(Lifes);
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
            SoundHit.Play();

            if (lifes.LoseLife())
            {
                SoundDeath.Play();
                Events.EventDeath();
            }
        }
    }
}
