using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Minigames.AvoidRocket
{
    internal class RocketMissile
    {
        public GameObject RocketGameObject;
        public Rigidbody2D Rigidbody2D;
        public float RotationSpeed;
        public float FlySpeed; 
    }
    internal class RocketSpawnner : MonoBehaviour
    {
        public AudioSource SoundSpawn;
        public GameObject RocketPrefab;
        [SerializeField] public Transform[] SpawnPoints;
        public Transform Target;
        public Text SpeedText;

        public Vector2 RotationSpeedMinMax;
        public Vector2 FlySpeedMinMax;

        private List<RocketMissile> liveEntities;
        private List<RocketMissile> deadEntities;
        private MinigameManager gameManager;

        public float IncreaseRateAfter = 2f;
        public float IncreaseRateBy = 0.1f;
        public float SpawnRocketPeriod = 5.0f;

        private float currentDifficulty = 0.1f;
        private float spawnTimer;
        private float difficultyTimer;

        public void Start()
        {
            gameManager = GetComponentInParent<MinigameManager>();
            liveEntities = new List<RocketMissile>();
            deadEntities = new List<RocketMissile>();

            spawnTimer = IncreaseRateAfter;
            SpeedText.text = $"DIFFICULTY: {currentDifficulty * 100}";

            gameManager.Events.OnHit += HandleHit;

            spawnTimer = SpawnRocketPeriod;
        }

        private void OnDisable()
        {
            gameManager.Events.OnHit -= HandleHit;
        }

        private void HandleHit()
        {
            deadEntities.AddRange(liveEntities);
        }

        private void FixedUpdate()    
        {
            if (gameManager.GameOver)
            {
                return;
            }

            if ((spawnTimer += Time.deltaTime) >= SpawnRocketPeriod)
            {
                liveEntities.Add(spawnNewMissile());
                spawnTimer = 0;
            }

            checkDifficulty();

            rocketLifecycle();
        }

        private void checkDifficulty()
        {
            if ((difficultyTimer += Time.deltaTime) >= IncreaseRateAfter &&
                currentDifficulty < 1.0f)
            {
                currentDifficulty += IncreaseRateBy;
                SpeedText.text = $"DIFFICULTY: {currentDifficulty * 100}";
                difficultyTimer = 0;
            }
        }

        private void rocketLifecycle()
        {
            foreach (var item in liveEntities)
            {
                var direction = (Vector2) Target.position - item.Rigidbody2D.position;
                direction.Normalize();

                var up = item.RocketGameObject.transform.up;
                var rotationAngle = Vector3.Cross(direction, up).z;

                item.Rigidbody2D.angularVelocity = -rotationAngle * item.RotationSpeed;
                item.Rigidbody2D.velocity = up * item.FlySpeed;
            }
            
            foreach (var item in deadEntities)
            {
                liveEntities.Remove(item);
                Destroy(item.RocketGameObject);
            }

            deadEntities.Clear();
        }

        private RocketMissile spawnNewMissile()
        {
            SoundSpawn.Play();
            var randomSpawnPoint = SpawnPoints[Random.Range(0, SpawnPoints.Length)];

            var newRocketMissile = new RocketMissile
            {
                RocketGameObject = Instantiate(
                    RocketPrefab,
                    randomSpawnPoint.position,
                    Quaternion.identity,
                    gameObject.transform),
            };

            newRocketMissile.Rigidbody2D = newRocketMissile.RocketGameObject.GetComponent<Rigidbody2D>();

            var difficulty = Utils.DifficultyAdjuster.SpreadDifficulty(
                currentDifficulty,
                new List<Vector2>
                {
                    RotationSpeedMinMax,
                    FlySpeedMinMax
                });

            newRocketMissile.RotationSpeed = difficulty[0];
            newRocketMissile.FlySpeed = difficulty[1];

            return newRocketMissile;
        }
    }
}
