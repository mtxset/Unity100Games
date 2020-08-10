using System;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.AAReplica
{
    internal class CircleRotator : MonoBehaviour
    {
        public float IncreaseRateAfter = 2f;
        public float IncreaseRateBy = 10f;
        public float RotationDegreesPerSecond = 100f;
        public Text SpeedText;

        private float difficultyTimer;
        private MinigameManager gameManager;
        private float initialRotationSpeed;

        private void Start()
        {
            this.gameManager = this.GetComponentInParent<MinigameManager>();
            this.SpeedText.text = $"SPEED: {this.RotationDegreesPerSecond}";

            this.gameManager.Events.OnHit += HandleHit;
            this.initialRotationSpeed = this.RotationDegreesPerSecond;
        }

        private void OnDisable()
        {
            this.gameManager.Events.OnHit -= HandleHit;
        }

        private void HandleHit()
        {
            this.RotationDegreesPerSecond = this.initialRotationSpeed;
        }

        private void Update()
        {
            this.difficultyTimer += Time.deltaTime;
            if (this.difficultyTimer >= this.IncreaseRateAfter)
            {
                this.RotationDegreesPerSecond += this.IncreaseRateBy;
                this.SpeedText.text = $"SPEED: {this.RotationDegreesPerSecond}";
                this.difficultyTimer = 0;
            }

            this.transform.Rotate(
                0, 0, this.RotationDegreesPerSecond * Time.deltaTime);
        }
    }
}
