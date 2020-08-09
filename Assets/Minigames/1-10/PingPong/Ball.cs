using UnityEngine;
using UnityEngine.UI;

namespace Minigames.PingPong
{
    class Ball : MonoBehaviour
    {
        public float MovementSpeed = 0;
        public Text SpeedText = null;

        public AudioSource HitAudio = null;
        public AudioSource ScoreAudio = null;
        public AudioSource DieAudio = null;
        public float AccelerationRate = 0;
        public float SplitFrequencySeconds = 0;

        private Rigidbody2D rigidbody2d;
        private MinigameManager gameManager;
        private float currentAcceleration = 1.0f;
        private float t = 0.0f;
        private float splitTimer = 0.0f;

        private void Start()
        {
            this.gameManager = GetComponentInParent<MinigameManager>();

            this.rigidbody2d = this.GetComponent<Rigidbody2D>();
            float x = Random.Range(0, 2) == 0 ? -1 : 1;
            float y = Random.Range(0, 2) == 0 ? -1 : 1;
            this.rigidbody2d.velocity = new Vector2(this.MovementSpeed * x, this.MovementSpeed * y);
        }

        private void Update()
        {
            this.splitTimer += Time.deltaTime;
            if (this.splitTimer > this.SplitFrequencySeconds)
            {
                Instantiate(this.gameObject, this.gameManager.transform);
                this.splitTimer = 0.0f;
            }
            this.increaseAcceleration();
        }

        private void increaseAcceleration()
        {
            t += Time.deltaTime;
            if (t > 2.0f)
            {
                this.rigidbody2d.velocity = new Vector2(
                    this.rigidbody2d.velocity.x * this.currentAcceleration,
                    this.rigidbody2d.velocity.y * this.currentAcceleration);

                this.currentAcceleration += this.AccelerationRate * Time.deltaTime;
                if (this.SpeedText != null)
                {
                    this.SpeedText.text = $"SPEED: {this.currentAcceleration}";
                }
                t = 0.0f;
            }

        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("hit"))
            {
                this.HitAudio.Play();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            switch (collision.gameObject.tag)
            {
                case "scorezone":
                    this.gameManager.Events.EventScored();
                    this.ScoreAudio.Play();
                    break;
                case "deadzone":
                    this.DieAudio.Play();
                    this.gameManager.Events.EventDeath();
                    break;
            }
        }
    }
}
