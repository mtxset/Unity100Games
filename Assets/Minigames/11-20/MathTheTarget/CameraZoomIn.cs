using System;
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
            this.gameManager = this.GetComponentInParent<MinigameManager>();
            
            this.cameraStaringPoint = this.transform.position;
            
            this.gameManager.DartEvents.OnShoot += HandleShoot;
            this.gameManager.DartEvents.OnDartReset += HandleReset;
        }

        private void OnDisable()
        {
            this.gameManager.DartEvents.OnShoot -= HandleShoot;
            this.gameManager.DartEvents.OnDartReset -= HandleReset;
        }

        private void HandleShoot()
        {
            this.zoomIn = true;
        }

        private void HandleReset()
        {
            this.zoomIn = false;
        }

        private void cameraZoomIn()
        {
            this.transform.position =
                this.ObjectToFollow.position + new Vector3(0, 0, this.Zoffest - 10);
        }
        private void Update()
        {
            if (this.gameManager.GameOver)
            {
                return;
            }
            
            if (this.zoomIn)
            {
                cameraZoomIn();
            }
            else
            {
                this.transform.position = cameraStaringPoint;
            }
        }
    }
}