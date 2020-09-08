using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Minigames.FallingBlocks
{
    public class BlockSpawner : MonoBehaviour
    {
        public Text SpeedText;
        public Transform[] SpawnPoints;
        public GameObject BlockPrefab;
        public Camera CurrentCamera;
        public float SpawnRate;
        public float IncreaseRateAfter = 2f;
        public float IncreaseRateBy = 0.1f;
        public float Gravity = 2f;
            
        private MinigameManager gameManager;
        private Vector2 screenHalfSizeWorldUnits;
        private List<GameObject> liveEntities;
        private List<GameObject> deadEntities;
        private float spawnTimer;
        private float difficultyTimer;
        private float initialGravity;

        private void Start()
        {
            float orthographicSize;
            screenHalfSizeWorldUnits = new Vector2(
                CurrentCamera.aspect * (orthographicSize = CurrentCamera.orthographicSize),
                orthographicSize);
            
            liveEntities = new List<GameObject>();
            deadEntities = new List<GameObject>();
            
            gameManager = GetComponentInParent<MinigameManager>();
            spawnTimer = SpawnRate;

            initialGravity = Gravity;
            
            SpeedText.text = $"GRAVITY: {Gravity}";

            gameManager.Events.OnHit += HandleHit;
        }

        private void OnDisable()
        {
            gameManager.Events.OnHit -= HandleHit;
        }

        private void HandleHit()
        {
            deadEntities.AddRange(liveEntities);
            Gravity = initialGravity;
        }

        private void Update()
        {    
            if (gameManager.GameOver)
            {
                return;
            }
            
            if ((spawnTimer += Time.deltaTime) >= SpawnRate)
            {
                spawnNewBlocks();
                gameManager.Events.EventScored();
                spawnTimer = 0;
            }
            
            if ((difficultyTimer += Time.deltaTime) >= IncreaseRateAfter)
            {
                Gravity += IncreaseRateBy;
                SpeedText.text = $"GRAVITY: {Gravity}";
                difficultyTimer = 0;
            }

            blockLifecycle();
        }

        private void blockLifecycle()
        {
            foreach (var entity in liveEntities)
            {
                var belowY = -screenHalfSizeWorldUnits.y
                             - entity.transform.localScale.y
                             + gameManager.transform.position.y;
                
                if (entity.transform.position.y < belowY)
                {
                    deadEntities.Add(entity);
                }
            }
            
            foreach (var item in deadEntities)
            {
                liveEntities.Remove(item);
                Destroy(item);
            }

            deadEntities.Clear();
        }
        
        private void spawnNewBlocks()
        {
            var tempSpawnPoints = new List<Transform>(SpawnPoints);
            tempSpawnPoints.ShuffleList();

            for (var i = 0; i < 3; i++)
            {
                var newBlock = Instantiate(
                    BlockPrefab,
                    gameObject.transform);

                newBlock.GetComponent<Rigidbody2D>().gravityScale = Gravity;

                newBlock.transform.position = tempSpawnPoints[i].transform.position;
                liveEntities.Add(newBlock);
            }
        }
    }
}
