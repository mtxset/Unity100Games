using UnityEngine;

namespace Minigames.Flythrough
{
    class BackgroundMover : MonoBehaviour
    {
        public float Speed;

        public float StartY;
        public float EndY;

        public Vector2 Direction;

        private MinigameManager gameManager;

        private void Start()
        {
            this.gameManager = GetComponentInParent<MinigameManager>();
        }

        private void Update()
        {
            this.transform.Translate(this.Direction * (this.Speed * Time.deltaTime));

            if (this.transform.position.y <= this.EndY + this.gameManager.transform.position.y)
            {
                var transform1 = transform;
                transform1.position = new Vector2(transform1.position.x, StartY + this.gameManager.transform.position.y);
            }
        }
    }
}
