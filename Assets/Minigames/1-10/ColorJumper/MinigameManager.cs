using Components;
using Components.UnityComponents;
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

        private PlayerLifes playerLifes;

        protected override void UnityStart()
        {
            base.UnityStart();

            this.playerLifes = new PlayerLifes(this.Lifes);
        }

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            this.Events.OnHit += HandleHit;
        }

        protected override void UnsubscribeToEvents()
        {
            base.UnsubscribeToEvents();
            this.Events.OnHit -= HandleHit;
        }

        private void HandleHit()
        {
            this.SoundHit.Play();

            if (this.playerLifes.LoseLife())
            {
                this.SoundDeath.Play();
                this.Events.EventDeath();
            }
        }
    }
}
