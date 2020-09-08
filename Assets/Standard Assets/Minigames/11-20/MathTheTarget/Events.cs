using System;

namespace Minigames.MathTheTarget
{
    public class Events
    {
        public event Action OnShoot;
        public event Action OnDartReset;

        public void EventDartReset()
        {
            OnDartReset?.Invoke();
        }
        
        public void EventShoot()
        {
            OnShoot?.Invoke();
        }
    }
}