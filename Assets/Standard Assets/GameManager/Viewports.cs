using System.Collections.Generic;
using UnityEngine;

namespace GameManager
{
    public class Viewports
    {
        /// <summary>
        /// Returns List of Rect for Cameras of each player
        /// </summary>
        /// <param name="players">Amount of players</param>
        /// <returns></returns>
        public static List<Rect> GetViewports(int players)
        {
            const float third = 1.0f / 3.0f;
            var viewports = new List<Rect>();
            switch (players)
            {
                case 1:
                    viewports.Add(new Rect(0, 0, 1, 1));
                    return viewports;
                case 2:
                    viewports.Add(new Rect(0.5f, 0.5f, 1, 1));
                    viewports.Add(new Rect(0.0f, 0, 0.5f, 0.5f));
                    return viewports;
                case 3:
                case 4:
                    viewports.Add(new Rect(0.0f, 0.5f, 0.5f, 0.5f));
                    viewports.Add(new Rect(0.5f, 0.5f, 0.5f, 0.5f));
                    viewports.Add(new Rect(0.0f, 0.0f, 0.5f, 0.5f));
                    viewports.Add(new Rect(0.5f, 0.0f, 0.5f, 0.5f));
                    return viewports;
                case 5:
                case 6:
                    viewports.Add(new Rect(third * 0, 0.0f, third, 0.5f));
                    viewports.Add(new Rect(third * 1, 0.0f, third, 0.5f));
                    viewports.Add(new Rect(third * 2, 0.0f, third, 0.5f));
                    viewports.Add(new Rect(third * 0, 0.5f, third, 0.5f));
                    viewports.Add(new Rect(third * 1, 0.5f, third, 0.5f));
                    viewports.Add(new Rect(third * 2, 0.5f, third, 0.5f));
                    return viewports;
                default:
                    return null;
            }
        }
    }
}
