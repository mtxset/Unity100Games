using Components;
using Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.ZeFall
{
    public class MinigameManager : MonoBehaviour, IMinigameManager
    {
        public int Score { get; set; }
        public bool GameOver { get; set; }
        public ButtonEvents ButtonEvents { get; set; }
        public PlayerToManagerCommunicationBus CommunicationBus { get; set; }

        public GameObject GameOverPage;
        public Text ScoreText;
        public Events Events;

        public void Awake()
        {
            this.Events = new Events();
        }

        private void Start()
        {
            this.ButtonEvents = GetComponentInParent<ButtonEvents>();
            this.CommunicationBus = GetComponentInParent<PlayerToManagerCommunicationBus>();

            this.SubscribeToEvents();
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
        }

        public void SubscribeToEvents()
        {

            this.Events.OnScored += HandleScored;
            this.Events.OnDeath += HandleDeath;
        }

        public void UnsubscribeToEvents()
        {
            this.Events.OnScored -= HandleScored;
            this.Events.OnDeath -= HandleDeath;
        }
    }
}
