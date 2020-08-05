using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Minigames.DroneFly
{
    public class SimpleParalaxing : MonoBehaviour
    {
        class PoolObject
        {
            public Transform Transform;
            public bool InUse;
            public PoolObject(Transform t)
            {
                this.Transform = t;
            }

            public void Use()
            {
                InUse = true;
            }

            public void Dispose()
            {
                InUse = false;
            }
        }

        private PoolObject[] poolObjectArray;

        [Serializable]
        public struct YSpawnRangeStruct
        {
            public float Min;
            public float Max;
        }

        private float currentAcceleration = 1.0f;
        private float spawnTimer;
        private float targetApsect;

        public Text SpeedText;
        public Camera currentCamera;
        public GameObject Prefab;
        public int PoolSize;
        public float ShiftSpeed;
        public float SpawnRate;
        public float accelerationRate;

        public YSpawnRangeStruct YSpawnRange;
        public Vector3 DefaultSpawnPosition;
        public bool SpawnImediate;
        public Vector3 ImidiateSpawnPosition;
        public Vector2 targetApsectRatio;

        private MinigameManager gameManager;

        private void Start()
        {
            this.gameManager = GetComponentInParent<MinigameManager>();
            this.configure();
            this.gameManager.DroneEvents.OnGameConfirmed += HandleGameConfirmed;
        }

        public void OnDisable()
        {
            this.gameManager.DroneEvents.OnGameConfirmed -= HandleGameConfirmed;
        }

        private void HandleGameConfirmed()
        {
            for (int i = 0; i < poolObjectArray.Length; i++)
            {
                poolObjectArray[i].Dispose();
                poolObjectArray[i].Transform.position = Vector3.one * 1000;
            }

            if (this.SpawnImediate)
            {
                this.spawnObjectImediate();
            }
        }

        private void Update()
        {
            if (this.gameManager.GameOver)
            {
                return;
            }

            this.shift();

            this.spawnTimer += Time.deltaTime;

            if (this.spawnTimer > this.SpawnRate)
            {
                this.spawn();
                this.spawnTimer = 0;
            }
        }

        private void configure()
        {
            this.targetApsect = this.targetApsectRatio.x / this.targetApsectRatio.y;
            poolObjectArray = new PoolObject[this.PoolSize];

            for (int i = 0; i < poolObjectArray.Length; i++)
            {
                GameObject gameObject = Instantiate(Prefab);
                Transform transform = gameObject.transform;
                transform.SetParent(this.transform);
                transform.position = Vector3.one * 1000;
                poolObjectArray[i] = new PoolObject(transform);
            }

            if (this.SpawnImediate)
            {
                this.spawnObjectImediate();
            }
        }

        private void spawn()
        {
            Transform transform = this.getPoolObject();
            if (transform == null)
            {
                return; // increase pool size
            }

            var position = Vector3.zero;
            position.x = (this.DefaultSpawnPosition.x * this.currentCamera.aspect) / this.targetApsect;
            position.y = UnityEngine.Random.Range(YSpawnRange.Min, YSpawnRange.Max) + gameManager.transform.position.y;
            transform.position = position;
        }

        private void spawnObjectImediate()
        {
            Transform transform = this.getPoolObject();
            if (transform == null)
            {
                return; // increase pool size
            }

            var position = Vector3.zero;
            position.x = (this.ImidiateSpawnPosition.x * this.currentCamera.aspect) / this.targetApsect;
            position.y = UnityEngine.Random.Range(YSpawnRange.Min, YSpawnRange.Max) + gameManager.transform.position.y;
            transform.position = position;

            this.spawn();
        }

        private void shift()
        {
            for (int i = 0; i < poolObjectArray.Length; i++)
            {
                poolObjectArray[i].Transform.position += -Vector3.right * this.ShiftSpeed * Time.deltaTime * this.currentAcceleration;
                this.increaseAcceleration();
                this.checkDisposeObject(poolObjectArray[i]);
            }
        }

        private void increaseAcceleration()
        {
            this.currentAcceleration += this.accelerationRate * Time.deltaTime;
            if (this.SpeedText != null)
            {
                this.SpeedText.text = $"SPEED: {this.currentAcceleration}";
            }
        }

        private void checkDisposeObject(PoolObject poolObject)
        {
            if (poolObject.Transform.position.x < (-this.DefaultSpawnPosition.x * this.currentCamera.aspect) / this.targetApsect)
            {
                poolObject.Dispose();
                poolObject.Transform.position = Vector3.one * 1000;
            }
        }

        private Transform getPoolObject()
        {
            for (int i = 0; i < poolObjectArray.Length; i++)
            {
                if (!poolObjectArray[i].InUse)
                {
                    poolObjectArray[i].Use();
                    return poolObjectArray[i].Transform;
                }
            }

            return null;
        }
    }
}
