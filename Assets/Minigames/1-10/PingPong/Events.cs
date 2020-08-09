using System;

namespace Minigames.PingPong
{
    public class Events
    {
        public event Action OnDeath;
        /// <summary>
        /// Fires when drone dies (collides with anything tagged 'deadzone')
        /// </summary>
        public void EventDeath()
        {
            OnDeath?.Invoke();
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
