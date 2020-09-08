using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class Oscillator
    {
        public struct OsccilateData
        {
            public IEnumerable<Vector2> Points;
            public float TraverseSpeed;
        }
        
        public static OsccilateData Osccilate(
            Vector2 currentPoint,
            Vector2 rotateAround,
            float precision,
            Vector2 angleMinMax,
            Vector2 traverseTimeMinMax,
            float difficulty)
        {
            var vectors = new List<Vector2>
            {
                angleMinMax,
                traverseTimeMinMax
            };
            var difficultyPoints = DifficultyAdjuster.SpreadDifficulty(difficulty, vectors);

            var direction = (Random.Range(0, 2) == 0) ? 1 : -1;
            
            var points = CalculatePointsInBetween(
                rotateAround,
                currentPoint,
                difficultyPoints[0],
                precision,
                direction);

            var data = new OsccilateData
            {
                Points = points,
                TraverseSpeed = difficultyPoints[1]
            };

            return data;
        }

        public static IEnumerable<Vector2> CalculatePointsInBetween(
            Vector2 rotateAround,
            Vector2 startingPoint,
            float angle,
            float precision,
            int direction)
        {
            var points = new List<Vector2>();

            var last = startingPoint;
            for (float i = 0; i < angle; i += precision)
            {
                // angle in radians
                var rads = precision * Mathf.PI / 180 * direction;

                var x = Mathf.Cos(rads) * (last.x - rotateAround.x) -
                        Mathf.Sin(rads) * (last.y - rotateAround.y) + rotateAround.x;
                
                var y = Mathf.Sin(rads) * (last.x - rotateAround.x) +
                        Mathf.Cos(rads) * (last.y - rotateAround.y) + rotateAround.y;
                
                last.x = x;
                last.y = y;
                
                points.Add(new Vector2(x, y));
            }

            return points;
        }
    }
}