using System;
using Components;
using UnityEngine;

namespace Minigames.TrainRunner
{
    public class SlowMotionParallaxer : MonoBehaviour
    {
        public GameObject ObjectToParallax;
        public Camera CurrentCamera;
        public Animator TrainAnimator;

        public Parallaxer.Direction SelectMovementPostion;
        public float SlowMotionSpeed;
        
        public Vector2 ParallaxSpeedMinMax;
        public Vector2 AnimationSpeedMinMax;

        private Parallaxer parallaxer;
        private MinigameManager gameManager;
        
        private float targetAnimationSpeed;
        private float targetParallaxSpeed;
        
        private void Start()
        {
            this.gameManager = this.GetComponentInParent<MinigameManager>();
            
            this.parallaxer = new Parallaxer(
                this.ObjectToParallax,
                this.SelectMovementPostion,
                this.ParallaxSpeedMinMax.y,
                this.CurrentCamera,
                this.gameManager.transform.position,
                this.transform);

            this.targetParallaxSpeed = this.ParallaxSpeedMinMax.y;
            this.targetAnimationSpeed = this.AnimationSpeedMinMax.y;
            this.subscribeToEvents();
        }

        private void OnDisable()
        {
            this.unsubscribeToEvents();
        }

        private void subscribeToEvents()
        {
            this.gameManager.SlowMotionEvents.OnStartShooting += HandleStartShooting;
            this.gameManager.SlowMotionEvents.OnEndShooting += HandleEndShooting;
        }
        
        private void unsubscribeToEvents()
        {
            this.gameManager.SlowMotionEvents.OnStartShooting -= HandleStartShooting;
            this.gameManager.SlowMotionEvents.OnEndShooting -= HandleEndShooting;
        }

        private void HandleEndShooting()
        {
            // Setting to maximum slow motion value
            this.targetParallaxSpeed = this.ParallaxSpeedMinMax.y;
            this.targetAnimationSpeed = this.AnimationSpeedMinMax.y;
        }

        private void HandleStartShooting()
        {
            // Setting to minimal slow motion value
            this.targetParallaxSpeed = this.ParallaxSpeedMinMax.x;
            this.targetAnimationSpeed = this.AnimationSpeedMinMax.x;
        }

        private void FixedUpdate()
        {
            this.AnimationSpeedMinMax.x = Mathf.Clamp(this.AnimationSpeedMinMax.x, 0.1f, 0.9f);
            this.AnimationSpeedMinMax.y = Mathf.Clamp(this.AnimationSpeedMinMax.y, 0.1f, 1.0f);

            this.TrainAnimator.speed = Mathf.Lerp(
                this.TrainAnimator.speed,
                this.targetAnimationSpeed,
                this.SlowMotionSpeed * Time.deltaTime);
            
            this.parallaxer.ParallaxSpeed = Mathf.Lerp(
                this.parallaxer.ParallaxSpeed,
                this.targetParallaxSpeed,
                this.SlowMotionSpeed * Time.deltaTime);
            
            this.parallaxer.FixedUpdateRoutine();
        }
    }
}