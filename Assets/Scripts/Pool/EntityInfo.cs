using UnityEngine;

namespace Pool
{
    public abstract class EntityInfo : MonoBehaviour
    {
        public Transform Target { get; set; }
        public PoolObject PoolObjectReference { get; set; }
    }
}
