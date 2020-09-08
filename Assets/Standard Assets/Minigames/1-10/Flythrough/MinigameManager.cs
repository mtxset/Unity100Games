using Components;
using Components.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.Flythrough
{
    public class MinigameManager : MonoBehaviour, IMinigameManager
    {
        public AudioSource SoundExplosion;
        public AudioSource SoundDeath;
        public AudioSource SoundScore;
        public AudioSource SoundMove;
        public AudioSource SoundFlying;

        public int Score { get; set; } 
        public bool GameOver { get; set; }
        public ButtonEvents ButtonEvents { get; set; }
        public PlayerToManagerCommunicationBus CommunicationBus { get; set; }
        public Events Events { get; set; }
        public GameObject GameOverPage;
        public Text ScoreText;

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

        private void Awake()
        {
            Events = new Events();
        }

        private void Start()
        {
            ButtonEvents = GetComponentInParent<ButtonEvents>();
            CommunicationBus = GetComponentInParent<PlayerToManagerCommunicationBus>();

            SubscribeToEvents();
        }
    }
}
