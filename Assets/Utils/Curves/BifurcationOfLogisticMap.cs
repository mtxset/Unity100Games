using UnityEditor;
using UnityEngine;

namespace Utils.Curves
{
    [CustomEditor(typeof(BifurcationOfLogisticMapParameters))]
    public class BifurcationOfLogisticMap : Editor
    {
        private void OnSceneGUI()
        {
            var p = target as BifurcationOfLogisticMapParameters;
            if (p == null)
            {
                return;
            }

            var step = Mathf.Clamp(p.Step, 0.001f, 0.1f);
            var center = p.transform.position;

            var points = Curves.BifurcationOfLogisticMap(
                center,
                step,
                p.RLimitFrom,
                p.RLimitTo,
                p.IterationsInLogisticMap,
                p.InitialX);

            for (var i = 0; i < points.Count; i++)
            {
                if (i == points.Count - 1)
                    break;
                var point = new Vector2(
                    points[i].x + p.PointOffset.x, 
                    points[i].y + p.PointOffset.y);
                Handles.DrawLine(points[i], point);
            }
        }
    }
}