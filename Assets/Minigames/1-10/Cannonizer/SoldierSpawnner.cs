using DefaultImplementations;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.Cannonizer
{
    class SoldierSpawnner: MonoBehaviour
    {
        public Transform[] SpawnPoints;
        public Transform[] DirectionsForEnemiesToGo;
        public GameObject[] PrefabsToSpawn;
        public AudioSource SpeedIncreasedAudio;
        public Text SpeedText;
        
        public float IncreaseSpawnEachSeconds;
        public float DeacreseSpawnRate;
        public float SpawnFrequencyInSeconds;
        public float MovementSpeed;
        [Tooltip("Will not go below this threshold in seconds")]
        public float LowestSpawnRateSeconds;

        public Transform SetParentTo;

        private EnemySpawnner enemySpawnner;
        private CannonizerManager gameManager;
        private float timePassed;
        public void Start()
        {
            this.gameManager = this.GetComponentInParent<CannonizerManager>();
            this.SetParentTo = this.gameManager.transform;

            this.enemySpawnner = new EnemySpawnner(
                this.SpawnPoints,
                this.DirectionsForEnemiesToGo,
                this.PrefabsToSpawn,
                this.SetParentTo,
                this.SpawnFrequencyInSeconds,
                this.MovementSpeed);

            this.gameManager.EnemySpawnnerReference = this.enemySpawnner;
        }

        private void Update()
        {
            if (this.gameManager.GameOver) return;

            this.timePassed += Time.deltaTime;
            if (this.timePassed >= this.IncreaseSpawnEachSeconds)
            {
                if (this.SpawnFrequencyInSeconds >= 0.5f)
                {
                    this.SpawnFrequencyInSeconds -= this.DeacreseSpawnRate;
                    this.SpeedIncreasedAudio.Play();
                    this.SpeedText.text = $"SPAWN RATE: {this.SpawnFrequencyInSeconds}";
                    this.timePassed = 0;
                }
            }
            this.enemySpawnner.MovementSpeed = this.MovementSpeed;
            this.enemySpawnner.SpawnFrequencyInSeconds = this.SpawnFrequencyInSeconds;
            this.enemySpawnner.UpdateRoutine();
        }
    }
}
