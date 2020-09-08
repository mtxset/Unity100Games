using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.InfiniteTunnels
{
    internal class HexagonSpawnner : MonoBehaviour
    {
        public GameObject HexagonPrefab;

        public float SpawnRate = 1.0f;
        public float ShrinkSpeed = 3.0f;
        public int MaxScale = 100;
        public float GradientTraverseSpeed;
        public Text SpeedText;
        public float IncreaseRateAfter = 2f;
        public float IncreaseRateBy = 0.1f;

        private float spawnTimer;
        private float difficultyTimer;
        private List<GameObject> hexagons;
        private List<GameObject> markedForRemove;
        private MinigameManager gameManager;

        private void Start()
        {
            gameManager = GetComponentInParent<MinigameManager>();
            hexagons = new List<GameObject>();
            markedForRemove = new List<GameObject>();
            SpeedText.text = $"SPEED: {ShrinkSpeed}";
        }

        private void Update()
        {
            if (gameManager.GameOver)
            {
                return;
            }

            spawnTimer += Time.deltaTime;
            difficultyTimer += Time.deltaTime;

            checkDifficulty();

            if (spawnTimer >= SpawnRate)
            {
                hexagons.Add(spawnNewHexagon());
                spawnTimer = 0;
            }

            hexagonLifecycle();
        }

        private void checkDifficulty()
        {
            if (!(difficultyTimer >= IncreaseRateAfter))
            {
                return;
            }
            
            ShrinkSpeed += IncreaseRateBy;
            SpawnRate -= IncreaseRateBy / 2;
            SpeedText.text = $"SPEED: {ShrinkSpeed}";
            difficultyTimer = 0;
        }

        private void hexagonLifecycle()
        {
            foreach (var item in hexagons)
            {
                item.transform.localScale -= Vector3.one * (ShrinkSpeed * Time.deltaTime);

                var itemLineRenderer = item.GetComponent<LineRenderer>();
                var traverseGradient = GradientRainbow.Next(itemLineRenderer.colorGradient,
                    0.01f * GradientTraverseSpeed * Time.deltaTime);
                item.GetComponent<LineRenderer>().colorGradient = traverseGradient;

                if (item.transform.localScale.x <= 0.05f)
                {
                    markedForRemove.Add(item);
                }
            }

            foreach (var item in markedForRemove)
            {
                gameManager.Events.EventScored();
                gameManager.SoundScored.Play();
                hexagons.Remove(item);
                Destroy(item);
            }

            markedForRemove.Clear();
        }

        private GameObject spawnNewHexagon()
        {
            var hexagon = Instantiate(HexagonPrefab, gameManager.transform);
            hexagon.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360f)));
            hexagon.transform.localScale = Vector3.one * MaxScale;
            gameManager.SoundSpawnHexagon.Play();
            return hexagon;
        }
    }
}
