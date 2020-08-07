using Assets.Minigames.InfiniteTunnels;
using UnityEngine;

namespace Assets.Minigames.RGBDestroyer
{
    class Wall : MonoBehaviour
    {
        public float GradientTraverseSpeed = 0;

        private void Update()
        {
            var traverseGradient = GradientRainbow.Next(
                this.GetComponent<LineRenderer>().colorGradient, 0.01f * this.GradientTraverseSpeed * Time.deltaTime);

            this.GetComponent<LineRenderer>().colorGradient = traverseGradient;
        }
    }
}
