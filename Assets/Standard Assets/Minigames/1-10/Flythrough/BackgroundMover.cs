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
            gameManager = GetComponentInParent<MinigameManager>();
        }

        private void Update()
        {
            transform.Translate(Direction * (Speed * Time.deltaTime));

            if (transform.position.y <= EndY + gameManager.transform.position.y)
            {
                var transform1 = transform;
                transform1.position = new Vector2(transform1.position.x, StartY + gameManager.transform.position.y);
            }
        }
    }
}
