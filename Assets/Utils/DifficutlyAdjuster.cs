﻿using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class DifficutlyAdjuster
    {
        /// <summary>
        /// Returns random values based on difficultly
        /// </summary>
        /// <param name="difficulty">0 - 1f</param>
        /// <param name="minMax">difficulty variables</param>
        public static float[] SpreadDifficulty(float difficulty, List<Vector2> minMax)
        {
            var difficultyDistributions = new float[minMax.Count];
            var difficultySettings = new float[minMax.Count];

            // calculating difficulty percentage for each vector
            for (int i = 0; i < minMax.Count; i++)
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
