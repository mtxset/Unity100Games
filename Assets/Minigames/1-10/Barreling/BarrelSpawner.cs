using UnityEngine;
using UnityEngine.UI;

namespace Assets.Minigames.Barreling
{
    public class BarrelSpawner : MonoBehaviour
    {
        public Text TextSpeed;
        public GameObject BarrelPrefab;

        public float MovementSpeedFrequencyInSeconds;
        public float IncreaseMovementSpeedBy;

        private MinigameManager gameManager;
        private float timer = 0;
        private float currentMovementSpeed = 0;

        private void Start()
        {
            this.gameManager = this.GetComponentInParent<MinigameManager>();
            this.subscribetToEvents();
            this.spawnBarrel();
        }

        private void Update()
        {
            if (this.gameManager.GameOver)
            {
                return;
            }

            this.timer += Time.deltaTime;

            if (this.timer >= this.MovementSpeedFrequencyInSeconds)
            {
                this.currentMovementSpeed += IncreaseMovementSpeedBy;
                this.timer = 0;
            }

            this.TextSpeed.text = $"SPEED: {this.currentMovementSpeed}";
        }

        private void spawnBarrel()
        {
            if (this.gameManager.GameOver)
            {
                return;
            }

            var barrel = Instantiate(this.BarrelPrefab, this.transform).GetComponent<Barrel>();

            this.gameManager.SetCurrentBarrel(barrel);
            barrel.AccelerateBarrelMovement(this.currentMovementSpeed);
        }

        private void subscribetToEvents()
        {
            this.gameManager.Events.OnLanded += HandleLanded;
        }

        private void unsubscribeToEvents()
        {
            this.gameManager.Events.OnLanded -= HandleLanded;
        }

        private void HandleLanded()
        {
            if (this.gameManager.GameOver)
            {
                return;
            }

            this.spawnBarrel();
        }

        private void OnDisable()
        {
            this.unsubscribeToEvents();
        }
    }
}
