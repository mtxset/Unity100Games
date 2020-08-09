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
            this.gameManager = this.GetComponentInParent<MinigameManager>();
            this.hexagons = new List<GameObject>();
            this.markedForRemove = new List<GameObject>();
            this.SpeedText.text = $"SPEED: {this.ShrinkSpeed}";
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
                this.ShrinkSpeed += IncreaseRateBy;
                this.SpawnRate -= IncreaseRateBy / 2;
                this.SpeedText.text = $"SPEED: {this.ShrinkSpeed}";
                this.difficultyTimer = 0;
            }

            if (spawnTimer >= this.SpawnRate)
            {
                this.hexagons.Add(this.spawnNewHexagon());
                this.spawnTimer = 0;
            }

            foreach (var item in hexagons)
            {
                item.transform.localScale -= Vector3.one * this.ShrinkSpeed * Time.deltaTime;

                var itemLineRenderer = item.GetComponent<LineRenderer>();
                var traverseGradient = GradientRainbow.Next(itemLineRenderer.colorGradient, 0.01f * this.GradientTraverseSpeed * Time.deltaTime);
                item.GetComponent<LineRenderer>().colorGradient = traverseGradient;

                if (item.transform.localScale.x <= 0.05f)
                {
                    this.markedForRemove.Add(item);
                }
            }

            foreach (var item in markedForRemove)
            {
                this.gameManager.Events.EventScored(1);
                this.gameManager.SoundScored.Play();
                this.hexagons.Remove(item);
                Destroy(item);
            }

            this.markedForRemove.Clear();
        }

        private GameObject spawnNewHexagon()
        {
            var hexagon = Instantiate(HexagonPrefab, Vector3.zero, Quaternion.identity);
            hexagon.transform.SetParent(this.gameObject.transform);
            hexagon.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360f)));
            hexagon.transform.localScale = Vector3.one * this.MaxScale;
            this.gameManager.SoundSpawnHexagon.Play();
            return hexagon;
        }
    }
}
