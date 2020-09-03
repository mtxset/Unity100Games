using Components;
using UnityEngine;

namespace Minigames.FallForever
{
    public class Player : AddMinigameManager2
    {
        public float MovementSpeed;
        private Rigidbody2D rigidbody2d;
        private void Start()
        {
            this.rigidbody2d = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            this.rigidbody2d.velocity = new Vector2(
                MovementSpeed * (int)this.MinigameManager.Controls.HorizontalState, rigidbody2d.velocity.y);
        }
    }
}