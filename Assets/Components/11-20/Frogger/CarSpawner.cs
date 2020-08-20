using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Minigames.Frogger
{
    public class CarSpawner : MonoBehaviour
    {
        public float SpawnRate = 0.3f;
        public GameObject[] CarPrefabs;
        public Transform[] SpawnPoints;
        
        private float timer;
        private void Update()
        {
            if ((this.timer += Time.deltaTime) >= this.SpawnRate)
            {
                this.spawnCar();
                this.timer = 0;
            }
        }

        private GameObject spawnCar()
        {
            var randomSpawnIndex = Random.Range(0, this.SpawnPoints.Length);
            var randomCarIndex = Random.Range(0, this.CarPrefabs.Length);

            var car = Instantiate(
                this.CarPrefabs[randomCarIndex],
                this.SpawnPoints[randomSpawnIndex].position,
                this.SpawnPoints[randomSpawnIndex].rotation,
                this.transform);
            
            Destroy(car, 10.0f);
            
            return car;    
        }
    }
}