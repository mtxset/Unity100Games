using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityInterfaces;

namespace Pool
{
    public struct PoolObject
    {
        public GameObject gameObject { get; private set; }
        public Entity entityRef { get; private set; }
        public float lifetime { get; private set; }
        public float dieAtTime { get; set; }

        public PoolObject(Entity entity, GameObject goObj, float time)
        {
            entityRef = entity;
            gameObject = goObj;
            lifetime = time;
            dieAtTime = 0;
        }
    }

    public class PoolArray
    {
        public GameObject prefabToPool;
        public float defaultLifeTime = 0f;
        public bool active = false;
        public Queue<PoolObject> pool = new Queue<PoolObject>();

        public PoolArray(GameObject prefab)
        {
            prefabToPool = prefab;
        }
    }

    public class Pool : MonoBehaviour
    {
        public static Pool current;

        private readonly Dictionary<Entity, PoolArray> pool = new Dictionary<Entity, PoolArray>();

        private readonly LifeTimeController lifeTimeContr = new LifeTimeController();

        private void Awake()
        {
            current = this;
        }

        private void Update()
        {
            lifeTimeContr.Update();
        }

        public void CreateNewPool(Entity entity, float defaultLifetime, int prewarmAmount = 10)
        {
            pool.Add(entity, new PoolArray(entity.GameObject));
            pool[entity].active = true;
            pool[entity].defaultLifeTime = defaultLifetime;

            for (int i = 0; i < prewarmAmount; i++)
            {
                AddNewObject(entity);
            }
        }

        private void AddNewObject(Entity entity)
        {
            GameObject gameObj = Instantiate(pool[entity].prefabToPool);
            PoolObject poolObj = new PoolObject(entity, gameObj, pool[entity].defaultLifeTime);

            gameObj.SetActive(false);
            pool[entity].pool.Enqueue(poolObj);
        }

        public PoolObject Get(Entity entity)
        {
            if (pool[entity].pool.Count == 0)
                AddNewObject(entity);

            if (pool[entity].pool.Count > 0)
            {
                var poolObj = pool[entity].pool.Dequeue();
                poolObj.gameObject.SetActive(true);
                poolObj.dieAtTime = poolObj.lifetime + Time.realtimeSinceStartup;

                var id = lifeTimeContr.liveItems[entity.EntityType].Count;
                poolObj.gameObject.GetComponent<EntityInfo>().PoolObjectReference = poolObj;

                lifeTimeContr.AddNewItem(poolObj);
                return poolObj;
            }
            else
            {
                AddNewObject(entity);
                return Get(entity);
            }
        }

        public List<PoolObject> GetLiveObjects(EntityType entityType)
        {
            return lifeTimeContr.liveItems[entityType];
        }

        public void ReturnToPool(PoolObject obj)
        {
            lifeTimeContr.RemoveItem(obj);
            obj.gameObject.SetActive(false);
            pool[obj.entityRef].pool.Enqueue(obj);
        }
    }
}