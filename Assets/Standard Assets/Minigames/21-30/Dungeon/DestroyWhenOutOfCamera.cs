using Components;
using UnityEngine;

namespace Minigames.Dungeon
{
    public class DestroyWhenOutOfCamera : AddMinigameManager2
    {
        private Camera currentCamera;

        private void Start()
        {
            currentCamera = MinigameManager.CurrentCamera;
        }

        private void FixedUpdate()
        {
            var viewPosition = currentCamera.WorldToViewportPoint(transform.position);

            if (viewPosition.x >= 0 &&
                viewPosition.x <= 1 &&
                viewPosition.y >= 0 &&
                viewPosition.y <= 1 &&
                viewPosition.z > 0)
            {
                return;
            }
            
            MinigameManager.Events.EventDodged();
            Destroy(gameObject);
        }
    }
}