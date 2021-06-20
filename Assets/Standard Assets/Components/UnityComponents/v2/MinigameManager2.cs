using Components.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Components.UnityComponents.v2
{
    public class MinigameManager2 : MonoBehaviour, IMinigameManager
    {
        public GameObject GameOverPage;
        public Text ScoreText;
        public Camera CurrentCamera;

        public float DiffCurrent;
        public float DiffIncreaseBy;
        public float DiffIncreaseRate;
        public float DiffTimer;

        public int Score { get; set; }
        public bool GameOver { get; set; }
        public ButtonEvents ButtonEvents { get; set; }
        public PlayerToManagerCommunicationBus CommunicationBus { get; set; }
        public SimpleControls Controls { get; private set; }
        public EventsDefault Events { get; private set; }
				public AudioSource AudioSource;

        protected virtual void UnityStart() { }
        protected virtual void UnityAwake() { }
        protected virtual void UnityUpdate() { }

        protected virtual void UnityOnDisable() { }

        private void Awake()
        {
						AudioSource = gameObject.AddComponent<AudioSource>() as AudioSource;
            Controls = new SimpleControls();
            Events = new EventsDefault();
            UnityAwake();
        }

        private void Start()
        {
            ButtonEvents = GetComponentInParent<ButtonEvents>();
            CommunicationBus = GetComponentInParent<PlayerToManagerCommunicationBus>();
            CurrentCamera = GetComponentInChildren<Camera>();

            SubscribeToEvents();
            UnityStart();
        }

        private void Update() {

            if ((DiffTimer += Time.deltaTime) > DiffIncreaseRate) {
                DiffTimer = 0;

                DiffCurrent += DiffIncreaseBy;

                if (DiffCurrent > 1f)
                    DiffCurrent = 1;
            }

            UnityUpdate();
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
            UnityOnDisable();
        }

        protected virtual void SubscribeToEvents()
        {
            Events.OnDeath += HandleDeath;
            Events.OnScored += HandleScored;

            ButtonEvents.OnHorizontalPressed += Controls.HandleHorizontalStateChange;
            ButtonEvents.OnVerticalPressed += Controls.HandleVerticalStateChange;
        }

        protected virtual void UnsubscribeToEvents()
        {
            Events.OnDeath -= HandleDeath;
            Events.OnScored -= HandleScored;

            ButtonEvents.OnHorizontalPressed -= Controls.HandleHorizontalStateChange;
            ButtonEvents.OnVerticalPressed -= Controls.HandleVerticalStateChange;
        }
    }
}
