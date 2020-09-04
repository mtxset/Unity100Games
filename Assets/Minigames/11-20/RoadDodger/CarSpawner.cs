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
            DifficultyText.text = $"DIFFICULTY: {CurrentDifficulty * 100}";
            MinigameManager.Events.OnHit += HandleHit;
        }

        private void OnDisable()
        {
            MinigameManager.Events.OnHit -= HandleHit;
        }

        private void HandleHit()
        {
            CurrentDifficulty = 0;
        }

        private void Update()
        {
            gameRoutine();
        }

        private void gameRoutine()
        {
            if (MinigameManager.GameOver)
            {
                return;
            }

            if ((spawnTimer += Time.deltaTime) >= SpawnAfter)
            {
                spawnCar(Random.Range(1, 4));
                MinigameManager.Events.EventScored();
                spawnTimer = 0;
            }

            if ((difficultyTimer += Time.deltaTime) >= IncreaseAfter)
            {
                if (CurrentDifficulty > 1.0f)
                {
                    return;
                }
                
                CurrentDifficulty += IncreaseBy;
                DifficultyText.text = $"DIFFICULTY: {CurrentDifficulty * 100}";
                difficultyTimer = 0;
            }
        }

        private void spawnCar(int amout = 1)
        {
            var occupiedSpawnIndex = new List<int>();
            for (var i = 0; i < amout; i++)
            {
                var randomCarIndex = Random.Range(0, CarPrefabs.Length);

                int randomSpawnIndex;
                do
                {
                    randomSpawnIndex = Random.Range(0, SpawnPoints.Length);
                } while (occupiedSpawnIndex.Contains(randomSpawnIndex));
                
                occupiedSpawnIndex.Add(randomSpawnIndex);

                var vectors = new List<Vector2>
                {
                    CarSpawnOffsetMinMax,
                    CarSpeedMinMax
                };
                
                var difficulty = DifficultyAdjuster.SpreadDifficulty(CurrentDifficulty, vectors);
                var spawnPoint = new Vector2(
                    SpawnPoints[randomSpawnIndex].position.x,
                    SpawnPoints[randomSpawnIndex].position.y + difficulty[0]);
                
                var newCar = Instantiate(
                    CarPrefabs[randomCarIndex],
                    spawnPoint,
                    Quaternion.identity,
                    transform);
            
                newCar.GetComponent<Rigidbody2D>().AddForce(Vector2.down * difficulty[1], ForceMode2D.Impulse);
                Destroy(newCar, 4.0f);
            }
        }
    }
}