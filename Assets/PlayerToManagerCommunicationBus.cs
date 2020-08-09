using System;
using UnityEngine;

public class PlayerToManagerCommunicationBus : MonoBehaviour
{
    public int PlayerId;
    public Color PlayerColor;

    public event Action<int, int> OnPlayerScored;
    public event Action<int> OnPlayerDeath;

    /// <summary>
    /// Minigame should start the game
    /// </summary>
    public event Action OnIntermissionEnded;

    public void IntermissionEnded()
    {
        this.OnIntermissionEnded?.Invoke();
    }

    public void PlayerScored(int points)
    {
        this.OnPlayerScored?.Invoke(this.PlayerId, points);
    }

    public void PlayerDied()
    {
        this.OnPlayerDeath?.Invoke(this.PlayerId);
    }
}