using System;
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
            this.gameManager = this.GetComponentInParent<MinigameManager>();
            this.liveEntities = new List<RocketMissile>();
            this.deadEntities = new List<RocketMissile>();

            this.spawnTimer = this.IncreaseRateAfter;
            this.SpeedText.text = $"DIFFICULTY: {this.currentDifficulty * 100}";

            this.gameManager.Events.OnHit += HandleHit;
        }

        private void OnDisable()
        {
            this.gameManager.Events.OnHit -= HandleHit;
        }

        private void HandleHit()
        {
            deadEntities.AddRange(liveEntities);
        }

        private void FixedUpdate()    
        {
            if (this.gameManager.GameOver)
            {
                return;
            }

            if ((this.spawnTimer += Time.deltaTime) >= this.SpawnRocketPeriod)
            {
                this.liveEntities.Add(this.spawnNewMissile());
                this.spawnTimer = 0;
            }

            this.checkDifficulty();

            this.rocketLifecycle();
        }

        private void checkDifficulty()
        {
            if ((this.difficultyTimer += Time.deltaTime) >= this.IncreaseRateAfter &&
                this.currentDifficulty < 1.0f)
            {
                this.currentDifficulty += this.IncreaseRateBy;
                this.SpeedText.text = $"DIFFICULTY: {this.currentDifficulty * 100}";
                this.difficultyTimer = 0;
            }
        }

        private void rocketLifecycle()
        {
            foreach (var item in liveEntities)
            {
                var direction = (Vector2) this.Target.position - item.Rigidbody2D.position;
                direction.Normalize();

                var up = item.RocketGameObject.transform.up;
                var rotationAngle = Vector3.Cross(direction, up).z;

                item.Rigidbody2D.angularVelocity = -rotationAngle * item.RotationSpeed;
                item.Rigidbody2D.velocity = up * item.FlySpeed;
            }
            
            foreach (var item in deadEntities)
            {
                this.liveEntities.Remove(item);
                Destroy(item.RocketGameObject);
            }

            this.deadEntities.Clear();
        }

        private RocketMissile spawnNewMissile()
        {
            var randomSpawnPoint = SpawnPoints[Random.Range(0, SpawnPoints.Length)];

            var newRocketMissile = new RocketMissile
            {
                RocketGameObject = Instantiate(
                    this.RocketPrefab,
                    randomSpawnPoint.position,
                    Quaternion.identity,
                    this.gameObject.transform),
            };

            newRocketMissile.Rigidbody2D = newRocketMissile.RocketGameObject.GetComponent<Rigidbody2D>();

            var difficulty = Utils.DifficutlyAdjuster.SpreadDifficulty(
                this.currentDifficulty,
                new List<Vector2>
                {
                    this.RotationSpeedMinMax,
                    this.FlySpeedMinMax
                });

            newRocketMissile.RotationSpeed = difficulty[0];
            newRocketMissile.FlySpeed = difficulty[1];

            return newRocketMissile;
        }
    }
}
