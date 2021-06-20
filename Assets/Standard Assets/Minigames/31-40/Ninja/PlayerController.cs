using System.Collections;
using Components.UnityComponents;
using Components.UnityComponents.v2;
using UnityEngine;

namespace Minigames.Ninja
{
  public class PlayerController : BasicControls {
		public AudioClip LightSound;
		public AudioClip[] MeteorDeathSounds;
		public AudioClip[] HitSounds;
		public BoxCollider2D LeftHit;
		public BoxCollider2D RightHit;
		public float MovementSpeed = 1f;

		private MinigameManager gameManager;
		private Animator animator;
		private SpriteRenderer spriteRenderer;
		private Rigidbody2D rigidbody2d;
		private BoxCollider2D boxCollider2d;

		private int playerIdle = Animator.StringToHash("idle");
		private int playerDeath = Animator.StringToHash("death");
		private int playerRunState = Animator.StringToHash("run");
		private int playerRunning = Animator.StringToHash("running");
		private int playerDodge = Animator.StringToHash("dodge");
		private int playerReset = Animator.StringToHash("reset");
		private int playerAttack = Animator.StringToHash("attack");

		private Vector2 screenSize;
		private int currentAnimationId;
		private BoxCollider2D currentHit;
		private AudioSource audioSource;

		private void Start() {
			rigidbody2d = GetComponent<Rigidbody2D>();
			spriteRenderer = GetComponent<SpriteRenderer>();
			animator = GetComponent<Animator>();
			gameManager = GetComponentInParent<MinigameManager>();
			boxCollider2d = GetComponent<BoxCollider2D>();
			audioSource = gameManager.AudioSource;

			screenSize.y = gameManager.CurrentCamera.orthographicSize * 2;
			screenSize.x = gameManager.CurrentCamera.orthographicSize * gameManager.CurrentCamera.aspect * 2;

			currentHit = LeftHit;
			subscribeToEvents();
		}

		private void OnDisable() {
			unsubscribeToEvents();
		}

		private void Update() {
			if (gameManager.GameOver) return;

			// hit
			currentHit.enabled = (currentAnimationId == playerAttack);

			currentAnimationId = animator.GetCurrentAnimatorStateInfo(0).shortNameHash;
			animator.SetBool(playerRunning, HorizontalState != AxisState.Idle);

			if (currentAnimationId == playerAttack || currentAnimationId == playerDodge) return;

			// move
			if (HorizontalState == AxisState.Negative) {
				spriteRenderer.flipX = true;
				currentHit = LeftHit;
			} else if (HorizontalState == AxisState.Positive) {
				spriteRenderer.flipX = false;
				currentHit = RightHit;
			}

			// light up
			if (VerticalState != AxisState.Idle && (currentAnimationId == playerRunState || currentAnimationId == playerIdle)) {
				animator.SetTrigger(playerDodge);
			}
		}

		private void lightAnimationEnd() {
			audioSource.PlayOneShot(LightSound);
			gameManager.PlayerLightAction();
		}

		private void FixedUpdate() {
			if (gameManager.GameOver) return;

			var movement = (float)HorizontalState * Time.fixedDeltaTime * MovementSpeed;

			if (movement == 0 || currentAnimationId == playerAttack || currentAnimationId == playerDodge) return;

			var newPosition = rigidbody2d.position + Vector2.right * movement;

			var bodyOffset = boxCollider2d.size.x * 4;
			var maxX = screenSize.x / 2;

			newPosition.x = Mathf.Clamp(newPosition.x, -maxX + bodyOffset, maxX - bodyOffset);

			rigidbody2d.MovePosition(newPosition);
		}

		private void subscribeToEvents() {
				gameManager.ButtonEvents.OnHorizontalPressed += HandleHorizontalStateChange;
				gameManager.ButtonEvents.OnVerticalPressed += HandleVerticalStateChange;
				gameManager.ButtonEvents.OnActionButtonPressed += HandleAction;
		}

		private void unsubscribeToEvents() {
				gameManager.ButtonEvents.OnHorizontalPressed -= HandleHorizontalStateChange;
				gameManager.ButtonEvents.OnVerticalPressed -= HandleVerticalStateChange;
				gameManager.ButtonEvents.OnActionButtonPressed -= HandleAction;
		}

		private void HandleAction() {
			if (currentAnimationId == playerRunState || currentAnimationId == playerIdle) {
				animator.SetTrigger(playerAttack);
				audioSource.PlayOneShot(HitSounds[Random.Range(0, HitSounds.Length - 1)]);
				StartCoroutine(playSoundWithDelay(HitSounds[Random.Range(0, HitSounds.Length - 1)], 0.3f));
			}
		}

		private void OnCollisionEnter2D(Collision2D other) {
			var tag = other.collider.gameObject.tag;
			if (tag == "deadzone" || tag == "hit") {
				gameManager.Events.EventHit();
				Destroy(other.gameObject);
			}
		}

		IEnumerator playSoundWithDelay(AudioClip clip, float delay) {
			yield return new WaitForSeconds(delay);
			audioSource.PlayOneShot(clip);
		}
  }
}
