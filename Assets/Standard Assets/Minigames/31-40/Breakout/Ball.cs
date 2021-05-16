using Components;
using UnityEngine;

namespace Minigames.Breakout
{
  public class Ball: AddMinigameManager2 {
		public AudioClip[] HitSounds;
		public float Speed;

		private Rigidbody2D rigidbody2d;
		private Vector2 initialPosition;
		private float currentAcceleration = 1.0f;

		private AudioSource audioSource;

		private void Start() {
			this.rigidbody2d = this.GetComponent<Rigidbody2D>();
			initialPosition = transform.position;
			resetBall();
			audioSource = GetComponent<AudioSource>();
		}

		private void FixedUpdate() {
			transform.Rotate(Vector3.forward * 10, Space.Self);
		}

		private void resetBall() {
				currentAcceleration = 1.0f;
				transform.position = initialPosition;
				rigidbody2d = GetComponent<Rigidbody2D>();
				float x = Random.Range(0, 2) == 0 ? -1 : 1;
				float y = Random.Range(0, 2) == 0 ? -1 : 1;
				rigidbody2d.velocity = new Vector2(Speed * x, Speed * y);
    }

		private void OnCollisionEnter2D(Collision2D other) {
			var randomClipIndex = Random.Range(0, HitSounds.Length - 1);
			audioSource.PlayOneShot(HitSounds[randomClipIndex]);
			if (other.gameObject.CompareTag("deadzone")) {
				resetBall();
				MinigameManager.Events.EventHit();
			} else if (other.gameObject.CompareTag("scorezone")) {
				MinigameManager.Events.EventScored();
				Destroy(other.gameObject);
			}
		}
	}
}
