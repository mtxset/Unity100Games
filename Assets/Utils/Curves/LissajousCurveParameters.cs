using UnityEngine;

namespace Utils.Curves
{
    public class LissajousCurveParameters : MonoBehaviour
    {
        public float Step = 0.1f;
        public float HorizontalAngularFrequency = 3;
        public float HorizontalAngularPhase = Mathf.PI/2;
        public float HorizontalAplitude = 1;
        public float LimitFrom;
        public float LimitTo = Mathf.PI*2;
        public int XMultiplier = 1;
        public int YMultiplier = 1; 
    }
}