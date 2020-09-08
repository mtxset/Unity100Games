using System;

namespace Components
{
    public class EventsDefault
    {
        public event Action OnDodged;

        public void EventDodged()
        {
            OnDodged?.Invoke();
        }
        
        public event Action OnHit;

        public void EventHit()
        {
            OnHit?.Invoke();
        }

        public event Action OnDeath;
        /// <summary>
        /// Fires when drone dies (collides with anything tagged 'deadzone')
        /// </summary>
        public void EventDeath()
        {
            OnDeath?.Invoke();
        }

        public event Action OnGameStarted;

        public void EventGameStarted()
        {
            OnGameStarted?.Invoke();
        }

        public event Action<int> OnScored;

        /// <summary>
        /// Fires when drone flies past obstacles
        /// </summary>
        public void EventScored(int points = 1)
        {
            OnScored?.Invoke(points);
        }

    }
}
