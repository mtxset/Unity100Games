using UnityEngine;

namespace Assets.Minigames.ColorJumper
{
    class CameraFollow : MonoBehaviour
    {
        public Transform Player = null;

        private void Update()
        {
            if (this.Player.position.y > this.transform.position.y)
            {
                this.transform.position = new Vector3(
                    this.Player.position.x,
                    this.Player.position.y,
                    this.Player.position.z - 10.0f);
            }
        }
    }
}
