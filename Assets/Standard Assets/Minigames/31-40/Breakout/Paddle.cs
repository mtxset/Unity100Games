using System;
using Components.UnityComponents;
using Components.UnityComponents.v2;
using UnityEngine;

namespace Minigames.Breakout
{
  public class Paddle: BasicControls {
		public GameObject Ball;
		public float Speed;

		private Rigidbody2D rigidbody2d;
		private float halfCollider;
		private MinigameManager2 gameManager;
		private const float halfAngle = 60 * Mathf.Deg2Rad;
		private const int paddleRotationSpeed = 10;
		private Ball ballScript;
		private int rotationDirection = -1;

		private void Start() {
			this.rigidbody2d = this.GetComponent<Rigidbody2D>();
			this.halfCollider = this.GetComponent<BoxCollider2D>().bounds.extents.x;
			gameManager = GetComponentInParent<MinigameManager2>();

			gameManager.ButtonEvents.OnHorizontalPressed += HandleHorizontalStateChange;
			gameManager.ButtonEvents.OnActionButtonPressed += HandleAction;
			ballScript = Ball.GetComponent<Ball>();
		}

		private void HandleAction()
		{
			if (rotationDirection == -1)
				rotationDirection = 1;
			else
				rotationDirection = -1;
		}

		private void OnDestroy() {
			gameManager.ButtonEvents.OnHorizontalPressed -= HandleHorizontalStateChange;
			gameManager.ButtonEvents.OnActionButtonPressed -= HandleAction;
		}

		private void FixedUpdate() {
			if (gameManager.GameOver) return;
			transform.Rotate(Vector3.forward * paddleRotationSpeed * rotationDirection, Space.Self);
			rigidbody2d.velocity = (float)HorizontalState * Time.fixedDeltaTime * Vector2.right * Speed;
		}
	}
}
