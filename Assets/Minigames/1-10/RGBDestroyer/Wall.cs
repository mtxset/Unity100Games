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
            lineRenderer = this.GetComponent<LineRenderer>();
        }

        private void Update()
        {
            var traverseGradient = GradientRainbow.Next(
                lineRenderer.colorGradient, 0.01f * this.GradientTraverseSpeed * Time.deltaTime);

            this.lineRenderer.colorGradient = traverseGradient;
        }
    }
}
