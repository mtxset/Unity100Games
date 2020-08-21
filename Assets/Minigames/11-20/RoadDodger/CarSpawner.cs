using System.Collections.Generic;
using Components;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Random = UnityEngine.Random;

namespace Minigames.RoadDodger
{
    public class CarSpawner : AddMinigameManager
    {
        public Text DifficultyText;
        public Transform[] SpawnPoints;
        public GameObject[] CarPrefabs;
        public float SpawnAfter = 2f;
        public float CurrentDifficulty;
        public float IncreaseAfter = 1;
        public float IncreaseBy = 1;
        
        public Vector2 CarSpawnOffsetMinMax;
        public Vector2 CarSpeedMinMax;

        private float difficultyTimer;
        private float spawnTimer;
        private void Start()
        {
            DifficultyText.text = $"DIFFICULTY: {this.CurrentDifficulty * 100}";
            this.MinigameManager.Events.OnHit += HandleHit;
        }

        private void OnDisable()
        {
            this.MinigameManager.Events.OnHit -= HandleHit;
        }

        private void HandleHit()
        {
            this.CurrentDifficulty = 0;
        }

        private void Update()
        {
            gameRoutine();
        }

        private void gameRoutine()
        {
            if (this.MinigameManager.GameOver)
            {
                return;
            }

            if ((spawnTimer += Time.deltaTime) >= this.SpawnAfter)
            {
                this.spawnCar(2);
                this.MinigameManager.Events.EventScored();
                this.spawnTimer = 0;
            }

            if ((this.difficultyTimer += Time.deltaTime) >= this.IncreaseAfter)
            {
                if (this.CurrentDifficulty > 1.0f)
                {
                    return;
                }
                
                this.CurrentDifficulty += this.IncreaseBy;
                DifficultyText.text = $"DIFFICULTY: {this.CurrentDifficulty * 100}";
                this.difficultyTimer = 0;
            }
        }

        private void spawnCar(int amout = 1)
        {
            var occupiedSpawnIndex = new List<int>();
            for (var i = 0; i < amout; i++)
            {
                var randomCarIndex = Random.Range(0, this.CarPrefabs.Length);

                int randomSpawnIndex;
                do
                {
                    randomSpawnIndex = Random.Range(0, this.SpawnPoints.Length);
                } while (occupiedSpawnIndex.Contains(randomSpawnIndex));
                
                occupiedSpawnIndex.Add(randomSpawnIndex);

                var vectors = new List<Vector2>
                {
                    this.CarSpawnOffsetMinMax,
                    this.CarSpeedMinMax
                };
                
                var difficulty = DifficultyAdjuster.SpreadDifficulty(this.CurrentDifficulty, vectors);
                var spawnPoint = new Vector2(
                    this.SpawnPoints[randomSpawnIndex].position.x,
                    this.SpawnPoints[randomSpawnIndex].position.y + difficulty[0]);
                
                var newCar = Instantiate(
                    this.CarPrefabs[randomCarIndex],
                    spawnPoint,
                    Quaternion.identity,
                    this.transform);
            
                newCar.GetComponent<Rigidbody2D>().AddForce(Vector2.down * difficulty[1], ForceMode2D.Impulse);
                Destroy(newCar, 4.0f);
            }
        }
    }
}