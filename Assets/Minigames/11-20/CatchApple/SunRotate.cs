using System;
using UnityEngine;

namespace Minigames.CatchApple
{
    public class SunRotate : MonoBehaviour
    {
        public float RotationSpeed;

        private void Update()
        {
            this.transform.Rotate(Vector3.forward, this.RotationSpeed * Time.deltaTime);
        }
    }
}