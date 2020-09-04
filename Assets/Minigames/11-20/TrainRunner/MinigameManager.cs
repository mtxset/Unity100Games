using Components.UnityComponents.v1;
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
            SlowMotionEvents = new SlowMotionEvents();
        }
    }
}
