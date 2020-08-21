using System;
using UnityEngine;

namespace Components.UnityComponents
{
    public class ParallaxController : MonoBehaviour
    {
        public GameObject ObjectToParallax;
        public Parallaxer.Direction SelectMovementPostion;
        public float ParallaxSpeed;
        public Camera CurrentCamera;
        
        private Parallaxer parallaxer;
        private MinigameManagerDefault gameManager;

        private void Start()
        {
            this.gameManager = this.GetComponentInParent<MinigameManagerDefault>();
            
            this.parallaxer = new Parallaxer(
                this.ObjectToParallax,
                this.SelectMovementPostion,
                this.ParallaxSpeed,
                this.CurrentCamera,
                this.gameManager.transform.position,
                this.transform);
        }

        private void FixedUpdate()
        {
            this.parallaxer.FixedUpdateRoutine();
        }
    }
}