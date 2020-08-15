using System;

namespace Minigames.TrainRunner
{
    public class SlowMotionEvents
    {
        public event Action OnStartShooting;
        public event Action OnEndShooting;
        public event Action<float> OnSpeedChanged;

        public void EventSpeedChanged(float speed)
        {
            this.OnSpeedChanged?.Invoke(speed);
        }
        public void EventStartShooting()
        {
            this.OnStartShooting?.Invoke();
        }

        public void EventEndShooting()
        {
            this.OnEndShooting?.Invoke();
        }
    }
}