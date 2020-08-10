using System;
using UnityEngine;

namespace Minigames.Barreling
{
    public class Events
    {
        public event Action OnLanded;
        public void EventLanded() { OnLanded?.Invoke(); }

        public event Action<GameObject> OnDeath;
        /// <summary>
        /// Fires when drone dies (collides with anything tagged 'deadzone')
        /// </summary>
        public void EventDeath(GameObject barrelToShow)
        {
            OnDeath?.Invoke(barrelToShow);
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

        public event Action<int> OnScored;

        /// <summary>
        /// Fires when drone flies past obstacles
        /// </summary>
        public void EventScored(int points)
        {
            OnScored?.Invoke(points);
        }

    }
}
