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
            DifficultyText.text = $"DIFFICULTY: {CurrentDifficulty * 100}";
        }

        private void Update()
        {
            checkDifficulty();
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
                CurrentDifficulty, vectors);

            Curve.TargetMovementSpeed = difficulty[0];
            Curve.TargetRotationSpeed = difficulty[1];
            Curve.MovementSpeed = difficulty[2];
            Curve.TargetScale = difficulty[3];
        }

        private void checkDifficulty()
        {
            if ((timer += Time.deltaTime) > IncreaseAfter)
            {
                if (CurrentDifficulty >= 1.0f)
                {
                    return;
                }
                
                CurrentDifficulty += IncreaseBy;
                DifficultyText.text = $"DIFFICULTY: {CurrentDifficulty * 100}";
                adjustDifficulty();
                timer = 0;
            }
        }
    }
}