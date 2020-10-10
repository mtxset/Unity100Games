using UnityEngine;

namespace GameManager
{
    public partial class GameManager
    {
        private void OnPlayerScored(int playerId, int score)
        {
            playersData[playerId].GameStateData.TotalScore += score;
        }

        /// <summary>
        /// Event to handle players death
        /// </summary>
        /// <param name="playerId">Player's id</param>
        private void OnPlayerDeath(int playerId)
        {
            if (Intermission)
            {
                return;
            }
            
            playersData[playerId].GameStateData.Alive = false;

            if (checkIfAllDied())
                StartCoroutine(intermissionStart());
        }

        private void OnPlayerVoted(int playerId, string vote) {
            playersData[playerId].GameStateData.LastVote = vote;
            var votes = new string[currentPlayerCount];

            // check if all voted
            for (var i = 0; i < currentPlayerCount; i++) {
                if (playersData[i].GameStateData.LastVote == string.Empty)
                    return;
                else
                    votes[i] = playersData[i].GameStateData.LastVote;
                    
            }
            // sends new vote
        }

    }
}
