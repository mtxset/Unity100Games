#if UNITY_EDITOR 
using UnityEditor;
using UnityEngine;

namespace Utils.Curves
{
    [CustomEditor(typeof(SinWaveParameters))]
    public class SinWave : Editor
    {
        private void OnSceneGUI()
        {
            var p = target as SinWaveParameters;
            if (p == null)
            {
                return;
            }

            var step = Mathf.Clamp(p.Step, 0.001f, 0.1f);
            var center = p.transform.position;

            var points = Curves.SinWave(center, p.LimitFromTo, step);
            for (var i = 0; i < points.Count; i++)
            {
                if (i == points.Count - 1)
                    break;
                
                Handles.DrawLine(points[i], points[i+1]);
            }
        }
    }
}
#endif