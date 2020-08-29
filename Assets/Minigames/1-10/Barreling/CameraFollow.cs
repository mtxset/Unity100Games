using UnityEngine;

namespace Minigames.Barreling
{
    public class CameraFollow : MonoBehaviour
    {
        [Tooltip("Camera will move after x many boxes have landed")]
        public int BoxLandedThreshold = 2;
        public float SmoothMove = 1f;
        public float MoveInYBy = 2f;

        private MinigameManager gameManager;

        private int moveCameraCount;
        private Vector3 targetPosition;
        private Camera currentCamera;

        private void Start()
        {
            this.currentCamera = this.GetComponent<Camera>();
            this.targetPosition = this.transform.position;
            this.gameManager = this.GetComponentInParent<MinigameManager>();
            this.subscribetToEvents();
        }

        private void subscribetToEvents()
        {
            this.gameManager.Events.OnLanded += HandleLanded;
            this.gameManager.Events.OnDeath += HandleDeath;
        }

        private void unsubscribeToEvents()
        {
            this.gameManager.Events.OnLanded -= HandleLanded;
            this.gameManager.Events.OnDeath -= HandleDeath;
        }

        private void HandleDeath(GameObject barrel)
        {
            /*
             * If the x and y coordinates values of the result vector are between 0 and 1 and the z value is
             * superior to 0, it means the center of object is seen by camera.
             */
            var maxIterations = 100;
            var viewPosition = this.currentCamera.WorldToViewportPoint(barrel.transform.position);
            while (!
                (viewPosition.x >= 0 && 
                viewPosition.x <= 1 && 
                viewPosition.y >= 0 && 
                viewPosition.y <= 1 && 
                viewPosition.z > 0))
            {
                viewPosition = this.currentCamera.WorldToViewportPoint(barrel.transform.position);
                this.currentCamera.orthographicSize += 1.0f;

                if (--maxIterations < 0)
                {
                    break;
                }
            }
        }

        private void HandleLanded()
        {
            if (this.gameManager.GameOver)
            {
                return;
            }

            this.moveCameraCount++;
            if (this.moveCameraCount != this.BoxLandedThreshold)
            {
                return;
            }
            
            this.moveCameraCount = 0;
            this.targetPosition = this.transform.position;
            targetPosition.y += this.MoveInYBy;

        }

        private void Update()
        {
            if (this.gameManager.GameOver)
                return;
            
            this.transform.position = Vector3.Lerp(
                this.transform.position,
                this.targetPosition,
                this.SmoothMove * Time.deltaTime);
        }

        private void OnDisable()
        {
            this.unsubscribeToEvents();
        }
    }
}
