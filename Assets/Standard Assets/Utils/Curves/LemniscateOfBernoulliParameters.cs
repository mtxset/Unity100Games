﻿using UnityEngine;

namespace Utils.Curves
{
    public class LemniscateOfBernoulliParameters : MonoBehaviour
    {
        public float Step = 0.1f;
        public float LimitFrom;
        public float LimitTo = Mathf.PI*2;
        public float A = 1;
        public int XMultiplier = 1;
        public int YMultiplier = 1; 
    }
}