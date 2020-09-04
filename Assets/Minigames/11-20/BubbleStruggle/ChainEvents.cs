using System;

namespace Minigames.BubbleStruggle
{
    public class ChainEvents
    {
        public event Action OnFired;
        public event Action OnHit;

        public void EventFired()
        {
            OnFired?.Invoke();
        }

        public void EventHit()
        {
            OnHit?.Invoke();
        }
    }
}