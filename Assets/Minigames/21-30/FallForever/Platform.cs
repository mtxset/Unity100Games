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
            MovingRight,
            Ramp,
            RandomDirectionRamp
        }

        public float RandomJumpXOffset = 1;
        public float JumpPower = 20;
        public float SlideForce = 5f;
        public PlatformType SelectPlatformType;

        private void OnCollisionEnter2D(Collision2D other)
        {
            var player = other.gameObject.GetComponent<Player>();
            switch (SelectPlatformType)
            {
                case PlatformType.Simple:
                    break;
                case PlatformType.MovingLeft:
                    player.PlatformOffset = -SlideForce;
                    break;
                case PlatformType.MovingRight:
                    player.PlatformOffset = SlideForce;
                    break;
                case PlatformType.Ramp:
                    player.Jump(JumpPower);
                    break;
                case PlatformType.RandomDirectionRamp:
                    player.RandomJump(JumpPower, RandomJumpXOffset);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}