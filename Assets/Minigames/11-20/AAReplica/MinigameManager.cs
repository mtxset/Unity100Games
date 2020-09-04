using System.Collections.Generic;
using System.Linq;
using Components;
using Components.UnityComponents.v1;
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

            lifes = new List<GameObject>(Lifes);
            liveEntities = new List<GameObject>();

            prepareNewPin();
        }

        protected override void UnityOnDisable()
        {
            base.UnityOnDisable();

            UnsubscribeToEvents();
        }

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            Events.OnHit += HandleHit;
            Events.OnScored += OnScored;
            ButtonEvents.OnActionButtonPressed += HandleActionButtonPressed;
        }

        protected override void UnsubscribeToEvents()
        {
            base.UnsubscribeToEvents();
            Events.OnHit -= HandleHit;
            Events.OnScored -= OnScored;
            ButtonEvents.OnActionButtonPressed -= HandleActionButtonPressed;
        }

        private void HandleActionButtonPressed()
        {
            if (!canShoot || GameOver)
            {
                return;
            }

            canShoot = false;
            currentEntity.GetComponent<Rigidbody2D>().simulated = true;

            StartCoroutine(Delay.StartDelay(Cooldown, prepareNewPin, null));
        }

        private void prepareNewPin()
        {
            if (GameOver)
            {
                return;
            }

            canShoot = true;
            var pin = createNewPin();
            currentEntity = pin;
            liveEntities.Add(pin);
        }

        private GameObject createNewPin()
        {
            var newPin = Instantiate(
                PinPrefab, SpawnPoint.position, Quaternion.identity, transform);

            newPin.GetComponent<Rigidbody2D>().simulated = false;

            return newPin;
        }

        private void OnScored(int obj)
        {
            SoundPinned.Play();

            if (++scoreForResetting == MaxPointsBeforeReset)
            {
                scoreForResetting = 0;
                removeAllPins();
            }
        }

        private void removeAllPins()
        {
            foreach (var item in liveEntities)
            {
                Destroy(item);
            }

            liveEntities.Clear();
        }
        
        private void HandleHit()
        {
            scoreForResetting = 0;
            if (lifes.Count == 0 || GameOver)
            {
                SoundDeath.Play();
                Events.EventDeath();
                return;
            }

            CameraAnimation.SetTrigger(Hit);
            removeAllPins();

            var lastEntry = lifes.Last();
            Destroy(lastEntry);
            lifes.Remove(lastEntry);
            SoundHit.Play();

            if (lifes.Count != 0 && !GameOver)
            {
                return;
            }
            
            SoundDeath.Play();
            Events.EventDeath();
        }
    }
}
