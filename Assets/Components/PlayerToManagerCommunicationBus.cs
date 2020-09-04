using System;
using UnityEngine;

namespace Components
{
    public class PlayerToManagerCommunicationBus : MonoBehaviour
    {
        public int PlayerId;
        public Color PlayerColor;

        public event Action<int, int> OnPlayerScored;
        public event Action<int> OnPlayerDeath;

        /// <summary>
        /// Mini game should start the game
        /// </summary>
        public event Action OnIntermissionEnded;

        public void IntermissionEnded()
        {
            OnIntermissionEnded?.Invoke();
        }

        public void PlayerScored(int points)
        {
            OnPlayerScored?.Invoke(PlayerId, points);
        }

        public void PlayerDied()
        {
            OnPlayerDeath?.Invoke(PlayerId);
        }
    }
}