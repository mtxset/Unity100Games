using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Minigames.MathTheTarget
{
    public class DifficultyController : MonoBehaviour
    {
        public Text DifficultyText;
        public Curve Curve;
        public Vector2 TargetMovementSpeedMinMax;
        public Vector2 TargetRotationSpeedMinMax;
        public Vector2 CrosshairMovementSpeedMin;
        public Vector2 TargetScaleMinMax;

        public float IncreaseBy;
        public float IncreaseAfter;

        public float CurrentDifficulty = 0.01f;

        private float timer;

        private void Start()
        {
            this.DifficultyText.text = $"DIFFICULTY: {this.CurrentDifficulty * 100}";
        }

        private void Update()
        {
            this.checkDifficulty();
        }

        private void adjustDifficulty()
        {
            var vectors = new List<Vector2>
            {
                TargetMovementSpeedMinMax,
                TargetRotationSpeedMinMax,
                CrosshairMovementSpeedMin,
                TargetScaleMinMax
            };

            var difficulty = DifficultyAdjuster.SpreadDifficulty(
                this.CurrentDifficulty, vectors);

            this.Curve.TargetMovementSpeed = difficulty[0];
            this.Curve.TargetRotationSpeed = difficulty[1];
            this.Curve.MovementSpeed = difficulty[2];
            this.Curve.TargetScale = difficulty[3];
        }

        private void checkDifficulty()
        {
            if ((this.timer += Time.deltaTime) > this.IncreaseAfter)
            {
                if (this.CurrentDifficulty >= 1.0f)
                {
                    return;
                }
                
                this.CurrentDifficulty += this.IncreaseBy;
                this.DifficultyText.text = $"DIFFICULTY: {this.CurrentDifficulty * 100}";
                this.adjustDifficulty();
                this.timer = 0;
            }
        }
    }
}