using UnityEngine;

namespace Minigames.Dungeon
{
    public class EnemyType : MonoBehaviour
    {
        public enum EnemyTypeEnum
        {
            Melee,
            Ranged
        }

        public EnemyTypeEnum SelectEnemyType;
    }
}