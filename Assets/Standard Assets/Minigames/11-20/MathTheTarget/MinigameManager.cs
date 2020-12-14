using Components.UnityComponents.v2;

namespace Minigames.MathTheTarget
{
    public class MinigameManager : MinigameManager2
    {
        public Events DartEvents;

        protected override void UnityStart()
        {
            base.UnityStart();
            DartEvents = new Events();
        }
    }
}