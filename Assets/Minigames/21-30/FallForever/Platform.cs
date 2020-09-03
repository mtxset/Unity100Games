using System;
using UnityEngine;

namespace Minigames.FallForever
{
    public class Platform : MonoBehaviour
    {
        public enum PlatformType
        {
            Simple,
            MovingLeft,
            MovingRight
        }

        public PlatformType SelectPlatformType;

        private void OnCollisionEnter2D(Collision2D other)
        {
            var rbVelocity = other.gameObject.GetComponent<Rigidbody2D>();
            switch (this.SelectPlatformType)
            {
                case PlatformType.Simple:
                    break;
                case PlatformType.MovingLeft:
                    rbVelocity.velocity = new Vector2(-1, rbVelocity.velocity.y);
                    break;
                case PlatformType.MovingRight:
                    rbVelocity.velocity = new Vector2(1, rbVelocity.velocity.y);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}