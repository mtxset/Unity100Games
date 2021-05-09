using System;
using System.Collections.Generic;
using Components;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Random = UnityEngine.Random;

namespace Minigames.RepeatColors {
	class GameController: AddMinigameManager2 {
		public AudioClip StartTimerSound;
		public SpriteRenderer[] Directions;
		public Vector2 TimeInBetweenPlaysMinMax;
		public Vector2 WaitTimeMinMax;
		public Transform WaitTimer;
		public float TimerRotationMultiplier;
		public float Alpha;
		public Text SpeedText;
		public AudioClip[] SelectSounds;
		public Text ExplainText;

		public float DiffCurrent;
		public float DiffIncreaseBy;

		private Queue<int> currentSequence;
		private Queue<int> playersSequence;

		private bool sequencePlaying;
		private float sequenceTimer;
		private float waitTimer;
		private int lastSequenceIndex;
		private int currentSequenceCount;
		private int playersSequenceCount;
		private float initialTimerScale;
		private float currentTimeInBetweenPlays;
		private AudioSource audioSource;

		public void PlayerSelectsColor(int index) {
			if (!sequencePlaying) {
				var color = Directions[index].color;
				color.a = 1;
				Directions[index].color = color;

				var correctIndex = playersSequence.Dequeue();
				playersSequenceCount--;

				playRandomSelectSound();

				if (index != correctIndex) {
					MinigameManager.Events.EventHit();
					createRandomSequence();
				} else if (playersSequenceCount == 0) {
					MinigameManager.Events.EventScored();
					createRandomSequence();
				}
			}
		}

		private void Start() {
			audioSource = this.GetComponent<AudioSource>();
			createRandomSequence();
			initialTimerScale = WaitTimer.localScale.x;

			MinigameManager.ButtonEvents.OnLeftButtonPressed += HandleRed;
			MinigameManager.ButtonEvents.OnUpButtonPressed += HandleBlue;
			MinigameManager.ButtonEvents.OnRightButtonPressed += HandleGreen;
			MinigameManager.ButtonEvents.OnDownButtonPressed += HandleYellow;
		}

		private void HandleRed() => PlayerSelectsColor(0);
		private void HandleGreen() => PlayerSelectsColor(1);
		private void HandleBlue() => PlayerSelectsColor(2);
		private void HandleYellow() => PlayerSelectsColor(3);

		private void OnDisable() {
			MinigameManager.ButtonEvents.OnLeftButtonPressed -= HandleRed;
			MinigameManager.ButtonEvents.OnUpButtonPressed -= HandleBlue;
			MinigameManager.ButtonEvents.OnRightButtonPressed -= HandleGreen;
			MinigameManager.ButtonEvents.OnDownButtonPressed -= HandleYellow;
		}

		private void playRandomSelectSound() {
			var randomClipIndex = Random.Range(0, SelectSounds.Length - 1);
			audioSource.PlayOneShot(SelectSounds[randomClipIndex]);
		}

		private void createRandomSequence() {
			for (var i = 0; i < Directions.Length; i++) {
				var color = Directions[i].color;
				color.a = Alpha;
				Directions[i].color = color;
			}

			sequenceTimer = 0;
			waitTimer = 0;

			// adjust difficulty
			{
				var vectors = new List<Vector2> {
					// reverse because min is hardest, max is easiet
					new Vector2(TimeInBetweenPlaysMinMax.y, TimeInBetweenPlaysMinMax.x)
				};
				currentTimeInBetweenPlays = DifficultyAdjuster.SpreadDifficulty(DiffCurrent, vectors)[0];
				SpeedText.text = $"DIFFICULTY: {Mathf.Round(DiffCurrent * 100)}";

				DiffCurrent += DiffIncreaseBy;

				if (DiffCurrent > 1f)
					DiffCurrent = 1;
			}

			var sequence = new List<int>();
			for (var i = 0; i < Directions.Length; i++) {
				sequence.Add(i);
			}

			currentSequenceCount = sequence.Count;
			playersSequenceCount = sequence.Count;
			sequence.ShuffleList();

			currentSequence = new Queue<int>();
			playersSequence = new Queue<int>();
			foreach (var item in sequence) {
				currentSequence.Enqueue(item);
				playersSequence.Enqueue(item);
			}
			sequencePlaying = true;
			ExplainText.text = "Follow Sequence";
		}

		private void FixedUpdate() {
			if (MinigameManager.GameOver)
				return;

			// if playing we wait before jumping to next sequence
			if (sequencePlaying && (sequenceTimer += Time.fixedDeltaTime) > currentTimeInBetweenPlays) {
				sequenceTimer = 0;

				// hide last color
				var color = Directions[lastSequenceIndex].color;
				color.a = Alpha;
				Directions[lastSequenceIndex].color = color;

				if (currentSequenceCount == 0) {
					sequencePlaying = false;
					ExplainText.text = "Repeat Sequence";
					audioSource.PlayOneShot(StartTimerSound);
					return;
				}

				lastSequenceIndex = currentSequence.Dequeue();
				currentSequenceCount--;
				playRandomSelectSound();

				color = Directions[lastSequenceIndex].color;
				color.a = 1;
				Directions[lastSequenceIndex].color = color;
			}

			if (!sequencePlaying) {
				// WaitTimeMinMax.x - 100;
				// waitTimer - x;
				var temp = initialTimerScale - (waitTimer / WaitTimeMinMax.x) * initialTimerScale;
				WaitTimer.localScale = new Vector3(temp, temp, temp);
				WaitTimer.Rotate(Vector3.forward * (waitTimer / WaitTimeMinMax.x) * TimerRotationMultiplier, Space.Self);
			}

			if (!sequencePlaying && (waitTimer += Time.fixedDeltaTime) > WaitTimeMinMax.x) {
				// time run out
				waitTimer = 0;
				MinigameManager.Events.EventHit();
				createRandomSequence();
			}
		}
	}
}
