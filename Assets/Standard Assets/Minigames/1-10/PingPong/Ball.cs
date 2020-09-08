using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Minigames.PingPong
{
    internal class Ball : MonoBehaviour
    {
        public float MovementSpeed;
        public Text SpeedText;

        public AudioSource HitAudio;
        public AudioSource ScoreAudio;
        public AudioSource DieAudio;
        public float AccelerationRate;
        public float SplitFrequencySeconds;

        private Rigidbody2D rigidbody2d;
        private MinigameManager gameManager;
        private float currentAcceleration = 1.0f;
        private float t;
        private float splitTimer;
        private List<GameObject> liveBalls;
        private Vector2 initialPosition;
        
        private void Start()
        {
            liveBalls = new List<GameObject>();
            gameManager = GetComponentInParent<MinigameManager>();
            initialPosition = transform.position;
            launchFirstBall();

            gameManager.Events.OnHit += HandleHit;
        }

        private void launchFirstBall()
        {
            currentAcceleration = 1.0f;
            transform.position = initialPosition;
            rigidbody2d = GetComponent<Rigidbody2D>();
            float x = Random.Range(0, 2) == 0 ? -1 : 1;
            float y = Random.Range(0, 2) == 0 ? -1 : 1;
            rigidbody2d.velocity = new Vector2(MovementSpeed * x, MovementSpeed * y);
        }

        private void OnDisable()
        {
            gameManager.Events.OnHit -= HandleHit;
        }

        private void HandleHit()
        {
            foreach (var item in liveBalls)
            {
                Destroy(item);
            }
            
            launchFirstBall();
            liveBalls.Clear();
        }

        private void Update()
        {
            splitTimer += Time.deltaTime;
            if (splitTimer > SplitFrequencySeconds)
            {
                liveBalls.Add(Instantiate(gameObject, gameManager.transform));
                splitTimer = 0;
            }
            increaseAcceleration();
        }

        private void increaseAcceleration()
        {
            t += Time.deltaTime;
            if (t > 2.0f)
            {
                var velocity = rigidbody2d.velocity;
                velocity = new Vector2(
                    velocity.x * currentAcceleration,
                    velocity.y * currentAcceleration);
                rigidbody2d.velocity = velocity;

                currentAcceleration += AccelerationRate * Time.deltaTime;
                if (SpeedText != null)
                {
                    SpeedText.text = $"SPEED: {currentAcceleration}";
                }
                t = 0.0f;
            }

        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("hit"))
            {
                HitAudio.Play();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            switch (collision.gameObject.tag)
            {
                case "scorezone":
                    gameManager.Events.EventScored();
                    ScoreAudio.Play();
                    break;
                case "deadzone":
                    DieAudio.Play();
                    gameManager.Events.EventHit();
                    break;
            }
        }
    }
}
