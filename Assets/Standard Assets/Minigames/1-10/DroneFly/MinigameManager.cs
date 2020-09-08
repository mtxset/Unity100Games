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
            DroneEvents = new DroneEvents();
        }

        private void setPageState(PageState state)
        {
            switch (state)
            {
                case PageState.None:
                    StartPage.SetActive(false);
                    GameOverPage.SetActive(false);
                    CountDownPage.SetActive(false);
                    break;
                case PageState.Start:
                    StartPage.SetActive(true);
                    GameOverPage.SetActive(false);
                    CountDownPage.SetActive(false);
                    break;
                case PageState.CountDown:
                    StartPage.SetActive(false);
                    GameOverPage.SetActive(false);
                    CountDownPage.SetActive(true);
                    break;
                case PageState.GameOver:
                    StartPage.SetActive(false);
                    GameOverPage.SetActive(true);
                    CountDownPage.SetActive(false);
                    break;
            }
        }

        public void ConfirmGameOver()
        {
            DroneEvents.EventGameConfirmed();
            setScore(0);
            setPageState(PageState.Start);
        }

        public void StartGame()
        {
            setPageState(PageState.CountDown);
        }

        private void setScore(uint newScore)
        {
            ScoreText.text = newScore.ToString();
            Score = newScore;
        }

        private void Start()
        {
            ButtonEvents = GetComponentInParent<ButtonEvents>();
            CommunicationBus = GetComponentInParent<PlayerToManagerCommunicationBus>();

            DroneEvents.OnScored += HandleScored;
            DroneEvents.OnDroneDeath += HandleDroneDeath;
            DroneEvents.OnCountdownFinished += HandleCountdownFinished;
        }


        private void OnDisable()
        {
            DroneEvents.OnScored -= HandleScored;
            DroneEvents.OnDroneDeath -= HandleDroneDeath;
            DroneEvents.OnCountdownFinished -= HandleCountdownFinished;
        }

        private void HandleScored()
        {
            setScore(Score + 1);
            CommunicationBus.PlayerScored(1);
        }

        private void HandleCountdownFinished()
        {
            setPageState(PageState.None);
            DroneEvents.EventGameStarted();
            setScore(0);
            GameOver = false;
        }

        private void HandleDroneDeath()
        {
            GameOver = true;
            int savedScore = PlayerPrefs.GetInt("Highscore");
            if (Score > savedScore)
            {
                PlayerPrefs.SetInt("Highscore", (int)Score);
            }

            setPageState(PageState.GameOver);
            CommunicationBus.PlayerDied();
        }
    }
}
