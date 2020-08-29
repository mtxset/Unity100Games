using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Components.UnityComponents;
using UnityEngine;

namespace Minigames.AAReplica
{
    public class MinigameManager : MinigameManagerDefault
    {
        public GameObject[] Lifes;
        public GameObject PinPrefab;
        public Transform SpawnPoint;
        public float Cooldown = 1f;
        public Animator CameraAnimation;
        public int MaxPointsBeforeReset = 15;

        private List<GameObject> liveEntities;
        private List<GameObject> lifes;

        public AudioSource SoundHit;
        public AudioSource SoundDeath;
        public AudioSource SoundPinned;

        private GameObject currentEntity;
        private bool canShoot = true;
        private static readonly int Hit = Animator.StringToHash("hit");
        private int scoreForResetting;

        protected override void UnityStart()
        {
            base.UnityStart();

            this.lifes = new List<GameObject>(this.Lifes);
            this.liveEntities = new List<GameObject>();

            this.prepareNewPin();
        }

        protected override void UnityOnDisable()
        {
            base.UnityOnDisable();

            this.UnsubscribeToEvents();
        }

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            this.Events.OnHit += HandleHit;
            this.Events.OnScored += OnScored;
            this.ButtonEvents.OnActionButtonPressed += HandleActionButtonPressed;
        }

        protected override void UnsubscribeToEvents()
        {
            base.UnsubscribeToEvents();
            this.Events.OnHit -= HandleHit;
            this.Events.OnScored -= OnScored;
            this.ButtonEvents.OnActionButtonPressed -= HandleActionButtonPressed;
        }

        private async void HandleActionButtonPressed()
        {
            if (!this.canShoot || this.GameOver)
            {
                return;
            }

            this.canShoot = false;
            currentEntity.GetComponent<Rigidbody2D>().simulated = true;

            await Task.Delay(TimeSpan.FromSeconds(this.Cooldown));

            this.prepareNewPin();
        }

        private void prepareNewPin()
        {
            if (this.GameOver)
            {
                return;
            }

            this.canShoot = true;
            var pin = this.createNewPin();
            currentEntity = pin;
            this.liveEntities.Add(pin);
        }

        private GameObject createNewPin()
        {
            var newPin = Instantiate(
                this.PinPrefab, this.SpawnPoint.position, Quaternion.identity, this.transform);

            newPin.GetComponent<Rigidbody2D>().simulated = false;

            return newPin;
        }

        private void OnScored(int obj)
        {
            this.SoundPinned.Play();

            if (++this.scoreForResetting == this.MaxPointsBeforeReset)
            {
                this.scoreForResetting = 0;
                this.removeAllPins();
            }
        }

        private void removeAllPins()
        {
            foreach (var item in this.liveEntities)
            {
                Destroy(item);
            }

            this.liveEntities.Clear();
        }
        
        private void HandleHit()
        {
            this.scoreForResetting = 0;
            if (this.lifes.Count == 0 || this.GameOver)
            {
                this.SoundDeath.Play();
                this.Events.EventDeath();
                return;
            }

            CameraAnimation.SetTrigger(Hit);
            this.removeAllPins();

            var lastEntry = this.lifes.Last();
            Destroy(lastEntry);
            this.lifes.Remove(lastEntry);
            this.SoundHit.Play();

            if (this.lifes.Count != 0 && !this.GameOver)
            {
                return;
            }
            
            this.SoundDeath.Play();
            this.Events.EventDeath();
        }
    }
}
