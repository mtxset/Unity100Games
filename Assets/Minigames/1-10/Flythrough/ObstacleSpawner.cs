using UnityEngine;
using UnityEngine.UI;

namespace Minigames.Flythrough
{
    class ObstacleSpawner : MonoBehaviour
    {
        public GameObject[] ObstaclesPrefab;

        public float SpawnPeriod;
        public float ObstacleSpeed;
        public Text SpeedText;
        public float IncreaseObstacleSpeedBy;
        public float DecreaseSpawnPeriodBy;

        private float time;
        private MinigameManager gameManager;

        private void Start()
        {
            gameManager = GetComponentInParent<MinigameManager>();
            SpeedText.text = $"SPEED: {ObstacleSpeed}";
        }

        private void Update()
        {
            if (gameManager.GameOver)
                return;

            time += Time.deltaTime;

            if (time >= SpawnPeriod)
            {
                SpawnPeriod -= DecreaseSpawnPeriodBy;
                var randomIndex = Random.Range(0, ObstaclesPrefab.Length);
                var obstacle = Instantiate(ObstaclesPrefab[randomIndex], transform);

                var rigidBodies = obstacle.GetComponentsInChildren<Rigidbody2D>();

                ObstacleSpeed += IncreaseObstacleSpeedBy;

                foreach (var item in rigidBodies)
                {
                    item.GetComponent<Rigidbody2D>().AddForce(Vector2.down * ObstacleSpeed);
                }

                SpeedText.text = $"SPEED: {ObstacleSpeed}";
                
                Destroy(obstacle, 5.0f);

                time = 0;
            }
        }

    }
}
