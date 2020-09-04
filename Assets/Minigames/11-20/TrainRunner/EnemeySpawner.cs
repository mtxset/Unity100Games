using UnityEngine;

namespace Minigames.TrainRunner
{
    public class EnemeySpawner
    {
        private readonly GameObject barrelPrefab;
        private readonly Transform parent;
        private readonly Transform spawnYPosition;
        private readonly float spawnDistance;
        
        public EnemeySpawner(
            GameObject enemyPrefab,
            Transform setParentTo,
            Transform spawnYPosition,
            float spawnDistance)
        {
            barrelPrefab = enemyPrefab;
            parent = setParentTo;
            this.spawnDistance = spawnDistance;
            this.spawnYPosition = spawnYPosition;
        }
        
        public GameObject SpawnBarrel()
        {
            var randomX = Random.Range(-spawnDistance, spawnDistance);
            var barrel = Object.Instantiate(barrelPrefab, parent);
            barrel.transform.position = new Vector3(
                randomX, spawnYPosition.position.y);
            
            return barrel;
        }
    }
}
