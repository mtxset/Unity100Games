using System;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.DroneFly
{
    public class SimpleParalaxing : MonoBehaviour
    {
        private class PoolObject
        {
            public readonly Transform Transform;
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
        public Camera CurrentCamera;
        public GameObject Prefab;
        public int PoolSize;
        public float ShiftSpeed;
        public float SpawnRate;
        public float AccelerationRate;

        public YSpawnRangeStruct YSpawnRange;
        public Vector3 DefaultSpawnPosition;
        public bool SpawnImediate;
        public Vector3 ImidiateSpawnPosition;
        public Vector2 TargetApsectRatio;

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
            foreach (var poolObject in poolObjectArray)
            {
                poolObject.Dispose();
                poolObject.Transform.position = Vector3.one * 1000;
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
            this.targetApsect = this.TargetApsectRatio.x / this.TargetApsectRatio.y;
            poolObjectArray = new PoolObject[this.PoolSize];

            for (var i = 0; i < poolObjectArray.Length; i++)
            {
                var newGameObject = Instantiate(Prefab);
                var newTransform = newGameObject.transform;
                newTransform.SetParent(this.transform);
                newTransform.position = Vector3.one * 1000;
                poolObjectArray[i] = new PoolObject(newTransform);
            }

            if (this.SpawnImediate)
            {
                this.spawnObjectImediate();
            }
        }

        private void spawn()
        {
            var poolObjectTransform = this.getPoolObject();
            if (transform == null)
            {
                return; // increase pool size
            }

            var position = Vector3.zero;
            position.x = (this.DefaultSpawnPosition.x * this.CurrentCamera.aspect) / this.targetApsect;
            position.y = UnityEngine.Random.Range(YSpawnRange.Min, YSpawnRange.Max) + gameManager.transform.position.y;
            poolObjectTransform.position = position;
        }

        private void spawnObjectImediate()
        {
            var newTransform = this.getPoolObject();
            if (newTransform == null)
            {
                return; // increase pool size
            }

            var position = Vector3.zero;
            position.x = (this.ImidiateSpawnPosition.x * this.CurrentCamera.aspect) / this.targetApsect;
            position.y = UnityEngine.Random.Range(YSpawnRange.Min, YSpawnRange.Max) + gameManager.transform.position.y;
            newTransform.position = position;

            this.spawn();
        }

        private void shift()
        {
            foreach (var poolObject in poolObjectArray)
            {
                poolObject.Transform.position += 
                    -Vector3.right * (this.ShiftSpeed * Time.deltaTime * this.currentAcceleration);
                this.increaseAcceleration();
                this.checkDisposeObject(poolObject);
            }
        }

        private void increaseAcceleration()
        {
            this.currentAcceleration += this.AccelerationRate * Time.deltaTime;
            if (this.SpeedText != null)
            {
                this.SpeedText.text = $"SPEED: {this.currentAcceleration}";
            }
        }

        private void checkDisposeObject(PoolObject poolObject)
        {
            if (poolObject.Transform.position.x < (-this.DefaultSpawnPosition.x * this.CurrentCamera.aspect) / this.targetApsect)
            {
                poolObject.Dispose();
                poolObject.Transform.position = Vector3.one * 1000;
            }
        }

        private Transform getPoolObject()
        {
            foreach (var poolObject in poolObjectArray)
            {
                if (poolObject.InUse) continue;
                
                poolObject.Use();
                return poolObject.Transform;
            }

            return null;
        }
    }
}
