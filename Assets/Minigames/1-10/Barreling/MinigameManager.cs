using Components;
using UnityEngine;
using UnityEngine.UI;
using UnityInterfaces;

namespace Minigames.Barreling
{
    public class MinigameManager : MonoBehaviour, IMinigameManagerOld
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
            Events = new Events();
        }

        private void Start()
        {
            ButtonEvents = GetComponentInParent<ButtonEvents>();
            CommunicationBus = GetComponentInParent<PlayerToManagerCommunicationBus>();

            subscribeToEvents();
        }

        private void OnDisable()
        {
            unsubscribeToEvents();
        }

        public void SetCurrentBarrel(Barrel barrel)
        {
            currentBarrel = barrel;
        }

        public int GetScore()
        {
            return Score;
        }

        private void subscribeToEvents()
        {
            ButtonEvents.OnActionButtonPressed += HandleActionButtonPressed;

            Events.OnScored += HandleScored;
            Events.OnDeath += HandleDeath;
        }

        private void HandleActionButtonPressed()
        {
            currentBarrel.SetBarrelStatic();
        }

        private void unsubscribeToEvents()
        {
            ButtonEvents.OnActionButtonPressed -= HandleActionButtonPressed;

            Events.OnScored -= HandleScored;
            Events.OnDeath -= HandleDeath;
        }

        private void HandleDeath(GameObject obj)
        {
            GameOver = true;
            GameOverPage.SetActive(true);
            DeathAudio.Play();
            CommunicationBus.PlayerDied();
        }

        private void HandleScored(int score)
        {
            LandedAudio.Play();
            setScore(Score + score);
            CommunicationBus.PlayerScored(score);
        }
        private void setScore(int newScore)
        {
            ScoreText.text = newScore.ToString();
            Score = newScore;
        }
    }
}
