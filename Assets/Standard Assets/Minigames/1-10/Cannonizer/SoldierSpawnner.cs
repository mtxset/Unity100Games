using Components;
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
            gameManager = GetComponentInParent<CannonizerManager>();
            SetParentTo = gameManager.transform;

            enemySpawnner = new EnemySpawnner(
                SpawnPoints,
                DirectionsForEnemiesToGo,
                PrefabsToSpawn,
                SetParentTo,
                SpawnFrequencyInSeconds,
                MovementSpeed);

            gameManager.EnemySpawnnerReference = enemySpawnner;
        }

        private void Update()
        {
            if (gameManager.GameOver) return;

            timePassed += Time.deltaTime;
            if (timePassed >= IncreaseSpawnEachSeconds)
            {
                if (SpawnFrequencyInSeconds >= 0.5f)
                {
                    SpawnFrequencyInSeconds -= DeacreseSpawnRate;
                    SpeedIncreasedAudio.Play();
                    SpeedText.text = $"SPAWN RATE: {SpawnFrequencyInSeconds}";
                    timePassed = 0;
                }
            }
            enemySpawnner.MovementSpeed = MovementSpeed;
            enemySpawnner.SpawnFrequencyInSeconds = SpawnFrequencyInSeconds;
            enemySpawnner.UpdateRoutine();
        }
    }
}
