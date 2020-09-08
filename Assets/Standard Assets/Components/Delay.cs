using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

namespace Components
{
    public static class Delay
    {
        public delegate void CallbackMethod();
        public static IEnumerator StartDelay(
            float seconds, 
            Action callbackWhenDone,
            [CanBeNull] Action callbackAfterStep,
            float step = 0.1f)
        {
            for (float i = 0; i < seconds; i += step)    
            {
                callbackAfterStep?.Invoke();
                yield return new WaitForSeconds(step);
            }

            callbackWhenDone.Invoke();
        }
    }
}