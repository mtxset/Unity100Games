using System;
using Components.UnityComponents;

namespace Minigames.MathTheTarget
{
    public class MinigameManager : MinigameManagerDefault
    {
        public Events DartEvents;

        protected override void UnityStart()
        {
            base.UnityStart();
            this.DartEvents = new Events();
        }
    }
}