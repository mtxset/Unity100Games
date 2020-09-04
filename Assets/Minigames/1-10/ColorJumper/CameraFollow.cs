using UnityEngine;

namespace Minigames.ColorJumper
{
    internal class CameraFollow : MonoBehaviour
    {
        public Transform Player;

        private void Update()
        {
            if (!(Player.position.y > transform.position.y)) return;
            
            var position = Player.position;
            transform.position = new Vector3(
                position.x,
                position.y,
                position.z - 10.0f);
        }
    }
}
