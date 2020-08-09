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
            playersData[playerId].GameStateData.Alive = false;

            if (this.checkIfAllDied())
                this.intermissionStart();
        }
    }
}
