using UnityEngine;

namespace Minigames.Dungeon
{
    public class DestroyWhenOutOfCamera : MonoBehaviour
    {
        private MinigameManager gameManager;
        private Camera currentCamera;

        private void Start()
        {
            this.gameManager = this.GetComponentInParent<MinigameManager>();
            this.currentCamera = this.gameManager.currentCamera;
        }

        private void FixedUpdate()
        {
            var viewPosition = this.currentCamera.WorldToViewportPoint(this.transform.position);

            if (viewPosition.x >= 0 &&
                viewPosition.x <= 1 &&
                viewPosition.y >= 0 &&
                viewPosition.y <= 1 &&
                viewPosition.z > 0)
            {
                return;
            }
            
            this.gameManager.Events.EventDodged();
            Destroy(this.gameObject);
        }
    }
}