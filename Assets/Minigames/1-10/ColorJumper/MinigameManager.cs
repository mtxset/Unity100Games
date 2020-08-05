using Assets.DefaultImplementations;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Minigames.ColorJumper
{
    public class MinigameManager : MinigameManagerDefault
    {
        public GameObject[] Lifes = null;

        [Tooltip("MAX 4 colors")]
        public Color[] ColorList;

        public AudioSource SoundChangeColor;
        public AudioSource SoundHit;
        public AudioSource SoundDeath;
        public AudioSource SoundScored;

        private List<GameObject> lifes;

        public override void UnityStart()
        {
            base.UnityStart();

            this.lifes = new List<GameObject>(this.Lifes);
        }

        public override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            this.Events.OnHit += HandleHit;
        }

        public override void UnsubscribeToEvents()
        {
            base.UnsubscribeToEvents();
            this.Events.OnHit -= HandleHit;
        }

        private void HandleHit()
        {
            var lastEntry = this.lifes.Last();
            Destroy(lastEntry);
            this.lifes.Remove(lastEntry);
            this.SoundHit.Play();

            if (this.lifes.Count == 0)
            {
                this.SoundDeath.Play();
                this.Events.EventDeath();
            }
        }
    }
}
