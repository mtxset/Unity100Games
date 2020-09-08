using System.Collections.Generic;
using Components;
using UnityEngine;
using UnityEngine.UI;
using UnityInterfaces;

namespace Minigames.PingPong
{
    public class MinigameManager : MonoBehaviour, IMinigameManagerOld
    {
        public GameObject GameOverPage;
        public Text ScoreText;

        public int Score;

        public Events Events;
        public ButtonEvents ButtonEvents;
        public PlayerToManagerCommunicationBus CommunicationBus;

        public List<GameObject> LiveBalls;

        public bool GameOver { get; set; }

        private void Awake()
        {
            Events = new Events();
        }

        private void Start()
        {
            ButtonEvents = GetComponentInParent<ButtonEvents>();
            CommunicationBus = GetComponentInParent<PlayerToManagerCommunicationBus>();

            Events.OnScored += HandleScored;
            Events.OnDeath += HandleDeath;
        }

        private void OnDisable()
        {
            Events.OnScored -= HandleScored;
            Events.OnDeath -= HandleDeath;
        }
        
        private void HandleDeath()
        {
            GameOver = true;
            GameOverPage.SetActive(true);
            CommunicationBus.PlayerDied();
        }

        private void HandleScored()
        {
            setScore(Score + 1);
            CommunicationBus.PlayerScored(1);
        }

        private void setScore(int newScore)
        {
            ScoreText.text = newScore.ToString();
            Score = newScore;
        }

        public int GetScore()
        {
            return Score;
        }
    }
}
