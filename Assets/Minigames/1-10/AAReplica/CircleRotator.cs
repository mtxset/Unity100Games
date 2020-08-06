using UnityEngine;
using UnityEngine.UI;

namespace Assets.Minigames.AAReplica
{
    class CircleRotator : MonoBehaviour
    {
        public float IncreaseRateAfter = 2f;
        public float IncreaseRateBy = 10f;
        public float RotationDegreesPerSecond = 100f;
        public Text SpeedText;

        private float difficultyTimer = 0;

        private void Start()
        {
            this.SpeedText.text = $"SPEED: {this.RotationDegreesPerSecond}";
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
