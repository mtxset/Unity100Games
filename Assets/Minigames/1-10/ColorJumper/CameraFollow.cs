using UnityEngine;

namespace Minigames.ColorJumper
{
    internal class CameraFollow : MonoBehaviour
    {
        public Transform Player;

        private void Update()
        {
            if (!(this.Player.position.y > this.transform.position.y)) return;
            
            var position = this.Player.position;
            this.transform.position = new Vector3(
                position.x,
                position.y,
                position.z - 10.0f);
        }
    }
}
