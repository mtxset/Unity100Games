using Assets.Minigames.InfiniteTunnels;
using UnityEngine;

namespace Assets.Minigames.RGBDestroyer
{
    class Wall : MonoBehaviour
    {
        private Gradient colorGradient;
        public float GradientTraverseSpeed;

        private void Start()
        {
            this.colorGradient = this.GetComponent<LineRenderer>().colorGradient;
        }

        private void Update()
        {
            var traverseGradient = GradientRainbow.Next(
                this.GetComponent<LineRenderer>().colorGradient, 0.01f * this.GradientTraverseSpeed * Time.deltaTime);

            this.GetComponent<LineRenderer>().colorGradient = traverseGradient;
        }
    }
}
