using Components;
using UnityEngine;

namespace Minigames.FallForever
{
    public class PlatformSpawner : AddMinigameManager2
    {
        public GameObject[] PlatformPrefabs;
        public Transform MaxXOffset;
        public Vector2 PlatfromFloatSpeedMinMax;

        public Vector2 SpawnAfterMinMax;

        private Camera currentCamera;
        private SimpleEntityLifecycle entityLifecycle;
        private float spawnTimer; 
        private void Start()
        {
            this.currentCamera = MinigameManager.CurrentCamera;
            
            this.entityLifecycle = new SimpleEntityLifecycle(
                this.transform,
                PlatformPrefabs,
                moveEntity,
                spawnMethod,
                null,
                outsideCameraDestroy);
            
            this.entityLifecycle.CreateNewEntity();
        }
        
        private void FixedUpdate()
        {
            this.entityLifecycle.UpdateRoutine();

            this.spawnAfter();
        }

        private void spawnAfter()
        {
            if ((this.spawnTimer += Time.fixedDeltaTime) > SpawnAfterMinMax.x)
            {
                this.entityLifecycle.CreateNewEntity();
                this.spawnTimer = 0;
            }
        }

        private bool outsideCameraDestroy(Vector3 objectPosition)
        {
            return objectPosition.y > this.currentCamera.orthographicSize;
        }
        private Vector2 spawnMethod(Transform platform)
        {
            var randomOffset = Random.Range(
                -this.MaxXOffset.position.x + platform.localScale.x,
                this.MaxXOffset.position.x - platform.localScale.x);
            
            return new Vector2(
                randomOffset,
                -currentCamera.orthographicSize - platform.localScale.y);
        }
        
        private Vector3 moveEntity(Vector3 position)
        {
            return SimpleEntityLifecycle.MoveEntityLinearly(
                position,
                Vector3.up, 
                Vector3.up, 
                this.PlatfromFloatSpeedMinMax.x,
                SimpleEntityLifecycle.UpdateType.FixedUpdate);
        }
    }
}