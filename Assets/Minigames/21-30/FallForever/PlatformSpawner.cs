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
        private float globalPositionOffsetY;
        private void Start()
        {
            globalPositionOffsetY = MinigameManager.transform.position.y;
            currentCamera = MinigameManager.CurrentCamera;
            
            entityLifecycle = new SimpleEntityLifecycle(
                transform,
                PlatformPrefabs,
                moveEntity,
                spawnMethod,
                null,
                outsideCameraDestroy);
            
            entityLifecycle.CreateNewEntity();
        }
        
        private void FixedUpdate()
        {
            entityLifecycle.UpdateRoutine();

            spawnAfter();
        }

        private void spawnAfter()
        {
            if ((spawnTimer += Time.fixedDeltaTime) > SpawnAfterMinMax.x)
            {
                entityLifecycle.CreateNewEntity();
                spawnTimer = 0;
            }
        }

        private bool outsideCameraDestroy(Vector3 objectPosition)
        {
            return objectPosition.y > currentCamera.orthographicSize + globalPositionOffsetY;
        }
        private Vector2 spawnMethod(Transform platform)
        {
            var randomOffset = Random.Range(
                -MaxXOffset.position.x + platform.localScale.x,
                MaxXOffset.position.x - platform.localScale.x);
            
            return new Vector2(
                randomOffset,
                -currentCamera.orthographicSize 
                - platform.localScale.y 
                + globalPositionOffsetY);
        }
        
        private Vector3 moveEntity(Vector3 position)
        {
            return SimpleEntityLifecycle.MoveEntityLinearly(
                position,
                Vector3.up, 
                Vector3.up, 
                PlatfromFloatSpeedMinMax.x,
                SimpleEntityLifecycle.UpdateType.FixedUpdate);
        }
    }
}