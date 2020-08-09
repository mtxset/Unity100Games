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
            this.gameManager = this.GetComponentInParent<MinigameManager>();
            this.SpeedText.text = $"SPEED: {this.ObstacleSpeed}";
        }

        private void Update()
        {
            if (this.gameManager.GameOver)
                return;

            this.time += Time.deltaTime;

            if (this.time >= this.SpawnPeriod)
            {
                this.SpawnPeriod -= DecreaseSpawnPeriodBy;
                var randomIndex = Random.Range(0, this.ObstaclesPrefab.Length);
                var obstacle = Instantiate(this.ObstaclesPrefab[randomIndex], this.transform);

                var rigidBodies = obstacle.GetComponentsInChildren<Rigidbody2D>();

                this.ObstacleSpeed += this.IncreaseObstacleSpeedBy;

                foreach (var item in rigidBodies)
                {
                    item.GetComponent<Rigidbody2D>().AddForce(Vector2.down * this.ObstacleSpeed);
                }

                this.SpeedText.text = $"SPEED: {this.ObstacleSpeed}";
                
                Destroy(obstacle, 5.0f);

                this.time = 0;
            }
        }

    }
}
