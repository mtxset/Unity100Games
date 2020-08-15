using UnityEngine;

namespace Minigames.TrainRunner
{
    public class EnemeySpawner : MonoBehaviour
    {
        public GameObject BarrelPrefab;
        public float SpawnDistance = 5f;
        public float SpawnAfter = 2f; 
            
        private float timer;
        private void Update()
        {
            if ((this.timer += Time.deltaTime) >= this.SpawnAfter)
            {
                this.spawnBarrel();
                this.timer = 0;
            }
        }

        private GameObject spawnBarrel()
        {
            var randomX = Random.Range(-this.SpawnDistance, this.SpawnDistance);
            var barrel = Instantiate(this.BarrelPrefab, this.transform);
            barrel.transform.position = new Vector3(
                randomX, this.transform.position.y);

            return barrel;
        }
    }
}
