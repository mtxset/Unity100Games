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
            this.barrelPrefab = enemyPrefab;
            this.parent = setParentTo;
            this.spawnDistance = spawnDistance;
            this.spawnYPosition = spawnYPosition;
        }
        
        public GameObject SpawnBarrel()
        {
            var randomX = Random.Range(-this.spawnDistance, this.spawnDistance);
            var barrel = Object.Instantiate(this.barrelPrefab, this.parent);
            barrel.transform.position = new Vector3(
                randomX, this.spawnYPosition.position.y);
            
            return barrel;
        }
    }
}
