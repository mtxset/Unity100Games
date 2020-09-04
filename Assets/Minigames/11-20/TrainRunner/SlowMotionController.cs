using System.Collections.Generic;
using Components;
using UnityEngine;

namespace Minigames.TrainRunner
{
    public class SlowMotionController : MonoBehaviour
    {
        public GameObject BarrelPrefab;
        public float BarrelSpawnDistance = 5f;
        public Transform SpawnYPosition;
            
        public GameObject ObjectToParallax;
        public Camera CurrentCamera;
        public Animator TrainAnimator;

        public Parallaxer.Direction SelectMovementPostion;
        public float SlowMotionSpeed;
        
        public Vector2 ParallaxSpeedMinMax;
        public Vector2 AnimationSpeedMinMax;

        private Parallaxer parallaxer;
        private MinigameManager gameManager;
        private EnemeySpawner enemeySpawner;
        
        private float targetAnimationSpeed;
        private float targetParallaxSpeed;

        private List<GameObject> enemyList;
        
        private void Start()
        {
            gameManager = GetComponentInParent<MinigameManager>();
            
            parallaxer = new Parallaxer(
                ObjectToParallax,
                SelectMovementPostion,
                ParallaxSpeedMinMax.y,
                CurrentCamera,
                gameManager.transform.position,
                transform);
            
            enemyList = new List<GameObject>();
            
            enemeySpawner = new EnemeySpawner(
                BarrelPrefab, 
                transform, 
                SpawnYPosition,
                BarrelSpawnDistance);

            targetParallaxSpeed = ParallaxSpeedMinMax.y;
            targetAnimationSpeed = AnimationSpeedMinMax.y;
            subscribeToEvents();
            
            enemyList.Add(enemeySpawner.SpawnBarrel());
        }

        private void OnDisable()
        {
            unsubscribeToEvents();
        }

        private void subscribeToEvents()
        {
            gameManager.SlowMotionEvents.OnStartShooting += HandleStartShooting;
            gameManager.SlowMotionEvents.OnEndShooting += HandleEndShooting;
            gameManager.SlowMotionEvents.OnReloaded += HandleReloaded;
        }
        
        private void unsubscribeToEvents()
        {
            gameManager.SlowMotionEvents.OnStartShooting -= HandleStartShooting;
            gameManager.SlowMotionEvents.OnEndShooting -= HandleEndShooting;
            gameManager.SlowMotionEvents.OnReloaded -= HandleReloaded;
        }

        private void HandleReloaded()
        {
            enemyList.Add(enemeySpawner.SpawnBarrel());
        }

        private void HandleEndShooting()
        {
            // Setting to maximum slow motion value
            targetParallaxSpeed = ParallaxSpeedMinMax.y;
            targetAnimationSpeed = AnimationSpeedMinMax.y;
        }

        private void HandleStartShooting()
        {
            // Setting to minimal slow motion value
            targetParallaxSpeed = ParallaxSpeedMinMax.x;
            targetAnimationSpeed = AnimationSpeedMinMax.x;
        }

        private void enemyCycle()
        {
            if (enemyList.Count == 0)
            {
                gameManager.SlowMotionEvents.EventNoEnemies();
                return;
            }
            
            foreach (var item in enemyList)
            {
                item.transform.Translate(
                    Vector2.down * (parallaxer.ParallaxSpeed * Time.deltaTime));
            }
        }

        private void FixedUpdate()
        {
            if (gameManager.GameOver)
            {
                return;
            }
            
            enemyCycle();
            
            AnimationSpeedMinMax.x = Mathf.Clamp(AnimationSpeedMinMax.x, 0.1f, 0.9f);
            AnimationSpeedMinMax.y = Mathf.Clamp(AnimationSpeedMinMax.y, 0.1f, 1.0f);

            TrainAnimator.speed = Mathf.Lerp(
                TrainAnimator.speed,
                targetAnimationSpeed,
                SlowMotionSpeed * Time.deltaTime);
            
            parallaxer.ParallaxSpeed = Mathf.Lerp(
                parallaxer.ParallaxSpeed,
                targetParallaxSpeed,
                SlowMotionSpeed * Time.deltaTime);
            
            parallaxer.FixedUpdateRoutine();
        }
    }
}