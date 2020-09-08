using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Components
{
    public partial class SimpleEntityLifecycle
    {
        public delegate Vector3 MoveEntityDelegate(Vector3 position);
        public delegate bool DestroyConditionDelegate(Vector3 position);
        public delegate Vector2 SpawnPositionDelegate(Transform objectTransform);
        
        private readonly GameObject[] entitiesToSpawn;

        private readonly List<GameObject> liveEntities;
        private readonly List<GameObject> deadEntities;
        private readonly Transform setParentTo;

        private readonly SpawnPositionDelegate spawnPositionDelegate;
        private readonly MoveEntityDelegate moveMethod;
        private readonly DestroyConditionDelegate destroyConditionDelegate;
        private readonly Action callbackOnDestroyed;
        
        public SimpleEntityLifecycle(
            Transform setParentTo,
            GameObject[] entitiesToSpawn,
            MoveEntityDelegate moveMethod,
            SpawnPositionDelegate spawnPositionDelegate,
            [CanBeNull] Action callbackOnDestroyed,
            [CanBeNull] DestroyConditionDelegate destroyConditionDelegate)
        {
            this.entitiesToSpawn = entitiesToSpawn;

            liveEntities = new List<GameObject>();
            deadEntities = new List<GameObject>();

            this.moveMethod = moveMethod;
            this.destroyConditionDelegate = destroyConditionDelegate;
            this.setParentTo = setParentTo;
            this.callbackOnDestroyed = callbackOnDestroyed;
            this.spawnPositionDelegate = spawnPositionDelegate;
        }

        public int GetLiveEntityCount()
        {
            return liveEntities.Count;
        }

        public void UpdateRoutine()
        {
            foreach (var entity in liveEntities)
            {
                entity.transform.position = moveMethod(entity.transform.position);

                if (destroyConditionDelegate == null) continue;
                
                if (destroyConditionDelegate(entity.transform.position))
                {
                    deadEntities.Add(entity);
                }
            }

            foreach (var entity in deadEntities)
            {
                liveEntities.Remove(entity);
                Object.Destroy(entity);
                callbackOnDestroyed?.Invoke();
            }
            
            deadEntities.Clear();
        }

        public void CreateNewEntity()
        {
            var randomIndex = Random.Range(0, entitiesToSpawn.Length);

            var newEntity =
                Object.Instantiate(entitiesToSpawn[randomIndex], setParentTo);

            newEntity.transform.position = spawnPositionDelegate(newEntity.transform);
            
            liveEntities.Add(newEntity);
        }
    }
    
}