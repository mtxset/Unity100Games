using UnityEngine;
using UnityEngine.UI;
using UnityInterfaces;

namespace Minigames.Barreling
{
    public class MinigameManager : MonoBehaviour, IMinigameManagerOLD
    {
        public AudioSource DeathAudio;
        public AudioSource LandedAudio;
        public AudioSource FallingAudio;

        private Barrel currentBarrel;

        public int Score;

        public Events Events;

        [HideInInspector]
        public ButtonEvents ButtonEvents;

        [HideInInspector]
        public PlayerToManagerCommunicationBus CommunicationBus;

        public bool GameOver { get; set; }

        public GameObject GameOverPage;
        public Text ScoreText;

        private void Awake()
        {
            this.Events = new Events();
        }

        private void Start()
        {
            this.ButtonEvents = GetComponentInParent<ButtonEvents>();
            this.CommunicationBus = GetComponentInParent<PlayerToManagerCommunicationBus>();

            this.subscribeToEvents();
        }

        private void OnDisable()
        {
            this.unsubscribeToEvents();
        }

        public void SetCurrentBarrel(Barrel barrel)
        {
            this.currentBarrel = barrel;
        }

        public int GetScore()
        {
            return this.Score;
        }

        private void subscribeToEvents()
        {
            this.ButtonEvents.OnActionButtonPressed += HandleActionButtonPressed;

            this.Events.OnScored += HandleScored;
            this.Events.OnDeath += HandleDeath;
        }

        private void HandleActionButtonPressed()
        {
            this.currentBarrel.SetBarrelStatic();
        }

        private void unsubscribeToEvents()
        {
            this.ButtonEvents.OnActionButtonPressed -= HandleActionButtonPressed;

            this.Events.OnScored -= HandleScored;
            this.Events.OnDeath -= HandleDeath;
        }

        private void HandleDeath(GameObject obj)
        {
            this.GameOver = true;
            this.GameOverPage.SetActive(true);
            this.DeathAudio.Play();
            this.CommunicationBus.PlayerDied();
        }

        private void HandleScored(int score)
        {
            this.LandedAudio.Play();
            this.setScore(this.Score + score);
            this.CommunicationBus.PlayerScored(score);
        }
        private void setScore(int newScore)
        {
            this.ScoreText.text = newScore.ToString();
            this.Score = newScore;
        }
    }
}
