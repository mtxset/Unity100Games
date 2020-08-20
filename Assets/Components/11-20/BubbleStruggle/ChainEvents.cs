using System;

namespace Minigames.BubbleStruggle
{
    public class ChainEvents
    {
        public event Action OnFired;
        public event Action OnHit;

        public void EventFired()
        {
            this.OnFired?.Invoke();
        }

        public void EventHit()
        {
            this.OnHit?.Invoke();
        }
    }
}