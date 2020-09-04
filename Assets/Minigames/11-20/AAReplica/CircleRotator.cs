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
            gameManager = GetComponentInParent<MinigameManager>();
            SpeedText.text = $"SPEED: {RotationDegreesPerSecond}";

            gameManager.Events.OnHit += HandleHit;
            initialRotationSpeed = RotationDegreesPerSecond;
        }

        private void OnDisable()
        {
            gameManager.Events.OnHit -= HandleHit;
        }

        private void HandleHit()
        {
            RotationDegreesPerSecond = initialRotationSpeed;
        }

        private void Update()
        {
            difficultyTimer += Time.deltaTime;
            if (difficultyTimer >= IncreaseRateAfter)
            {
                RotationDegreesPerSecond += IncreaseRateBy;
                SpeedText.text = $"SPEED: {RotationDegreesPerSecond}";
                difficultyTimer = 0;
            }

            transform.Rotate(
                0, 0, RotationDegreesPerSecond * Time.deltaTime);
        }
    }
}
