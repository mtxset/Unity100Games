using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Minigames.AvoidRocket
{
    class RocketMissile
    {
        public GameObject RocketGameObject;
        public Rigidbody2D Rigidbody2D;
        public float RotationSpeed;
        public float FlySpeed; 
    }
    class RocketSpawnner : MonoBehaviour
    {
        public GameObject RocketPrefab;
        public Transform[] spawnPoints;
        public Transform Target;
        public Text SpeedText;

        public Vector2 RotationSpeedMinMax;
        public Vector2 FlySpeedMinMax;

        private List<RocketMissile> liveEntities;
        private MinigameManager gameManager;

        public float IncreaseRateAfter = 2f;
        public float IncreaseRateBy = 0.1f;
        public float SpawnRocketPeriod = 5.0f;

        [SerializeField]
        private float currentDifficulty = 0.1f;

        private float spawnTimer = 0;
        private float difficultyTimer = 0;

        public void Start()
        {
            this.gameManager = this.GetComponentInParent<MinigameManager>();
            this.liveEntities = new List<RocketMissile>();

            this.spawnTimer = this.IncreaseRateAfter;
            this.SpeedText.text = $"DIFFICULTY: {this.currentDifficulty * 100}";
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

            if ((this.difficultyTimer += Time.deltaTime) >= this.IncreaseRateAfter &&
                this.currentDifficulty < 1.0f)
            {
                this.currentDifficulty += this.IncreaseRateBy;
                this.SpeedText.text = $"DIFFICULTY: {this.currentDifficulty * 100}";
                this.difficultyTimer = 0;
            }

            foreach (var item in liveEntities)
            {
                var direction = (Vector2)this.Target.position - item.Rigidbody2D.position;
                direction.Normalize();

                float rotationAngle = Vector3.Cross(direction, item.RocketGameObject.transform.up).z;

                item.Rigidbody2D.angularVelocity = -rotationAngle * item.RotationSpeed;
                item.Rigidbody2D.velocity = item.RocketGameObject.transform.up * item.FlySpeed;
            }
        }

        private RocketMissile spawnNewMissile()
        {
            var randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

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
