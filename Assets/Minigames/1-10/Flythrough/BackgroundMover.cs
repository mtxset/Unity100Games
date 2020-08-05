using UnityEngine;

namespace Assets.Minigames.Flythrough
{
    class BackgroundMover : MonoBehaviour
    {
        public float Speed = 0;

        public float StartY = 0;
        public float EndY = 0;

        public Vector2 Direction = new Vector2();

        private MinigameManager gameManager;

        private void Start()
        {
            this.gameManager = GetComponentInParent<MinigameManager>();
        }

        private void Update()
        {
            this.transform.Translate(this.Direction * this.Speed * Time.deltaTime);

            if (this.transform.position.y <= this.EndY + this.gameManager.transform.position.y)
            {
                this.transform.position = new Vector2(transform.position.x, StartY + this.gameManager.transform.position.y);
            }
        }
    }
}
