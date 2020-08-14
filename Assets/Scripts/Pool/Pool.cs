using System.Collections.Generic;
using UnityEngine;
using UnityInterfaces;

namespace Pool
{
    public struct PoolObject
    {
        public GameObject GameObject { get; }
        public Entity EntityRef { get; }
        public float Lifetime { get; }
        public float DieAtTime { get; set; }

        public PoolObject(Entity entity, GameObject goObj, float time)
        {
            EntityRef = entity;
            GameObject = goObj;
            Lifetime = time;
            DieAtTime = 0;
        }
    }

    public class PoolArray
    {
        public readonly GameObject PrefabToPool;
        public float DefaultLifeTime;
        public readonly Queue<PoolObject> Pool = new Queue<PoolObject>();

        public PoolArray(GameObject prefab)
        {
            PrefabToPool = prefab;
        }
    }

    public class Pool : MonoBehaviour
    {
        public static Pool Current;

        private readonly Dictionary<Entity, PoolArray> pool = new Dictionary<Entity, PoolArray>();

        private readonly LifeTimeController lifeTimeContr = new LifeTimeController();

        private void Awake()
        {
            Current = this;
        }

        private void Update()
        {
            lifeTimeContr.Update();
        }

        public void CreateNewPool(Entity entity, float defaultLifetime, int prewarmAmount = 10)
        {
            pool.Add(entity, new PoolArray(entity.GameObject));
            pool[entity].DefaultLifeTime = defaultLifetime;

            for (var i = 0; i < prewarmAmount; i++)
            {
                AddNewObject(entity);
            }
        }

        private void AddNewObject(Entity entity)
        {
            var gameObj = Instantiate(pool[entity].PrefabToPool);
            var poolObj = new PoolObject(entity, gameObj, pool[entity].DefaultLifeTime);

            gameObj.SetActive(false);
            pool[entity].Pool.Enqueue(poolObj);
        }

        public PoolObject Get(Entity entity)
        {
            while (true)
            {
                if (pool[entity].Pool.Count == 0) AddNewObject(entity);

                if (pool[entity].Pool.Count > 0)
                {
                    var poolObj = pool[entity].Pool.Dequeue();
                    poolObj.GameObject.SetActive(true);
                    poolObj.DieAtTime = poolObj.Lifetime + Time.realtimeSinceStartup;

                    poolObj.GameObject.GetComponent<EntityInfo>().PoolObjectReference = poolObj;

                    lifeTimeContr.AddNewItem(poolObj);
                    return poolObj;
                }
                else
                {
                    AddNewObject(entity);
                }
            }
        }

        public List<PoolObject> GetLiveObjects(EntityType entityType)
        {
            return lifeTimeContr.LiveItems[entityType];
        }

        public void ReturnToPool(PoolObject obj)
        {
            lifeTimeContr.RemoveItem(obj);
            obj.GameObject.SetActive(false);
            pool[obj.EntityRef].Pool.Enqueue(obj);
        }
    }
}