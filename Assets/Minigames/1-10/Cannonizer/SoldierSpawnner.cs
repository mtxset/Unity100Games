using Assets.DefaultImplementations;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Minigames.Cannonizer
{
    class SoldierSpawnner: MonoBehaviour
    {
        public Transform[] SpawnPoints = null;
        public Transform[] DirectionsForEnemiesToGo = null;
        public GameObject[] PrefabsToSpawn = null;
        public AudioSource SpeedIncreasedAudio = null;
        public Text SpeedText = null;
        
        public float IncreaseSpawnEachSeconds = 0;
        public float DeacreseSpawnRate = 0;
        public float SpawnFrequencyInSeconds = 0;
        public float MovementSpeed = 0;
        [Tooltip("Will not go below this threshold in seconds")]
        public float LowestSpawnRateSeconds = 0;

        public Transform setParentTo;

        private EnemySpawnner enemySpawnner;
        private CannonizerManager gameManager;
        private float timePassed = 0.0f;
        public void Start()
        {
            this.gameManager = this.GetComponentInParent<CannonizerManager>();
            this.setParentTo = this.gameManager.transform;

            this.enemySpawnner = new EnemySpawnner(
                this.SpawnPoints,
                this.DirectionsForEnemiesToGo,
                this.PrefabsToSpawn,
                this.setParentTo,
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
