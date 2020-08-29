using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

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
            this.gameManager = this.GetComponentInParent<MinigameManager>();
            this.rigidbody2d = this.GetComponent<Rigidbody2D>();
            
            var difficulty = DifficultyAdjuster.SpreadDifficulty(
                this.gameManager.Difficulty, new List<Vector2>
                {
                  this.MovementSpeedMinMax  
                });

            this.currentSpeed = difficulty[0];
            Debug.Log(this.gameManager.Difficulty);
            Debug.Log(currentSpeed);
        }

        private void FixedUpdate()
        {
            var forward = new Vector2(transform.right.x, transform.right.y);
            this.rigidbody2d.MovePosition(
                this.rigidbody2d.position + forward * (Time.fixedDeltaTime * this.currentSpeed));
        }
    }
}