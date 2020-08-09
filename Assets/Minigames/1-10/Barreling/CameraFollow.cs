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

        private void Start()
        {
            this.targetPosition = this.transform.position;
            this.gameManager = this.GetComponentInParent<MinigameManager>();
            this.subscribetToEvents();
        }

        private void subscribetToEvents()
        {
            this.gameManager.Events.OnLanded += HandleLanded;
        }

        private void unsubscribeToEvents()
        {
            this.gameManager.Events.OnLanded -= HandleLanded;
        }

        private void HandleLanded()
        {
            if (this.gameManager.GameOver)
            {
                return;
            }

            this.moveCameraCount++;
            if (this.moveCameraCount == this.BoxLandedThreshold)
            {
                this.moveCameraCount = 0;
                this.targetPosition = this.transform.position;
                targetPosition.y += this.MoveInYBy;
            }

        }

        private void Update()
        {
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
