using UnityEditor;
using UnityEngine;

namespace Utils.Curves
{
    [CustomEditor(typeof(LemniscateOfGeronoParameters))]
    public class LemniscateOfGerono : Editor
    {
        private void OnSceneGUI()
        {
            var p = target as LemniscateOfGeronoParameters;
            if (p == null)
            {
                return;
            }

            var step = Mathf.Clamp(p.Step, 0.001f, 0.1f);
            var center = p.transform.position;

            var points = Curves.LemniscateOfGerono(
                center,
                step,
                p.LimitFrom,
                p.LimitTo,
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