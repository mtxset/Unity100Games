using Components;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.DroneFly
{
    public class MinigameManager : MonoBehaviour
    {
        public GameObject StartPage;
        public GameObject GameOverPage;
        public GameObject CountDownPage;
        public Text ScoreText;

        public uint Score;

        public DroneEvents DroneEvents;
        public ButtonEvents ButtonEvents;
        public PlayerToManagerCommunicationBus CommunicationBus;

        enum PageState
        {
            None,
            Start,
            GameOver,
            CountDown
        }

        public bool GameOver { get; private set; } = true;

        private void Awake()
        {
            this.DroneEvents = new DroneEvents();
        }

        private void setPageState(PageState state)
        {
            switch (state)
            {
                case PageState.None:
                    this.StartPage.SetActive(false);
                    this.GameOverPage.SetActive(false);
                    this.CountDownPage.SetActive(false);
                    break;
                case PageState.Start:
                    this.StartPage.SetActive(true);
                    this.GameOverPage.SetActive(false);
                    this.CountDownPage.SetActive(false);
                    break;
                case PageState.CountDown:
                    this.StartPage.SetActive(false);
                    this.GameOverPage.SetActive(false);
                    this.CountDownPage.SetActive(true);
                    break;
                case PageState.GameOver:
                    this.StartPage.SetActive(false);
                    this.GameOverPage.SetActive(true);
                    this.CountDownPage.SetActive(false);
                    break;
            }
        }

        public void ConfirmGameOver()
        {
            this.DroneEvents.EventGameConfirmed();
            this.setScore(0);
            this.setPageState(PageState.Start);
        }

        public void StartGame()
        {
            this.setPageState(PageState.CountDown);
        }

        private void setScore(uint newScore)
        {
            this.ScoreText.text = newScore.ToString();
            this.Score = newScore;
        }

        private void Start()
        {
            this.ButtonEvents = GetComponentInParent<ButtonEvents>();
            this.CommunicationBus = GetComponentInParent<PlayerToManagerCommunicationBus>();

            this.DroneEvents.OnScored += HandleScored;
            this.DroneEvents.OnDroneDeath += HandleDroneDeath;
            this.DroneEvents.OnCountdownFinished += HandleCountdownFinished;
        }


        private void OnDisable()
        {
            this.DroneEvents.OnScored -= HandleScored;
            this.DroneEvents.OnDroneDeath -= HandleDroneDeath;
            this.DroneEvents.OnCountdownFinished -= HandleCountdownFinished;
        }

        private void HandleScored()
        {
            this.setScore(this.Score + 1);
            this.CommunicationBus.PlayerScored(1);
        }

        private void HandleCountdownFinished()
        {
            this.setPageState(PageState.None);
            this.DroneEvents.EventGameStarted();
            this.setScore(0);
            this.GameOver = false;
        }

        private void HandleDroneDeath()
        {
            this.GameOver = true;
            int savedScore = PlayerPrefs.GetInt("Highscore");
            if (Score > savedScore)
            {
                PlayerPrefs.SetInt("Highscore", (int)Score);
            }

            this.setPageState(PageState.GameOver);
            this.CommunicationBus.PlayerDied();
        }
    }
}
