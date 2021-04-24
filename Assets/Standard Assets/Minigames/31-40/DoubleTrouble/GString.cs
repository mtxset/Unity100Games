using UnityEngine;

namespace Minigames.DoubleTrouble
{
    public class GString : MonoBehaviour {
        public float GradientTraverseSpeed = 1f; 
        public float Limit = 10f;
        public float StepBetweenPoints = 1;
        public float Speed = 2f;
        public Vector2 LengthMinMax;
        public float Amplitude = 2f;

        public float SpeedSlowDownRate = 0.1f;

        private LineRenderer lineRenderer;

        private float currentSpeed;
        private float currentAmplitude;
        private float currentLength;

        void Start() {
            lineRenderer = GetComponent<LineRenderer>();

            currentSpeed = Speed;
            currentAmplitude = Amplitude;
            currentLength = 0;
        }

        void Update() {
            var step = Mathf.Clamp(StepBetweenPoints, 0.001f, 0.1f);
            int indices = (int)(Limit / step);
            var points = new Vector3[indices];
            lineRenderer.positionCount = indices;

            currentSpeed = Mathf.Lerp(currentSpeed, 0, SpeedSlowDownRate * Time.deltaTime);
            currentAmplitude = Mathf.Lerp(currentAmplitude, 0, SpeedSlowDownRate * Time.deltaTime);
            currentLength = Random.Range(LengthMinMax.x, LengthMinMax.y);

            for (var i = 0; i < indices; i++) {
                var pos = new Vector3(
                    i * step + transform.position.x,
                    Mathf.Sin(i * step * currentLength + (Time.time * currentSpeed)) * currentAmplitude + transform.position.y, 
                    0);
                lineRenderer.SetPosition(i, pos);
            }

            var traverseGradient = GradientRainbow.Next(lineRenderer.colorGradient, 0.01f * GradientTraverseSpeed * Time.deltaTime);

            lineRenderer.colorGradient = traverseGradient;
        }

        public void Play() {
            currentSpeed = Speed;
            currentAmplitude = Amplitude;
        }
    }
}