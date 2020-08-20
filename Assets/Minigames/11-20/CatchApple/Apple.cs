using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Random = UnityEngine.Random;

namespace Minigames.CatchApple
{
    public class Apple : MonoBehaviour
    {
        public Text DifficultyText;
        public Transform SpawnPoint;
        public Transform HandObject;

        public float CurrentDifficulty = 0.01f;
        public float IncreaseAfter = 1f;
        public float IncreaseBy = 0.01f;
        public Vector2 MaxYOffsetMinMax;
        public Vector2 FallSpeedMinMax;
        public float SpawnAfter;
        
        private Rigidbody2D rigidbody2d;
        private float timer;
        private float difficultyTimer;
        private MinigameManager gameManager;
        private bool fallingApple;
        private bool followHand;

        private void Start()
        {
            this.gameManager = this.GetComponentInParent<MinigameManager>();
            this.rigidbody2d = this.GetComponent<Rigidbody2D>();
            this.DifficultyText.text = $"FALL SPEED: {this.CurrentDifficulty * 100}";
            this.resetApple();
        }

        private void Update()
        {
            if (fallingApple || this.gameManager.GameOver)
            {
                return;
            }

            if (this.followHand)
            {
                this.transform.position = this.HandObject.transform.position;
            }

            if ((difficultyTimer += Time.deltaTime) >= this.IncreaseAfter)
            {
                if (this.CurrentDifficulty >= 1)
                {
                    return;
                }
                
                this.CurrentDifficulty += this.IncreaseBy;
                this.difficultyTimer = 0;
                this.DifficultyText.text = $"FALL SPEED: {this.CurrentDifficulty * 100}";
            }
            
            if ((this.timer += Time.deltaTime) >= this.SpawnAfter)
            {
                this.spawnApple();
                this.timer = 0;
            }
        }

        private void spawnApple()
        {
            this.gameManager.AllowHand = true;
            this.followHand = false;
            this.fallingApple = true;
            
            var vectors = new List<Vector2>
            {
                new Vector2(this.MaxYOffsetMinMax.y, this.MaxYOffsetMinMax.x),
                this.FallSpeedMinMax
            };
            var difficulty = DifficultyAdjuster.SpreadDifficulty(this.CurrentDifficulty, vectors);
            
            var newPosition = new Vector2(
                this.SpawnPoint.position.x,
                this.SpawnPoint.position.y + difficulty[0]);

            this.transform.position = newPosition;
            this.rigidbody2d.simulated = true;
            this.rigidbody2d.AddForce(Vector2.down * difficulty[1], ForceMode2D.Impulse);
        }

        private void resetApple()
        {
            this.fallingApple = false;
            this.rigidbody2d.velocity = Vector2.zero;
            this.rigidbody2d.simulated = false;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("scorezone"))
            {
                this.gameManager.Events.EventScored();
                this.followHand = true;
            }
            else if (other.gameObject.CompareTag("deadzone"))
            {
                this.gameManager.Events.EventHit();
            }
            
            this.resetApple();
        }
    }
}