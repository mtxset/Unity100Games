using System.Collections.Generic;
using UnityEngine;

namespace Components
{
    public class EnemySpawnner
    {
        private readonly Transform[] spawnPoints;
        private readonly Transform[] directionsForEnemiesToGo;
        private readonly GameObject[] prefabsToSpawn;
        private readonly Transform setParentTo;
        private readonly List<GameObject> pool;
        private float timePassed;

        public float SpawnFrequencyInSeconds { get; set; }
        public float MovementSpeed { get; set; }

        public EnemySpawnner(
            Transform[] spawnPoints,
            Transform[] directionsForEnemiesToGo,
            GameObject[] prefabsToSpawn,
            Transform setParentTo,
            float spawnFrequencyInSeconds,
            float movementSpeed)
        {
            this.spawnPoints = spawnPoints;
            this.directionsForEnemiesToGo = directionsForEnemiesToGo;
            this.prefabsToSpawn = prefabsToSpawn;
            this.setParentTo = setParentTo;
            SpawnFrequencyInSeconds = spawnFrequencyInSeconds;
            MovementSpeed = movementSpeed;

            pool = new List<GameObject>();
            spawnEnemy();
        }

        public void UpdateRoutine()
        {
            timePassed += Time.deltaTime;

            if (timePassed >= SpawnFrequencyInSeconds)
            {
                spawnEnemy();
                timePassed = 0;
            }

            updatePositions();
        }

        public void RemoveEnemy(GameObject gameObjectToRemove)
        {
            pool.Remove(gameObjectToRemove);
            Object.Destroy(gameObjectToRemove);
        }

        private void spawnEnemy()
        {
            var randomIndex = Random.Range(0, prefabsToSpawn.Length);

            var randomEnemy = Object.Instantiate(
                prefabsToSpawn[randomIndex],
                setParentTo.transform);

            randomIndex = Random.Range(0, spawnPoints.Length);
            var randomPosition = spawnPoints[randomIndex];

            randomEnemy.transform.position = randomPosition.position;
            pool.Add(randomEnemy);
        }

        private void updatePositions()
        {
            var randomIndex = Random.Range(0, directionsForEnemiesToGo.Length);
            var randomPosition = directionsForEnemiesToGo[randomIndex];

            foreach (var item in pool)
            {
                item.transform.position = Vector3.MoveTowards(
                    item.transform.position,
                    randomPosition.transform.position,
                    MovementSpeed * Time.deltaTime);
            }
        }
    }
}