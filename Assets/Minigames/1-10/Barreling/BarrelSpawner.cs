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
            barrels = new List<GameObject>();
            gameManager = GetComponentInParent<MinigameManager>();
            subscribetToEvents();
            spawnBarrel();
        }

        private void Update()
        {
            if (gameManager.GameOver)
            {
                return;
            }

            timer += Time.deltaTime;

            if (timer >= MovementSpeedFrequencyInSeconds)
            {
                currentMovementSpeed += IncreaseMovementSpeedBy;
                timer = 0;
            }

            TextSpeed.text = $"SPEED: {currentMovementSpeed}";
        }

        private void spawnBarrel()
        {
            if (gameManager.GameOver)
            {
                return;
            }

            var barrelObject = Instantiate(BarrelPrefab, transform);
            barrels.Add(barrelObject);

            var barrel = barrelObject.GetComponent<Barrel>();
            
            gameManager.SetCurrentBarrel(barrel);
            barrel.AccelerateBarrelMovement(currentMovementSpeed);
        }

        private void subscribetToEvents()
        {
            gameManager.Events.OnLanded += HandleLanded;
            gameManager.Events.OnDeath += HandleDeath;
        }

        private void unsubscribeToEvents()
        {
            gameManager.Events.OnLanded -= HandleLanded;
            gameManager.Events.OnDeath -= HandleDeath;
        }

        private void HandleDeath(GameObject obj)
        {
            foreach (var item in barrels)
            {
                item.GetComponent<Rigidbody2D>().simulated = false;
            }
        }

        private void HandleLanded()
        {
            spawnBarrel();
        }

        private void OnDisable()
        {
            unsubscribeToEvents();
        }
    }
}
