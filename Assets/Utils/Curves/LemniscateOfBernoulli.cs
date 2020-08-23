using UnityEditor;
using UnityEngine;

namespace Utils.Curves
{
    [CustomEditor(typeof(LemniscateOfBernoulliParameters))]
    public class LemniscateOfBernoulli : Editor
    {
        private void OnSceneGUI()
        {
            var p = target as LemniscateOfBernoulliParameters;
            if (p == null)
            {
                return;
            }

            var step = Mathf.Clamp(p.Step, 0.001f, 0.1f);
            var center = p.transform.position;

            var points = Curves.LemniscateOfBernoulli(
                center,
                step,
                p.LimitFrom,
                p.LimitTo,
                p.A,
                p.XMultiplier,
                p.YMultiplier);

            
            for (var i = 0; i < points.Count; i++)
            {
                if (i == points.Count - 1)
                    break;
                
                Handles.DrawLine(points[i], points[i+1]);
            }
        }
    }
}