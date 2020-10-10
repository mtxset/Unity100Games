using System;
using System.Collections;
using System.Collections.Generic;
using Components;
using UnityEngine;
using UnityEngine.UI;

namespace GameManager
{
    public class Minigame
    {
        public GameObject MinigamePrefab;
        public Type MinigameManagerType;
        public GameObject PrestartGamePage;
        public bool Active = true;
    }

    public class Player
    {
        public int PlayerNumber;
        public Color PlayerColor;
        public Dictionary<Type, Component> InjectionInstance;
        public GameObject CurrentGamePrefab;
        public GameObject PlayersPrefabReference;
        public PlayerToManagerCommunicationBus PlayerToManagerCommunicationBusReference; 
        public GameStateData GameStateData;
    }

    /// <summary>
    /// Game state structure for each player
    /// </summary>
    public struct GameStateData
    {
        public int TotalScore;
        public int CurrentGameScore;
        public string LastVote;
        public bool Alive;
    }

    public partial class GameManager : MonoBehaviour
    {
        /// <summary>
        /// If -1 will random game, if -2 will start games one after another, 
        /// else will only launch game by index from <see cref="gameList"/>
        /// </summary>
        public int DebugGame = 10;

        public bool SkipPreparationRoom = false;

        public static GameManager Instance;

        public Text TimerText;
        public Text MainText;
        public GameObject InitialPage;
        public GameObject IntermissionPage;

        public bool Intermission;

        private const uint MAXPLAYERS = 6;
        private const int PREPARATIONROOM = 27;
        private List<Minigame> gameList;
        private Dictionary<int, Player> playersData;
        private int currentPlayerCount;
        private Queue<Color> colors;
        private int currentRandomGame = -1;

        // Intermission voting
        private enum VotingStages {
            SelectNextReplayVote,
            VoteParams,
            VoteAdjustment
        }
        private VotingStages currentVotingtage = VotingStages.SelectNextReplayVote;

        private string[] initialMenu = new string[] {
            "Next Random Game",
            "Replay",
            "Vote Params"
        };

        public string GetCurrentGameName() {
            return gameList[currentRandomGame].MinigamePrefab.name;
        }

        public int GetCurrentPlayersCount()
        {
            return currentPlayerCount;
        }

        public Dictionary<int, Player> GetPlayersData()
        {
            return playersData;
        }

        /// <summary>
        /// Creates prefab for new player which will hold games and other components
        /// </summary>
        /// <param name="newPlayerPrefab">Reference to player's prefab</param>
        /// <returns>button events reference</returns>
        public ButtonEvents AddNewPlayer(GameObject newPlayerPrefab)
        {
            MainText.text += "\nNew Player joined";
            return addFirstTimePlayer(newPlayerPrefab);
        }

        private IEnumerator countdown(int from, int to)
        {
            for (var i = from; i >= to; i--)
            {
                TimerText.text = i.ToString();
                yield return new WaitForSeconds(1);
            }
            startTheGame();
        }

        private void startTheGame()
        {
            InitialPage.SetActive(false);
            for (var i = 0; i < currentPlayerCount; i++)
            {
                playersData[i].PlayerToManagerCommunicationBusReference.MenuEntries = initialMenu;
                playersData[i].CurrentGamePrefab.SetActive(true);
            }
        }

        private void Awake()
        {
            TimerText.text = "";
            MainText.text = "Waiting for players..";

            StartCoroutine(countdown(3, 1));
            
            initializeGameList();
            initiateColors();
            selectRandomGame();
            Instance = this;

            playersData = new Dictionary<int, Player>();

            for (var i = 0; i < MAXPLAYERS; i++)
            {
                playersData.Add(i, new Player()
                {
                    PlayerNumber = i,
                    InjectionInstance = new Dictionary<Type, Component>(),
                    PlayersPrefabReference = null,
                    CurrentGamePrefab = null,
                    PlayerToManagerCommunicationBusReference = null,
                    GameStateData = new GameStateData()
                });
            }
        }

        private void selectRandomGame()
        {
            if (DebugGame >= 0)
            {
                // preselected game
                currentRandomGame = DebugGame;
            }
            else if (DebugGame == -2)
            {
                if (currentRandomGame < 0)
                    currentRandomGame = 0;
                // all games in a row
                while (!gameList[currentRandomGame++].Active);
            }
            else if (DebugGame == -1)
            {
                // games in random
                do
                {
                    var rand = new System.Random();
                    currentRandomGame = rand.Next(0, gameList.Count);
                }
                while (!gameList[currentRandomGame].Active);
            }
        }

        private void initiateColors()
        {
            colors = new Queue<Color>();
            colors.Enqueue(Color.green);
            colors.Enqueue(Color.yellow);
            colors.Enqueue(Color.blue);
            colors.Enqueue(Color.magenta);
            colors.Enqueue(Color.cyan);
            colors.Enqueue(Color.white);
        }

        /// <summary>
        /// Adding player for the first time
        /// </summary>
        /// <param name="playerPrefab">Reference to player's prefab</param>
        /// <returns>button events reference</returns>
        private ButtonEvents addFirstTimePlayer(GameObject playerPrefab)
        {
            // adding reference to playersPrefab to game manager's data
            playersData[currentPlayerCount].PlayersPrefabReference = playerPrefab;

            // Creating a mini game
            var randomGame = createNewMinigame(true);

            // set new game as player instance's child
            randomGame.transform.SetParent(playerPrefab.transform);

            // adding button events to player's prefab
            var buttonEventsReference = playerPrefab.AddComponent<ButtonEvents>();

            // adding interface for communication between mini game and singleton game manager
            var communicationBusReference = playerPrefab.AddComponent<PlayerToManagerCommunicationBus>();
            playersData[currentPlayerCount].PlayerToManagerCommunicationBusReference = communicationBusReference;

            communicationBusReference.PlayerId = currentPlayerCount;
            var color = colors.Dequeue();
            communicationBusReference.PlayerColor = color;
            playersData[currentPlayerCount].PlayerColor = color;

            // sub to events
            communicationBusReference.OnPlayerScored += OnPlayerScored;
            communicationBusReference.OnPlayerDeath += OnPlayerDeath;
            communicationBusReference.OnPlayerVoted += OnPlayerVoted;

            // offsetting scene for player
            playerPrefab.transform.position = new Vector3(0, -300 * currentPlayerCount, 0);
            randomGame.transform.position = playerPrefab.transform.position;

            // adding reference to mini game manager
            var comp = randomGame.GetComponentInChildren(gameList[currentRandomGame].MinigameManagerType);
            playersData[currentPlayerCount].InjectionInstance
                .Add(gameList[currentRandomGame].MinigameManagerType, comp);

            playersData[currentPlayerCount].CurrentGamePrefab = randomGame;

            // setting game state
            resetGameState(currentPlayerCount);

            // getting static viewports
            var viewPorts = Viewports.GetViewports(currentPlayerCount + 1);

            // setting cameras for each player
            for (var i = 0; i < currentPlayerCount + 1; i++) {
                playersData[i].CurrentGamePrefab.GetComponentInChildren<Camera>().rect = viewPorts[i];
            }

            currentPlayerCount++;

            // returning button events reference for each player
            return buttonEventsReference;
        }

        private void resetGameState(int playerId) {
            playersData[playerId].GameStateData.Alive = true;
            playersData[playerId].GameStateData.CurrentGameScore = 0;
            playersData[playerId].GameStateData.TotalScore = 0;
            playersData[playerId].GameStateData.LastVote = "";
        }

        /// <summary>
        /// Creates a minigame; if preparationRoom true sets to preparation room, used
        /// for letting know each player where he's located
        /// </summary>
        /// <param name="preparationRoom">if true sets to preparation room, used
        /// for letting know each player where he's located</param>
        /// <returns>Minigame prefab</returns>
        private GameObject createNewMinigame(bool preparationRoom = false) {
            if (preparationRoom && !SkipPreparationRoom)
                currentRandomGame = PREPARATIONROOM;
            // Creating game
            return Instantiate(gameList[currentRandomGame].MinigamePrefab);
        }

        private IEnumerator intermissionStart() {
            Intermission = true;
            // check if any player has total score over x
            yield return new WaitForSeconds(2);
            // disable games for all players
            for (var i = 0; i < currentPlayerCount; i++)
            {
                Destroy(playersData[i].CurrentGamePrefab);
            }
            // same game for everyone
            selectRandomGame();
            IntermissionPage.SetActive(true);
        }

        private void setNewGameForEveryPlayer()
        {
            // create new game for each player
            for (var i = 0; i < currentPlayerCount; i++)
            {
                var randomGame = createNewMinigame();
                randomGame.transform.SetParent(playersData[i].PlayersPrefabReference.transform);
                randomGame.transform.position = playersData[i].PlayersPrefabReference.transform.position;

                var comp = randomGame
                    .GetComponentInChildren(gameList[currentRandomGame].MinigameManagerType);

                // check if type already exists
                if (!playersData[i].InjectionInstance
                        .ContainsKey(gameList[currentRandomGame].MinigameManagerType))
                {
                    playersData[i].InjectionInstance
                       .Add(gameList[currentRandomGame].MinigameManagerType, comp);
                }

                // reset game state for each player
                
                playersData[i].GameStateData.Alive = true;

                playersData[i].CurrentGamePrefab = randomGame;
            }

            // getting static viewports
            var viewPorts = Viewports.GetViewports(currentPlayerCount);
            // set active new game for each player
            for (var i = 0; i < currentPlayerCount; i++)
            {
                playersData[i].CurrentGamePrefab.SetActive(true);

                // setting cameras for each player
                playersData[i].CurrentGamePrefab.GetComponentInChildren<Camera>().rect = viewPorts[i];
            }

            Intermission = false;
        }

        /// <summary>
        /// Checks if all players are dead
        /// </summary>
        /// <returns>true if all player have died</returns>
        private bool checkIfAllDied()
        {
            for (var i = 0; i < currentPlayerCount; i++) {
                if (playersData[i].GameStateData.Alive)
                    return false;
            }

            return true;
        }

        public void IntermissionDone()
        {
            IntermissionPage.SetActive(false);
            setNewGameForEveryPlayer();
        }

        private void calculateVotes(string[] votes) {
            if (currentVotingtage == VotingStages.SelectNextReplayVote) {

            }
            
        }

        private string getMostVoted(string[] votes) {
            
        }
    }
}