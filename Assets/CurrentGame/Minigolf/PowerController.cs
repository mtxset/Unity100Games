using UnityEngine;

namespace Minigames.Minigolf {
    public class PowerController : MonoBehaviour {
        public GameObject Ball;
        public GameObject PowerBar;
        public LevelGenerator LevelGenerator;

        public float IncreaseBy = 0.1f;
        public float Force = 10f;
        public float Friction = 0.01f;
        public float StopThreshold = 0.01f;
        public Vector2 BallVelocity;

        private Rigidbody2D ballRigidBody2d;

        private void Start() {
            ballRigidBody2d = Ball.GetComponent<Rigidbody2D>();
        }

        public void RotateBar(int leftRight) {
            PowerBar.transform.RotateAround(
                Ball.transform.position, 
                Vector3.forward * leftRight, 30f);
        }

        public void IncreasePower() {
            PowerBar.transform.localScale -= new Vector3(0, IncreaseBy, 0);
        }

        public void Shoot() {
            ballRigidBody2d.AddForce(PowerBar.transform.up * Force, ForceMode2D.Impulse);
        }

        private void Update() {
            BallVelocity = ballRigidBody2d.velocity;
            drawIfStopped();
        }

        private void drawIfStopped() {
            var state = Mathf.Abs(ballRigidBody2d.velocity.x) < StopThreshold 
                && Mathf.Abs(ballRigidBody2d.velocity.y) < StopThreshold;

            PowerBar.SetActive(state);
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag("scorezone")) {
                ballRigidBody2d.velocity = Vector2.zero;
                LevelGenerator.NextMap();
            }
        }
    }
}