using UnityEngine;

namespace Assets.Minigames.InfiniteTunnels
{
    public class GradientRainbow
    {
        /// <summary>
        /// Pass gradient which you would like to traverse
        /// </summary>
        /// <param name="gradient">Unity gradient</param>
        /// <param name="offset">Offset between 0-1, which controls speed of color traversion</param>
        static public Gradient Next(Gradient gradient, float offset)
        {
            if (offset > 1 || offset < 0) offset = 0.01f;

            var newGradientColors = new GradientColorKey[gradient.colorKeys.Length];

            for (int i = 0; i < gradient.colorKeys.Length; i++)
            {
                newGradientColors[i].color = gradient.colorKeys[i].color;
                
                if (gradient.colorKeys[i].time + offset > 1)
                {
                    newGradientColors[i].time = gradient.colorKeys[i].time + offset - 1;
                }
                else
                {
                    newGradientColors[i].time = gradient.colorKeys[i].time + offset;
                }
            }

            gradient.SetKeys(newGradientColors, gradient.alphaKeys);

            return gradient;
        }
    }
}
