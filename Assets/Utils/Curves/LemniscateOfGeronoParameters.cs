using UnityEngine;

namespace Utils.Curves
{
    public class LemniscateOfGeronoParameters : MonoBehaviour
    {
        public float Step = 0.1f;
        public float LimitFrom;
        public float LimitTo = Mathf.PI*2;
        public int XMultiplier = 1;
        public int YMultiplier = 1; 
    }
}