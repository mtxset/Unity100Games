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
            screenHalfSizeWorldUnits = new Vector2(
                CurrentCamera.aspect * (orthographicSize = CurrentCamera.orthographicSize),
                orthographicSize);

            gameManager = GetComponentInParent<MinigameManager>();
            liveRocks = new List<GameObject>();
            deadRocks = new List<GameObject>();

            SpeedText.text = $"SPAWN SPEED: {SpawnRate}";
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
                SpawnRate -= IncreaseRateBy;
                SpeedText.text = $"SPAWN SPEED: {SpawnRate}";
                difficultyTimer = 0;
            }

            if (spawnTimer >= SpawnRate)
            {
                liveRocks.Add(spawnNewRock());
                spawnTimer = 0;
            }

            rockLifecycle();
        }

        private void rockLifecycle()
        {
            foreach (var rock in liveRocks)
            {
                rock.transform.Translate(Vector2.down * (FallingSpeed * Time.deltaTime));
                if (rock.transform.position.y <=
                    - screenHalfSizeWorldUnits.y 
                    - rock.transform.localScale.y 
                    + gameManager.transform.position.y)
                {
                    deadRocks.Add(rock);
                }
            }

            foreach (var rock in deadRocks)
            {
                gameManager.Events.EventScored();
                gameManager.SoundScored.Play();
                liveRocks.Remove(rock);
                Destroy(rock);
            }

            deadRocks.Clear();
        }

        private GameObject spawnNewRock()
        {
            var randomRockIndex = Random.Range(0, RockPrefabs.Length);
            var newRock = Instantiate(
                RockPrefabs[randomRockIndex], gameObject.transform);

            newRock.transform.rotation = Quaternion.Euler(
                0, 0, Random.Range(-RotationAngle, RotationAngle));

            newRock.transform.localScale = 
                Vector3.one * Random.Range(SpawnSizeMinMax.x, SpawnSizeMinMax.y);

            var spawnPosition = new Vector2(
                Random.Range(-screenHalfSizeWorldUnits.x, screenHalfSizeWorldUnits.x),
                screenHalfSizeWorldUnits.y 
                    + newRock.transform.localScale.y 
                    + gameManager.transform.position.y);
            newRock.transform.position = spawnPosition;
            gameManager.SoundSpawnRock.Play();

            return newRock;
        }
    }
}
