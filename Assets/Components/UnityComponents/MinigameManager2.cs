using Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Components.UnityComponents
{
    public class MinigameManager2 : MonoBehaviour, IMinigameManager
    {
        public GameObject GameOverPage;
        public Text ScoreText;
        public Camera CurrentCamera;

        public int Score { get; set; }
        public bool GameOver { get; set; }
        public ButtonEvents ButtonEvents { get; set; }
        public PlayerToManagerCommunicationBus CommunicationBus { get; set; }
        public SimpleControls Controls { get; private set; }
        public EventsDefault Events { get; private set; }

        protected virtual void UnityStart() { }
        protected virtual void UnityAwake() { }

        protected virtual void UnityOnDisable() { }

        private void Awake()
        {
            this.Controls = new SimpleControls();
            this.Events = new EventsDefault();
            this.UnityAwake();
        }

        private void Start()
        {
            this.ButtonEvents = GetComponentInParent<ButtonEvents>();
            this.CommunicationBus = GetComponentInParent<PlayerToManagerCommunicationBus>();
            this.CurrentCamera = this.GetComponentInChildren<Camera>();

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
            this.ButtonEvents.OnHorizontalPressed += this.Controls.HandleHorizontalStateChange;
            this.ButtonEvents.OnVerticalPressed += this.Controls.HandleVerticalStateChange;
        }

        protected virtual void UnsubscribeToEvents()
        {
            this.Events.OnDeath -= this.HandleDeath;
            this.Events.OnScored -= this.HandleScored;
            
            this.ButtonEvents.OnHorizontalPressed -= this.Controls.HandleHorizontalStateChange;
            this.ButtonEvents.OnVerticalPressed -= this.Controls.HandleVerticalStateChange;
        }
    }
}
