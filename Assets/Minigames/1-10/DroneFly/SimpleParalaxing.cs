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
                Transform = t;
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
            gameManager = GetComponentInParent<MinigameManager>();
            configure();
            gameManager.DroneEvents.OnGameConfirmed += HandleGameConfirmed;
        }

        public void OnDisable()
        {
            gameManager.DroneEvents.OnGameConfirmed -= HandleGameConfirmed;
        }

        private void HandleGameConfirmed()
        {
            foreach (var poolObject in poolObjectArray)
            {
                poolObject.Dispose();
                poolObject.Transform.position = Vector3.one * 1000;
            }

            if (SpawnImediate)
            {
                spawnObjectImediate();
            }
        }

        private void Update()
        {
            if (gameManager.GameOver)
            {
                return;
            }

            shift();

            spawnTimer += Time.deltaTime;

            if (spawnTimer > SpawnRate)
            {
                spawn();
                spawnTimer = 0;
            }
        }

        private void configure()
        {
            targetApsect = TargetApsectRatio.x / TargetApsectRatio.y;
            poolObjectArray = new PoolObject[PoolSize];

            for (var i = 0; i < poolObjectArray.Length; i++)
            {
                var newGameObject = Instantiate(Prefab);
                var newTransform = newGameObject.transform;
                newTransform.SetParent(transform);
                newTransform.position = Vector3.one * 1000;
                poolObjectArray[i] = new PoolObject(newTransform);
            }

            if (SpawnImediate)
            {
                spawnObjectImediate();
            }
        }

        private void spawn()
        {
            var poolObjectTransform = getPoolObject();
            if (transform == null)
            {
                return; // increase pool size
            }

            var position = Vector3.zero;
            position.x = (DefaultSpawnPosition.x * CurrentCamera.aspect) / targetApsect;
            position.y = UnityEngine.Random.Range(YSpawnRange.Min, YSpawnRange.Max) + gameManager.transform.position.y;
            poolObjectTransform.position = position;
        }

        private void spawnObjectImediate()
        {
            var newTransform = getPoolObject();
            if (newTransform == null)
            {
                return; // increase pool size
            }

            var position = Vector3.zero;
            position.x = (ImidiateSpawnPosition.x * CurrentCamera.aspect) / targetApsect;
            position.y = UnityEngine.Random.Range(YSpawnRange.Min, YSpawnRange.Max) + gameManager.transform.position.y;
            newTransform.position = position;

            spawn();
        }

        private void shift()
        {
            foreach (var poolObject in poolObjectArray)
            {
                poolObject.Transform.position += 
                    -Vector3.right * (ShiftSpeed * Time.deltaTime * currentAcceleration);
                increaseAcceleration();
                checkDisposeObject(poolObject);
            }
        }

        private void increaseAcceleration()
        {
            currentAcceleration += AccelerationRate * Time.deltaTime;
            if (SpeedText != null)
            {
                SpeedText.text = $"SPEED: {currentAcceleration}";
            }
        }

        private void checkDisposeObject(PoolObject poolObject)
        {
            if (poolObject.Transform.position.x < (-DefaultSpawnPosition.x * CurrentCamera.aspect) / targetApsect)
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
