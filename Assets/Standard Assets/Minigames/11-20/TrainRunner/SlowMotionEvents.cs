using System;

namespace Minigames.TrainRunner
{
    public class SlowMotionEvents
    {
        public event Action OnNoEnemies;
        public event Action OnReloaded; 
        public event Action OnStartShooting;
        public event Action OnEndShooting;
        public event Action<float> OnSpeedChanged;

        public void EventNoEnemies()
        {
            OnNoEnemies?.Invoke();
        }
        public void EventReloaded()
        {
            OnReloaded?.Invoke();
        }
        public void EventSpeedChanged(float speed)
        {
            OnSpeedChanged?.Invoke(speed);
        }
        public void EventStartShooting()
        {
            OnStartShooting?.Invoke();
        }

        public void EventEndShooting()
        {
            OnEndShooting?.Invoke();
        }
    }
}