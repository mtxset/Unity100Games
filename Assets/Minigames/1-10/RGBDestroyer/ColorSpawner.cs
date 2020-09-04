using UnityEngine;
using UnityEngine.UI;

namespace Minigames.RGBDestroyer
{
    class ColorSpawner : MonoBehaviour
    {
        public GameObject[] ColorPrefabs;
        public Text SpeedText;

        public Transform[] SpawnPoints;

        public float FallingSpeed = 7.0f;
        public float SpawnRate = 1.0f;
        public float IncreaseRateAfter = 2f;
        public float IncreaseRateBy = 0.1f;

        private float spawnTimer;
        private float difficultyTimer;

        private MinigameManager gameManager;

        private Color[] colors;

        private void Start()
        {
            colors = new[]
            {
                Color.red, Color.green, Color.blue
            };

            gameManager = GetComponentInParent<MinigameManager>();

            SpeedText.text = $"SPEED: {FallingSpeed}";
        }

        private void Update()
        {
            if (gameManager.GameOver)
            {
                return;
            }

            spawnTimer += Time.deltaTime;
            difficultyTimer += Time.deltaTime;

            if (difficultyTimer >= IncreaseRateAfter)
            {
                FallingSpeed += IncreaseRateBy;
                SpeedText.text = $"SPEED: {FallingSpeed}";
                difficultyTimer = 0;
            }

            if (spawnTimer >= SpawnRate)
            {
                spawnNewEntity();
                spawnTimer = 0;
            }
        }

        private void spawnNewEntity()
        {
            var randomIndex = Random.Range(0, ColorPrefabs.Length);
            var newEntity = Instantiate(
                ColorPrefabs[randomIndex], gameObject.transform);

            var spawnPositionIndex = Random.Range(0, SpawnPoints.Length);
            var spawnPosition = SpawnPoints[spawnPositionIndex].position;

            newEntity.transform.position = spawnPosition;
            newEntity.GetComponent<Rigidbody2D>().AddForce(
                Vector2.down * (FallingSpeed * Time.deltaTime));

            var randomColorIndex = Random.Range(0, colors.Length);
            newEntity.GetComponent<SpriteRenderer>().color = colors[randomColorIndex];

            Destroy(newEntity, 6.0f);
            gameManager.SoundSpawn.Play();
        }
    }
}
