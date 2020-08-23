using System;

namespace Minigames.MathTheTarget
{
    public class Events
    {
        public event Action OnShoot;
        public event Action OnDartReset;

        public void EventDartReset()
        {
            this.OnDartReset?.Invoke();
        }
        
        public void EventShoot()
        {
            this.OnShoot?.Invoke();
        }
    }
}