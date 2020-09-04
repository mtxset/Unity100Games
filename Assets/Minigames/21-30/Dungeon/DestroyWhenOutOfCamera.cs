using UnityEngine;

namespace Minigames.Dungeon
{
    public class DestroyWhenOutOfCamera : MonoBehaviour
    {
        private MinigameManager gameManager;
        private Camera currentCamera;

        private void Start()
        {
            gameManager = GetComponentInParent<MinigameManager>();
            currentCamera = gameManager.CurrentCamera;
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
            
            gameManager.Events.EventDodged();
            Destroy(gameObject);
        }
    }
}