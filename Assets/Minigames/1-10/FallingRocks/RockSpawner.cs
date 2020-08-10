using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.FallingRocks
{
    internal class RockSpawner : MonoBehaviour
    {
        public GameObject[] RockPrefabs;
        public Text SpeedText;

        [Range(0, 10)]
        public float FallingSpeed = 7.0f;
        public float SpawnRate = 1.0f;
        public Camera CurrentCamera;
        public float RotationAngle = 30f;
        public Vector2 SpawnSizeMinMax;
        public float IncreaseRateAfter = 2f;
        public float IncreaseRateBy = 0.1f;

        private MinigameManager gameManager;

        private List<GameObject> liveRocks;
        private List<GameObject> deadRocks;
        private Vector2 screenHalfSizeWorldUnits;

        private float spawnTimer;
        private float difficultyTimer;

        private void Start()
        {
            float orthographicSize;
            this.screenHalfSizeWorldUnits = new Vector2(
                this.CurrentCamera.aspect * (orthographicSize = this.CurrentCamera.orthographicSize),
                orthographicSize);

            this.gameManager = this.GetComponentInParent<MinigameManager>();
            this.liveRocks = new List<GameObject>();
            this.deadRocks = new List<GameObject>();

            this.SpeedText.text = $"SPAWN SPEED: {this.SpawnRate}";
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
                this.SpawnRate -= IncreaseRateBy;
                this.SpeedText.text = $"SPAWN SPEED: {this.SpawnRate}";
                this.difficultyTimer = 0;
            }

            if (spawnTimer >= this.SpawnRate)
            {
                this.liveRocks.Add(this.spawnNewRock());
                this.spawnTimer = 0;
            }

            rockLifecycle();
        }

        private void rockLifecycle()
        {
            foreach (var rock in liveRocks)
            {
                rock.transform.Translate(Vector2.down * (this.FallingSpeed * Time.deltaTime));
                if (rock.transform.position.y <=
                    - this.screenHalfSizeWorldUnits.y 
                    - rock.transform.localScale.y 
                    + this.gameManager.transform.position.y)
                {
                    this.deadRocks.Add(rock);
                }
            }

            foreach (var rock in deadRocks)
            {
                this.gameManager.Events.EventScored();
                this.gameManager.SoundScored.Play();
                this.liveRocks.Remove(rock);
                Destroy(rock);
            }

            this.deadRocks.Clear();
        }

        private GameObject spawnNewRock()
        {
            var randomRockIndex = Random.Range(0, this.RockPrefabs.Length);
            var newRock = Instantiate(
                this.RockPrefabs[randomRockIndex], this.gameObject.transform);

            newRock.transform.rotation = Quaternion.Euler(
                0, 0, Random.Range(-this.RotationAngle, this.RotationAngle));

            newRock.transform.localScale = 
                Vector3.one * Random.Range(this.SpawnSizeMinMax.x, this.SpawnSizeMinMax.y);

            var spawnPosition = new Vector2(
                Random.Range(-this.screenHalfSizeWorldUnits.x, this.screenHalfSizeWorldUnits.x),
                this.screenHalfSizeWorldUnits.y 
                    + newRock.transform.localScale.y 
                    + this.gameManager.transform.position.y);
            newRock.transform.position = spawnPosition;
            this.gameManager.SoundSpawnRock.Play();

            return newRock;
        }
    }
}
