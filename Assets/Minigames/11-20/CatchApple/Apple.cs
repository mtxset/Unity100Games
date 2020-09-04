using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;

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
            gameManager = GetComponentInParent<MinigameManager>();
            rigidbody2d = GetComponent<Rigidbody2D>();
            DifficultyText.text = $"FALL SPEED: {CurrentDifficulty * 100}";
            resetApple();
        }

        private void Update()
        {
            if (fallingApple || gameManager.GameOver)
            {
                return;
            }

            if (followHand)
            {
                transform.position = HandObject.transform.position;
            }

            if ((difficultyTimer += Time.deltaTime) >= IncreaseAfter)
            {
                if (CurrentDifficulty >= 1)
                {
                    return;
                }
                
                CurrentDifficulty += IncreaseBy;
                difficultyTimer = 0;
                DifficultyText.text = $"FALL SPEED: {CurrentDifficulty * 100}";
            }
            
            if ((timer += Time.deltaTime) >= SpawnAfter)
            {
                spawnApple();
                timer = 0;
            }
        }

        private void spawnApple()
        {
            gameManager.AllowHand = true;
            followHand = false;
            fallingApple = true;
            
            var vectors = new List<Vector2>
            {
                new Vector2(MaxYOffsetMinMax.y, MaxYOffsetMinMax.x),
                FallSpeedMinMax
            };
            var difficulty = DifficultyAdjuster.SpreadDifficulty(CurrentDifficulty, vectors);
            
            var newPosition = new Vector2(
                SpawnPoint.position.x,
                SpawnPoint.position.y + difficulty[0]);

            transform.position = newPosition;
            rigidbody2d.simulated = true;
            rigidbody2d.AddForce(Vector2.down * difficulty[1], ForceMode2D.Impulse);
        }

        private void resetApple()
        {
            fallingApple = false;
            rigidbody2d.velocity = Vector2.zero;
            rigidbody2d.simulated = false;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("scorezone"))
            {
                gameManager.Events.EventScored();
                followHand = true;
            }
            else if (other.gameObject.CompareTag("deadzone"))
            {
                gameManager.Events.EventHit();
            }
            
            resetApple();
        }
    }
}