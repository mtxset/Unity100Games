using Components;
using Components.Interfaces;
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
            Events = new Events();
        }

        private void Start()
        {
            ButtonEvents = GetComponentInParent<ButtonEvents>();
            CommunicationBus = GetComponentInParent<PlayerToManagerCommunicationBus>();

            SubscribeToEvents();
        }

        public void HandleDeath()
        {
            GameOver = true;
            GameOverPage.SetActive(true);
            CommunicationBus.PlayerDied();
        }

        public void HandleScored(int points)
        {
            Score += points;
            ScoreText.text = Score.ToString();
            CommunicationBus.PlayerScored(points);
        }

        public void OnDisable()
        {
            UnsubscribeToEvents();
        }

        public void SubscribeToEvents()
        {

            Events.OnScored += HandleScored;
            Events.OnDeath += HandleDeath;
        }

        public void UnsubscribeToEvents()
        {
            Events.OnScored -= HandleScored;
            Events.OnDeath -= HandleDeath;
        }
    }
}
