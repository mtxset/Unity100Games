using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class DifficultyAdjuster
    {
        /// <summary>
        /// Returns spread values based on difficultly in same order as provided vector list 
        /// </summary>
        /// <param name="difficulty">0 - 1f</param>
        /// <param name="minMax">difficulty variables</param>
        public static float[] SpreadDifficulty(float difficulty, List<Vector2> minMax)
        {
            difficulty = Mathf.Clamp(difficulty, 0, 1);
            var difficultyDistributions = new float[minMax.Count];
            var difficultySettings = new float[minMax.Count];

            // calculating difficulty percentage for each vector
            for (var i = 0; i < minMax.Count; i++)
            {
                if (i == minMax.Count - 1)
                {
                    difficultyDistributions[i] = difficulty;
                    break;
                }

                difficultyDistributions[i] = Random.Range(0, difficulty);
                difficulty -= difficultyDistributions[i];
            }

            // calculating difficulty for each vector
            for (var i = 0; i < minMax.Count; i++)
            {
                if (minMax[i].x <= minMax[i].y)
                {
                    difficultySettings[i] =
                        minMax[i].x +
                        difficultyDistributions[i] *
                        (minMax[i].y - minMax[i].x);
                }
                else
                {
                    difficultySettings[i] =
                        minMax[i].x -
                        difficultyDistributions[i] *
                        (minMax[i].x - minMax[i].y);
                }
            }

            return difficultySettings;
        }
    }
}
