using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Minigames.Frogger
{
    public class CarController : MonoBehaviour
    {
        public Vector2 MovementSpeedMinMax;
        
        private Rigidbody2D rigidbody2d;
        private float currentSpeed;

        private void Start()
        {
            this.rigidbody2d = this.GetComponent<Rigidbody2D>();
            this.currentSpeed = Random.Range(this.MovementSpeedMinMax.x, this.MovementSpeedMinMax.y);
        }

        private void FixedUpdate()
        {
            var forward = new Vector2(transform.right.x, transform.right.y);
            this.rigidbody2d.MovePosition(
                this.rigidbody2d.position + forward * (Time.fixedDeltaTime * this.currentSpeed));
        }
    }
}