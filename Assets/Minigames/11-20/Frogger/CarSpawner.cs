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
            if ((timer += Time.deltaTime) >= SpawnRate)
            {
                spawnCar();
                timer = 0;
            }
        }

        private void spawnCar()
        {
            var randomSpawnIndex = Random.Range(0, SpawnPoints.Length);
            var randomCarIndex = Random.Range(0, CarPrefabs.Length);

            var car = Instantiate(
                CarPrefabs[randomCarIndex],
                SpawnPoints[randomSpawnIndex].position,
                SpawnPoints[randomSpawnIndex].rotation,
                transform);
            
            Destroy(car, 10.0f);
        }
    }
}