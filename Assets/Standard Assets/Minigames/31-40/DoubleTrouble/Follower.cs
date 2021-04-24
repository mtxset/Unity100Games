using System.Collections.Generic;
using Components;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Minigames.DoubleTrouble
{
    class Follower: AddMinigameManager2
    {
        public Text SpeedText;
        public AudioSource FollowerMovementSound; 
        public Transform Target;

        public Vector2 RotationSpeedMinMax;
        public Vector2 FlySpeedMinMax;

        public float RotationSpeed;
        public float FlySpeed;

        private float spawnTimer;

        private Rigidbody2D rigidBody2D;

        private Vector2 initialFollowerPos;

        private void Start() {
            initialFollowerPos = transform.position;
            this.rigidBody2D = this.GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate() {
            if (MinigameManager.GameOver)
                return;

            // difficulty check
            {
                var vectorList = new List<Vector2>{
                    RotationSpeedMinMax,
                    FlySpeedMinMax
                };

                var unparsed = DifficultyAdjuster.SpreadDifficulty(MinigameManager.DiffCurrent, vectorList);

                RotationSpeed = unparsed[0];
                FlySpeed = unparsed[1];

                SpeedText.text = $"DIFFICULTY: {Mathf.Round(MinigameManager.DiffCurrent * 100)}";
            }

            // following
            {
                // play sound?
                var direction = (Vector2)Target.position - rigidBody2D.position;
                direction.Normalize();

                var up = rigidBody2D.transform.up;
                var rotationAngle = Vector3.Cross(direction, up).z;

                rigidBody2D.angularVelocity = -rotationAngle * RotationSpeed;
                rigidBody2D.velocity = up * FlySpeed;
            }
        }

        private void OnCollisionEnter2D(Collision2D other) {
            if (other.collider.tag == "Player") {
                MinigameManager.Events.EventHit();

                transform.position = initialFollowerPos;
            }
        }
    }
}
