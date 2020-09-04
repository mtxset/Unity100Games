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
            currentCamera = GetComponent<Camera>();
            targetPosition = transform.position;
            gameManager = GetComponentInParent<MinigameManager>();
            subscribetToEvents();
        }

        private void subscribetToEvents()
        {
            gameManager.Events.OnLanded += HandleLanded;
            gameManager.Events.OnDeath += HandleDeath;
        }

        private void unsubscribeToEvents()
        {
            gameManager.Events.OnLanded -= HandleLanded;
            gameManager.Events.OnDeath -= HandleDeath;
        }

        private void HandleDeath(GameObject barrel)
        {
            /*
             * If the x and y coordinates values of the result vector are between 0 and 1 and the z value is
             * superior to 0, it means the center of object is seen by camera.
             */
            var maxIterations = 100;
            var viewPosition = currentCamera.WorldToViewportPoint(barrel.transform.position);
            while (!
                (viewPosition.x >= 0 && 
                viewPosition.x <= 1 && 
                viewPosition.y >= 0 && 
                viewPosition.y <= 1 && 
                viewPosition.z > 0))
            {
                viewPosition = currentCamera.WorldToViewportPoint(barrel.transform.position);
                currentCamera.orthographicSize += 1.0f;

                if (--maxIterations < 0)
                {
                    break;
                }
            }
        }

        private void HandleLanded()
        {
            if (gameManager.GameOver)
            {
                return;
            }

            moveCameraCount++;
            if (moveCameraCount != BoxLandedThreshold)
            {
                return;
            }
            
            moveCameraCount = 0;
            targetPosition = transform.position;
            targetPosition.y += MoveInYBy;

        }

        private void Update()
        {
            if (gameManager.GameOver)
                return;
            
            transform.position = Vector3.Lerp(
                transform.position,
                targetPosition,
                SmoothMove * Time.deltaTime);
        }

        private void OnDisable()
        {
            unsubscribeToEvents();
        }
    }
}
