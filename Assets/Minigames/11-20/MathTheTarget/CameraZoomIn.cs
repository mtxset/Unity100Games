using UnityEngine;

namespace Minigames.MathTheTarget
{
    public class CameraZoomIn : MonoBehaviour
    {
        public Transform ObjectToFollow;
        public float Zoffest = 1f;
        
        private float timer;
        private bool zoomIn;
        private Vector3 cameraStaringPoint;
        private MinigameManager gameManager;

        private void Start()
        {
            gameManager = GetComponentInParent<MinigameManager>();
            
            cameraStaringPoint = transform.position;
            
            gameManager.DartEvents.OnShoot += HandleShoot;
            gameManager.DartEvents.OnDartReset += HandleReset;
        }

        private void OnDisable()
        {
            gameManager.DartEvents.OnShoot -= HandleShoot;
            gameManager.DartEvents.OnDartReset -= HandleReset;
        }

        private void HandleShoot()
        {
            zoomIn = true;
        }

        private void HandleReset()
        {
            zoomIn = false;
        }

        private void cameraZoomIn()
        {
            transform.position =
                ObjectToFollow.position + new Vector3(0, 0, Zoffest - 10);
        }
        private void Update()
        {
            if (gameManager.GameOver)
            {
                return;
            }
            
            if (zoomIn)
            {
                cameraZoomIn();
            }
            else
            {
                transform.position = cameraStaringPoint;
            }
        }
    }
}