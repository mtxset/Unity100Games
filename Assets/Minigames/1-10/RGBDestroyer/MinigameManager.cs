﻿using System.Collections.Generic;
using System.Linq;
using Components.MonoBehaviours;
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

            this.lifes = new List<GameObject>(this.Lifes);
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
