using System;
using Components.UnityComponents;
using UnityEngine;

namespace Minigames.Bomber
{
    public class MinigameManager : MinigameManagerDefault
    {
        public event Action<GameObject> OnPlatformHit;
        public void PlatformHit(GameObject platform)
        {
            OnPlatformHit?.Invoke(platform);
        }
    }
}