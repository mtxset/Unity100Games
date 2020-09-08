using UnityEngine;

namespace Assets.Minigames.IsoRacer
{
    internal class MoveDirections
    {
        static Vector3 Left = new Vector3(0, 1, 0);
    }

    class Controller : MonoBehaviour
    {
        public float MoveSpeed = 4;
        public Camera CurrentCamera;
        public Vector3 Direction;

        private Vector3 forward, right;
        private float moveTimer;

        private void Start()
        {
            forward = CurrentCamera.transform.forward;
            forward.y = 0;
            forward = Vector3.Normalize(forward);

            right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
        }

        private void FixedUpdate()
        {
            movePlayer();
        }

        private void movePlayer()
        {
            var rightMovement = right * MoveSpeed * Time.fixedDeltaTime * Direction.x;
            var upMovement = forward * MoveSpeed * Time.fixedDeltaTime * Direction.y;

            var heading = Vector3.Normalize(rightMovement + upMovement);

            if (heading == Vector3.zero) return; 
            
            transform.forward = heading;
            transform.position += rightMovement;
            transform.position += upMovement;
        }
    }
}
