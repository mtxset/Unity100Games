using UnityEngine;

namespace Minigames.CatchApple
{
    public class SunRotate : MonoBehaviour
    {
        public float RotationSpeed;

        private void Update()
        {
            transform.Rotate(Vector3.forward, RotationSpeed * Time.deltaTime);
        }
    }
}