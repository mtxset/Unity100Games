using System.Collections.Generic;
using UnityEngine;

namespace Assets.DefaultImplementations
{
    public class EnemySpawnner
    {
        private Transform[] spawnPoints;
        private Transform[] directionsForEnemiesToGo;
        private GameObject[] prefabsToSpawn;
        private Transform setParentTo;
        private List<GameObject> pool;
        private float timePassed = 0;

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
            this.SpawnFrequencyInSeconds = spawnFrequencyInSeconds;
            this.MovementSpeed = movementSpeed;

            pool = new List<GameObject>();
            this.spawnEnemy();
        }

        public void UpdateRoutine()
        {
            this.timePassed += Time.deltaTime;

            if (timePassed >= this.SpawnFrequencyInSeconds)
            {
                this.spawnEnemy();
                this.timePassed = 0;
            }

            updatePositions();
        }

        public void RemoveEnemy(GameObject gameObjectToRemove)
        {
            this.pool.Remove(gameObjectToRemove);
            Object.Destroy(gameObjectToRemove);
        }

        private void spawnEnemy()
        {
            var randomIndex = Random.Range(0, this.prefabsToSpawn.Length);

            var randomEnemy = Object.Instantiate(
                this.prefabsToSpawn[randomIndex],
                this.setParentTo.transform);

            randomIndex = Random.Range(0, this.spawnPoints.Length);
            var randomPosition = this.spawnPoints[randomIndex];

            randomEnemy.transform.position = randomPosition.position;
            pool.Add(randomEnemy);
        }

        private void updatePositions()
        {
            var randomIndex = Random.Range(0, this.directionsForEnemiesToGo.Length);
            var randomPosition = this.directionsForEnemiesToGo[randomIndex];

            foreach (var item in pool)
            {
                item.transform.position = Vector3.MoveTowards(
                    item.transform.position,
                    randomPosition.transform.position,
                    this.MovementSpeed * Time.deltaTime);
            }
        }
    }
}