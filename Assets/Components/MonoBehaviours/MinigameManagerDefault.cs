﻿using Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Components.MonoBehaviours
{
    public class MinigameManagerDefault : MonoBehaviour, IMinigameManager
    {
        public GameObject GameOverPage;
        public Text ScoreText;

        public int Score { get; set; }
        public bool GameOver { get; set; }
        public ButtonEvents ButtonEvents { get; set; }
        public PlayerToManagerCommunicationBus CommunicationBus { get; set; }

        public EventsDefault Events { get; private set; }

        protected virtual void UnityStart() { }
        protected virtual void UnityAwake() { }

        protected virtual void UnityOnDisable() { }

        private void Awake()
        {
            this.Events = new EventsDefault();
            this.UnityAwake();
        }

        private void Start()
        {
            this.ButtonEvents = GetComponentInParent<ButtonEvents>();
            this.CommunicationBus = GetComponentInParent<PlayerToManagerCommunicationBus>();

            this.SubscribeToEvents();
            this.UnityStart();
        }

        public void HandleDeath()
        {
            this.GameOver = true;
            this.GameOverPage.SetActive(true);
            this.CommunicationBus.PlayerDied();
        }

        public void HandleScored(int points)
        {
            this.Score += points;
            this.ScoreText.text = this.Score.ToString();
            this.CommunicationBus.PlayerScored(points);
        }

        public void OnDisable()
        {
            this.UnsubscribeToEvents();
            this.UnityOnDisable();
        }

        protected virtual void SubscribeToEvents()
        {
            this.Events.OnDeath += this.HandleDeath;
            this.Events.OnScored += this.HandleScored;
        }

        protected virtual void UnsubscribeToEvents()
        {
            this.Events.OnDeath -= this.HandleDeath;
            this.Events.OnScored -= this.HandleScored;
        }
    }
}