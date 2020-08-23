using System.Collections.Generic;
using UnityEngine;

namespace Utils.Curves
{
    public static class Curves
    {
        public delegate Vector2 Function(float t);

        public static List<Vector2> LissajousCurve(
            Vector2 startingPoint,
            float stepBetweenPoints,
            float horizontalAngularFrequency = 3,
            float horizontalAngularPhase = Mathf.PI/2,
            float horizontalAplitude = 1,
            float limitFrom = 0,
            float limitTo = Mathf.PI*2,
            int xMultiplier = 1,
            int yMultiplier = 1)
        {
            var points = new List<Vector2>();
            var step = Mathf.Clamp(stepBetweenPoints, 0.001f, 0.1f);
            
            // https://www.wolframalpha.com/input/?i=Lissajous+curve
            // https://en.wikipedia.org/wiki/Lissajous_curve
            for (var i = limitFrom; i < limitTo; i += step)
            {
                var x = 
                    horizontalAplitude * 
                    Mathf.Sin(
                        horizontalAngularFrequency * (i*xMultiplier) + horizontalAngularPhase) +
                    startingPoint.x;
                var y = Mathf.Sin((i*yMultiplier)) + startingPoint.y;
                points.Add(new Vector2(x, y));
            }
            points.Add(points[0]);

            return points;
        }

        public static List<Vector2> LemniscateOfBernoulli(
            Vector2 startingPoint,
            float stepBetweenPoints,
            float limitFrom = 0,
            float limitTo = Mathf.PI * 2,
            float a = 1,
            int xMultiplier = 1,
            int yMultiplier = 1)
        {
            var points = new List<Vector2>();
            var step = Mathf.Clamp(stepBetweenPoints, 0.001f, 0.1f);
            
            // https://en.wikipedia.org/wiki/Lemniscate_of_Bernoulli
            for (var i = limitFrom; i < limitTo; i += step)
            {
                var t = new Vector2(i * xMultiplier, i * yMultiplier);
                var x =
                        (a * Mathf.Cos(t.x)) / 
                        (1 + Mathf.Sin(t.x) * Mathf.Sin(t.x));
                var y =
                        (a * Mathf.Sin(t.y) * Mathf.Cos(t.y)) / 
                        (1 + Mathf.Sin(t.y) * Mathf.Sin(t.y));
                
                points.Add(new Vector2(x, y) + startingPoint);
            }
            points.Add(points[0]);

            return points;
        }
        public static List<Vector2> LemniscateOfGerono(
            Vector2 startingPoint,
            float stepBetweenPoints,
            float limitFrom = 0,
            float limitTo = Mathf.PI * 2,
            int xMultiplier = 1,
            int yMultiplier = 1)
        {
            var points = new List<Vector2>();
            var step = Mathf.Clamp(stepBetweenPoints, 0.001f, 0.1f);
            
            // https://en.wikipedia.org/wiki/Lemniscate_of_Gerono
            for (var i = limitFrom; i < limitTo; i += step)
            {
                var t = new Vector2(i * xMultiplier, i * yMultiplier);
                var x = Mathf.Sin(t.x) + startingPoint.x;
                var y = Mathf.Sin(t.y) * Mathf.Cos(t.y) + startingPoint.y;
                points.Add(new Vector2(x, y));
            }
            points.Add(points[0]);

            return points;
        }
        
        
        
        // https://en.wikipedia.org/wiki/Lemniscate_of_Gerono
        public static Vector2 ComputeLemniscateOfGerono(float t)
        {
            return new Vector2(Mathf.Sin(t), Mathf.Sin(t) * Mathf.Cos(t));
        }
        
        public static List<Vector2> DynamicCurve(
            Vector2 startingPoint,
            float stepBetweenPoints,
            Vector2 limitFromTo,
            Function function)
        {
            var points = new List<Vector2>();
            var step = Mathf.Clamp(stepBetweenPoints, 0.001f, 0.1f);
            
            for (var i = limitFromTo.x; i < limitFromTo.y; i += step)
            {
                var result = function(i);
                result.x += startingPoint.x;
                result.y += startingPoint.y;
                points.Add(result);
            }
            points.Add(points[0]);

            return points;
        }
    }
}