using Minigames.InfiniteTunnels;
using UnityEngine;

namespace Minigames.RGBDestroyer
{
    class Wall : MonoBehaviour
    {
        public float GradientTraverseSpeed;
        private LineRenderer lineRenderer;

        private void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        private void Update()
        {
            var traverseGradient = GradientRainbow.Next(
                lineRenderer.colorGradient, 0.01f * GradientTraverseSpeed * Time.deltaTime);

            lineRenderer.colorGradient = traverseGradient;
        }
    }
}
