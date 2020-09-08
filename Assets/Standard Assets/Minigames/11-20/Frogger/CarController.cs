using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Minigames.Frogger
{
    public class CarController : MonoBehaviour
    {
        public Vector2 MovementSpeedMinMax;
        
        private Rigidbody2D rigidbody2d;
        private float currentSpeed;
        private MinigameManager gameManager;
        
        private void Start()
        {
            gameManager = GetComponentInParent<MinigameManager>();
            rigidbody2d = GetComponent<Rigidbody2D>();
            
            var difficulty = DifficultyAdjuster.SpreadDifficulty(
                gameManager.Difficulty, new List<Vector2>
                {
                  MovementSpeedMinMax  
                });

            currentSpeed = difficulty[0];
            Debug.Log(gameManager.Difficulty);
            Debug.Log(currentSpeed);
        }

        private void FixedUpdate()
        {
            var forward = new Vector2(transform.right.x, transform.right.y);
            rigidbody2d.MovePosition(
                rigidbody2d.position + forward * (Time.fixedDeltaTime * currentSpeed));
        }
    }
}