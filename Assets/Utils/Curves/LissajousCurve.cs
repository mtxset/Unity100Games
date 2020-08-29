using UnityEditor;
using UnityEngine;

namespace Utils.Curves
{
#if UNITY_EDITOR 
    [CustomEditor(typeof(LissajousCurveParameters))]
    public class LissajousCurve : Editor
    {
        private void OnSceneGUI()
        {
            var p = target as LissajousCurveParameters;
            if (p == null)
            {
                return;
            }

            var step = Mathf.Clamp(p.Step, 0.001f, 0.1f);
            var center = p.transform.position;

            var points = Curves.LissajousCurve(
                center,
                step,
                p.HorizontalAngularFrequency,
                p.HorizontalAngularPhase,
                p.HorizontalAplitude,
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
#endif
}