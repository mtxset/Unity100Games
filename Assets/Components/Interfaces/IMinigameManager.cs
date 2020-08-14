using Components;

namespace Interfaces
{
    public interface IMinigameManager
    {
        /// <summary>
        /// Property for score
        /// </summary>
        int Score { get; set; }

        /// <summary>
        /// GameOver state
        /// </summary>
        bool GameOver { get; set; }
    
        /// <summary>
        /// Button events from Unity Input System
        /// </summary>
        ButtonEvents ButtonEvents { get; set; }

        /// <summary>
        /// Communication with Main game manager
        /// </summary>
        PlayerToManagerCommunicationBus CommunicationBus { get; set; }

        /// <summary>
        /// Handle Death event
        /// </summary>
        void HandleDeath();

        /// <summary>
        /// Handle player scored event
        /// </summary>
        /// <param name="points">ponts player scored</param>
        void HandleScored(int points);
    }
}
