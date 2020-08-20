using Components.UnityComponents;
using UnityEngine;

namespace Minigames.TrainRunner
{
    public class MinigameManager : MinigameManagerDefault
    {
        [HideInInspector]
        public SlowMotionEvents SlowMotionEvents;

        protected override void UnityStart()
        {
            base.UnityStart();
            this.SlowMotionEvents = new SlowMotionEvents();
        }
    }
}
