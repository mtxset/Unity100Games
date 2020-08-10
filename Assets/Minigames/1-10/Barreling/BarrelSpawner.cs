using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.Barreling
{
    public class BarrelSpawner : MonoBehaviour
    {
        public Text TextSpeed;
        public GameObject BarrelPrefab;

        public float MovementSpeedFrequencyInSeconds;
        public float IncreaseMovementSpeedBy;

        private MinigameManager gameManager;
        private float timer;
        private float currentMovementSpeed;
        private List<GameObject> barrels;

        private void Start()
        {
            this.barrels = new List<GameObject>();
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

            var barrelObject = Instantiate(this.BarrelPrefab, this.transform);
            this.barrels.Add(barrelObject);

            var barrel = barrelObject.GetComponent<Barrel>();
            
            this.gameManager.SetCurrentBarrel(barrel);
            barrel.AccelerateBarrelMovement(this.currentMovementSpeed);
        }

        private void subscribetToEvents()
        {
            this.gameManager.Events.OnLanded += HandleLanded;
            this.gameManager.Events.OnDeath += HandleDeath;
        }

        private void unsubscribeToEvents()
        {
            this.gameManager.Events.OnLanded -= HandleLanded;
            this.gameManager.Events.OnDeath -= HandleDeath;
        }

        private void HandleDeath(GameObject obj)
        {
            foreach (var item in this.barrels)
            {
                item.GetComponent<Rigidbody2D>().simulated = false;
            }
        }

        private void HandleLanded()
        {
            this.spawnBarrel();
        }

        private void OnDisable()
        {
            this.unsubscribeToEvents();
        }
    }
}
