using System;

namespace Assets.Minigames.DroneFly
{
    public class DroneEvents
    {
        public event Action OnDroneDeath;
        /// <summary>
        /// Fires when drone dies (collides with anything tagged 'deadzone')
        /// </summary>
        public void EventDroneDeath()
        {
            OnDroneDeath?.Invoke();
        }

        public event Action OnGameConfirmed;

        /// <summary>
        /// Fires when play confirms his score after death
        /// </summary>
        public void EventGameConfirmed()
        {
            OnGameConfirmed?.Invoke();
        }

        public event Action OnCountdownFinished;

        /// <summary>
        /// Fires when countdown (3-1) finishes
        /// </summary>
        public void EventCountdownFinished()
        {
            OnCountdownFinished?.Invoke();
        }

        public event Action OnGameStarted;

        public void EventGameStarted()
        {
            OnGameStarted?.Invoke();
        }

        public event Action OnScored;

        /// <summary>
        /// Fires when drone flies past obstacles
        /// </summary>
        public void EventScored()
        {
            OnScored?.Invoke();
        }

    }
}
