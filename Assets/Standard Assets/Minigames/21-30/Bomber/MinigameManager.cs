using System;
using Components.UnityComponents.v2;
using UnityEngine;

namespace Minigames.Bomber
{
    public class MinigameManager : MinigameManager2
    {
        public event Action<GameObject> OnPlatformHit;
        public void PlatformHit(GameObject platform)
        {
            OnPlatformHit?.Invoke(platform);
        }
    }
}