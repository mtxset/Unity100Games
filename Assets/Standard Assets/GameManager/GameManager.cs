using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public int PreviousGameId = -1;
        public int CurrentRandomGame = -1;

        [HideInInspector]
        public bool Intermission;

        private const uint MAXPLAYERS = 6;
        private const int PREPARATIONROOM = 27;
        private const int INTERMISSIONROOM = 28;
        private List<Minigame> gameList;
        private Dictionary<int, Player> playersData;
        private int currentPlayerCount;
        private Queue<Color> colors;

        // Intermission voting
        private enum VotingStages {
            SelectNextReplayVote,
            VoteParams,
            VoteAdjustment
        }

        private enum InitialVotingMenu {
            NextRandomGame,
            Replay,
            // VoteParams
        }

        private VotingStages currentVotingStage = VotingStages.SelectNextReplayVote;

        public string GetCurrentGameName() {
            return gameList[CurrentRandomGame].MinigamePrefab.name;
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
            var initialMenu = Enum.GetNames(typeof(InitialVotingMenu));
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

        private void selectRandomGame(int specificGame = -1) {
            PreviousGameId = CurrentRandomGame;
            if (specificGame >= 0) {
                CurrentRandomGame = specificGame;
                return;
            }

            if (DebugGame >= 0)
            {
                // preselected game
                CurrentRandomGame = DebugGame;
            }
            else if (DebugGame == -2)
            {
                // all games in a row
                do {
                    CurrentRandomGame++;

                    if (CurrentRandomGame < 0 || CurrentRandomGame >= gameList.Count)
                    CurrentRandomGame = 0;
                }
                while (!gameList[CurrentRandomGame++].Active);
            }
            else if (DebugGame == -1)
            {
                // games in random
                var rand = new System.Random();

                do CurrentRandomGame = rand.Next(0, gameList.Count);
                    while (!gameList[CurrentRandomGame].Active);
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
            var comp = randomGame.GetComponentInChildren(gameList[CurrentRandomGame].MinigameManagerType);
            playersData[currentPlayerCount].InjectionInstance
                .Add(gameList[CurrentRandomGame].MinigameManagerType, comp);

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
                CurrentRandomGame = PREPARATIONROOM;
            // Creating game
            return Instantiate(gameList[CurrentRandomGame].MinigamePrefab);
        }

        private IEnumerator intermissionStart(bool startIntermission, bool newRandomGame) {
            Intermission = true;
            // TODO: check if any player has total score over x
            yield return new WaitForSeconds(2);
            // disable games for all players
            for (var i = 0; i < currentPlayerCount; i++)
            {
                Destroy(playersData[i].CurrentGamePrefab);
            }
            
            // select intermission game
            if (startIntermission)
                selectRandomGame(INTERMISSIONROOM);
            else {
                if (newRandomGame)
                    selectRandomGame();
                else
                    selectRandomGame(PreviousGameId);
            }
            
            setNewGameForEveryPlayer();
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
                    .GetComponentInChildren(gameList[CurrentRandomGame].MinigameManagerType);

                // check if type already exists
                if (!playersData[i].InjectionInstance
                        .ContainsKey(gameList[CurrentRandomGame].MinigameManagerType))
                {
                    playersData[i].InjectionInstance
                       .Add(gameList[CurrentRandomGame].MinigameManagerType, comp);
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

        private void calculateVotes(string[] votes) {
            if (currentVotingStage == VotingStages.SelectNextReplayVote) {
                var mostVoted = 
                    (InitialVotingMenu) Enum.Parse(typeof(InitialVotingMenu), getMostVoted(votes));

                switch (mostVoted) {
                    case InitialVotingMenu.NextRandomGame:
                        StartCoroutine(intermissionStart(false, true));
                        break;
                    case InitialVotingMenu.Replay:
                        StartCoroutine(intermissionStart(false, false));
                        break;
                    // case InitialVotingMenu.VoteParams:
                    //     for (var i = 0; i < currentPlayerCount; i++) {
                    //         playersData[i].PlayerToManagerCommunicationBusReference.MenuEntries = 
                    //         playersData[i].PlayerToManagerCommunicationBusReference.NewVote();
                    //     }
                    //     break;
                }
            }
            
        }

        // quick select is faster, but our max is MAX_PLAYERS (which is 6 for now)
        private string getMostVoted(string[] votes) {
            var voteMap = new Dictionary<string, int>();
            for (var i = 0; i < votes.Length; i++) {
                if (!voteMap.ContainsKey(votes[i]))
                    voteMap.Add(votes[i], 1);
                else
                    voteMap[votes[i]]++; 
            }

            // linq magic
            return voteMap.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
        }
    }
}