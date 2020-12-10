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
                return;
            
            playersData[playerId].GameStateData.Alive = false;

            // check if all died
            for (var i = 0; i < currentPlayerCount; i++) {
                if (playersData[i].GameStateData.Alive)
                    return;
            }
            
            if (PreviousGameId == -1) {
                StartCoroutine(intermissionStart(false, true));
            } else {
                StartCoroutine(intermissionStart(true, false));
            }
        }

        private void OnPlayerVoted(int playerId, string vote) {
            playersData[playerId].GameStateData.LastVote = vote;
            var votes = new string[currentPlayerCount];

            // check if all voted
            for (var i = 0; i < currentPlayerCount; i++) {
                if (playersData[i].GameStateData.LastVote == string.Empty)
                    return;
                else
                    // populate votes
                    votes[i] = playersData[i].GameStateData.LastVote;
            }
            // everyone voted if we get here
            calculateVotes(votes);
        }

    }
}
