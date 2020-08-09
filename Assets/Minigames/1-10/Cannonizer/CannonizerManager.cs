using DefaultImplementations;
using UnityEngine;
using UnityEngine.UI;
using UnityInterfaces;

namespace Minigames.Cannonizer
{
    public class CannonizerManager : MonoBehaviour, IMinigameManagerOLD
    {
        public GameObject GameOverPage;
        public Text ScoreText;

        public int Score = 0;

        public Events Events;
        public ButtonEvents ButtonEvents;
        public PlayerToManagerCommunicationBus CommunicationBus;

        // used by objects to remove themeselves
        public EnemySpawnner EnemySpawnnerReference;
        public bool GameOver { get; set; } = false;

        private void Awake()
        {
            this.Events = new Events();
        }

        private void Start()
        {
            this.ButtonEvents = GetComponentInParent<ButtonEvents>();
            this.CommunicationBus = GetComponentInParent<PlayerToManagerCommunicationBus>();

            this.Events.OnScored += HandleScored;
            this.Events.OnDeath += HandleDeath;
        }

        private void OnDisable()
        {
            this.Events.OnScored -= HandleScored;
            this.Events.OnDeath -= HandleDeath;
        }

        private void HandleDeath()
        {
            this.GameOver = true;
            this.GameOverPage.SetActive(true);
            this.CommunicationBus.PlayerDied();
        }

        private void HandleScored()
        {
            this.setScore(this.Score + 1);
            this.CommunicationBus.PlayerScored(1);
        }
        private void setScore(int newScore)
        {
            this.ScoreText.text = newScore.ToString();
            this.Score = newScore;
        }

        public int GetScore()
        {
            return this.Score;
        }
    }
}
