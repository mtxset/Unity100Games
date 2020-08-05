using System;
using UnityEngine;
public enum EntityType
{
    Projectile = 0,
    Enemy = 1,
    Friendly = 2
}

[CreateAssetMenu(menuName = "MTX/Entity")]
public class Entity : ScriptableObject
{
    public GameObject GameObject;
    public EntityType EntityType;
}
