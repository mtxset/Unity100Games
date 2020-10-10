using System;
using UnityEngine;

namespace Components
{
    public class PlayerToManagerCommunicationBus : MonoBehaviour
    {
        public int PlayerId;
        public Color PlayerColor;
        public string[] MenuEntries;

        // Minigame to GameManager Events
        public event Action<int, int> OnPlayerScored;
        public event Action<int> OnPlayerDeath;
        public event Action<int, string> OnPlayerVoted;

        // GameManager to Minigame Events
        public event Action OnIntermissionEnded;
        public event Action OnNewVote;
        public event Action OnEndVoting;

        public void NewVote() => OnNewVote?.Invoke();
        public void EndVoting() => OnEndVoting?.Invoke();

        public void PlayerVoted(string vote) =>
            OnPlayerVoted?.Invoke(PlayerId, vote);

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