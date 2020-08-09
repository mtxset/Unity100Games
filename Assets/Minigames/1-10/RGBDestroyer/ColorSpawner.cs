using UnityEngine;
using UnityEngine.UI;

namespace Minigames.RGBDestroyer
{
    class ColorSpawner : MonoBehaviour
    {
        public GameObject[] ColorPrefabs = null;
        public Text SpeedText = null;

        public Transform[] SpawnPoints = null;

        public float FallingSpeed = 7.0f;
        public float SpawnRate = 1.0f;
        public float IncreaseRateAfter = 2f;
        public float IncreaseRateBy = 0.1f;

        private float spawnTimer = 0;
        private float difficultyTimer = 0;

        private MinigameManager gameManager;

        private Color[] colors;

        private void Start()
        {
            colors = new Color[]
            {
                Color.red, Color.green, Color.blue
            };

            this.gameManager = this.GetComponentInParent<MinigameManager>();

            this.SpeedText.text = $"SPEED: {this.FallingSpeed}";
        }

        private void Update()
        {
            if (this.gameManager.GameOver)
            {
                return;
            }

            this.spawnTimer += Time.deltaTime;
            this.difficultyTimer += Time.deltaTime;

            if (this.difficultyTimer >= this.IncreaseRateAfter)
            {
                this.FallingSpeed += IncreaseRateBy;
                this.SpeedText.text = $"SPEED: {this.FallingSpeed}";
                this.difficultyTimer = 0;
            }

            if (spawnTimer >= this.SpawnRate)
            {
                this.spawnNewEntity();
                this.spawnTimer = 0;
            }
        }

        private void spawnNewEntity()
        {
            var randomIndex = Random.Range(0, this.ColorPrefabs.Length);
            var newEntity = Instantiate(
                this.ColorPrefabs[randomIndex], this.gameObject.transform);

            var spawnPositionIndex = Random.Range(0, this.SpawnPoints.Length);
            var spawnPosition = this.SpawnPoints[spawnPositionIndex].position;

            newEntity.transform.position = spawnPosition;
            newEntity.GetComponent<Rigidbody2D>().AddForce(
                Vector2.down * this.FallingSpeed * Time.deltaTime);

            var randomColorIndex = Random.Range(0, this.colors.Length);
            newEntity.GetComponent<SpriteRenderer>().color = this.colors[randomColorIndex];

            Destroy(newEntity, 6.0f);
            this.gameManager.SoundSpawn.Play();
        }
    }
}
