using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pool
{
    public class LifeTimeController
    {
        public readonly Dictionary<EntityType, List<PoolObject>> LiveItems;

        private readonly List<PoolObject> addItemsLst = new List<PoolObject>();
        private readonly List<PoolObject> remItemsLst = new List<PoolObject>();

        public LifeTimeController()
        {
            LiveItems = new Dictionary<EntityType, List<PoolObject>>();

            foreach (EntityType item in Enum.GetValues(typeof(EntityType)))
            {
                LiveItems.Add(item, new List<PoolObject>());
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
                foreach (var item in LiveItems[type])
                {
                    if (!item.GameObject.activeSelf)
                        continue;

                    if (item.DieAtTime < Time.realtimeSinceStartup)
                    {
                        Pool.Current.ReturnToPool(item);
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
            LiveItems[obj.EntityRef.EntityType].Add(obj);
        }

        private void removeItem(PoolObject obj)
        {
            LiveItems[obj.EntityRef.EntityType].Remove(obj);
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
