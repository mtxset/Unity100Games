using Pool;
using UnityEngine;

namespace UnityInterfaces
{
    public abstract class EntityInfo : MonoBehaviour
    {
        public Transform Target { get; set; }
        public PoolObject PoolObjectReference { get; set; }
    }
}
