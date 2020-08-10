using System;
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
            this.screenHalfSizeWorldUnits = new Vector2(
                this.CurrentCamera.aspect * (orthographicSize = this.CurrentCamera.orthographicSize),
                orthographicSize);
            
            this.liveEntities = new List<GameObject>();
            this.deadEntities = new List<GameObject>();
            
            this.gameManager = this.GetComponentInParent<MinigameManager>();
            this.spawnTimer = this.SpawnRate;

            this.initialGravity = this.Gravity;
            
            this.SpeedText.text = $"GRAVITY: {this.Gravity}";

            this.gameManager.Events.OnHit += HandleHit;
        }

        private void OnDisable()
        {
            this.gameManager.Events.OnHit -= HandleHit;
        }

        private void HandleHit()
        {
            this.deadEntities.AddRange(liveEntities);
            this.Gravity = initialGravity;
        }

        private void Update()
        {    
            if (this.gameManager.GameOver)
            {
                return;
            }
            
            if ((spawnTimer += Time.deltaTime) >= this.SpawnRate)
            {
                this.spawnNewBlocks();
                this.gameManager.Events.EventScored(1);
                this.spawnTimer = 0;
            }
            
            if ((this.difficultyTimer += Time.deltaTime) >= this.IncreaseRateAfter)
            {
                this.Gravity += IncreaseRateBy;
                this.SpeedText.text = $"GRAVITY: {this.Gravity}";
                this.difficultyTimer = 0;
            }

            this.blockLifecycle();
        }

        private void blockLifecycle()
        {
            foreach (var entity in liveEntities)
            {
                var belowY = -this.screenHalfSizeWorldUnits.y
                             - entity.transform.localScale.y
                             + this.gameManager.transform.position.y;
                
                if (entity.transform.position.y < belowY)
                {
                    this.deadEntities.Add(entity);
                }
            }
            
            foreach (var item in deadEntities)
            {
                this.liveEntities.Remove(item);
                Destroy(item);
            }

            this.deadEntities.Clear();
        }
        
        private void spawnNewBlocks()
        {
            var tempSpawnPoints = new List<Transform>(this.SpawnPoints);
            tempSpawnPoints.ShuffleList();

            for (var i = 0; i < 3; i++)
            {
                var newBlock = Instantiate(
                    this.BlockPrefab,
                    this.gameObject.transform);

                newBlock.GetComponent<Rigidbody2D>().gravityScale = Gravity;

                newBlock.transform.position = tempSpawnPoints[i].transform.position;
                this.liveEntities.Add(newBlock);
            }
        }
    }
}
