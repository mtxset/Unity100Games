using System;
using System.Collections.Generic;
using UnityEngine;
using UnityInterfaces;

namespace Pool
{
    public class LifeTimeController
    {
        public Dictionary<EntityType, List<PoolObject>> liveItems;

        private List<PoolObject> addItemsLst = new List<PoolObject>();
        private List<PoolObject> remItemsLst = new List<PoolObject>();

        public LifeTimeController()
        {
            liveItems = new Dictionary<EntityType, List<PoolObject>>();

            foreach (EntityType item in Enum.GetValues(typeof(EntityType)))
            {
                liveItems.Add(item, new List<PoolObject>());
            }
        }

        public void Update()
        {
            foreach (var item in addItemsLst)
            {
                addNewItem(item);
            }

            addItemsLst.Clear();

            foreach (EntityType type in Enum.GetValues(typeof(EntityType)))
            {
                foreach (var item in liveItems[type])
                {
                    if (!item.gameObject.activeSelf)
                        continue;

                    if (item.dieAtTime < Time.realtimeSinceStartup)
                    {
                        Pool.current.ReturnToPool(item);
                    }
                }
            }

            foreach (var item in remItemsLst)
            {
                removeItem(item);
            }

            remItemsLst.Clear();
        }

        private void addNewItem(PoolObject obj)
        {
            liveItems[obj.entityRef.EntityType].Add(obj);
        }

        private void removeItem(PoolObject obj)
        {
            liveItems[obj.entityRef.EntityType].Remove(obj);
        }

        public void AddNewItem(PoolObject obj)
        {
            addItemsLst.Add(obj);
        }

        public void RemoveItem(PoolObject obj)
        {
            remItemsLst.Add(obj);
        }
    }
}
