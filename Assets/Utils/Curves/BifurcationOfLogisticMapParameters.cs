using UnityEngine;

namespace Utils.Curves
{
    public class BifurcationOfLogisticMapParameters : MonoBehaviour
    {
        public float Step = 0.01f;
        public float RLimitFrom = -4;
        public float RLimitTo = 4;
        public float InitialX = 0.1f;
        public int IterationsInLogisticMap = 10;
        public Vector2 PointOffset; // y = 0.003 
    }
}