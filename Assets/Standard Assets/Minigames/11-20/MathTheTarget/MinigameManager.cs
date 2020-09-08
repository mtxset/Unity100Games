using Components.UnityComponents.v1;

namespace Minigames.MathTheTarget
{
    public class MinigameManager : MinigameManagerDefault
    {
        public Events DartEvents;

        protected override void UnityStart()
        {
            base.UnityStart();
            DartEvents = new Events();
        }
    }
}