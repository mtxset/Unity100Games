using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameManager
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
        public GameStateData GameStateData;
    }

    /// <summary>
    /// Game state structure for each player
    /// </summary>
    public struct GameStateData
    {
        public int TotalScore;
        public int CurrentGameScore;
        public bool Alive;
    }

    public partial class GameManager : MonoBehaviour
    {
        /// <summary>
        /// If 0 will random game, else will only launch game by index from <see cref="gameList"/>
        /// </summary>
        private readonly int debugGame = 10; 
        private const uint MAXPLAYERS = 9;
        private List<Minigame> gameList;
        private Dictionary<int, Player> playersData;
        private int currentPlayerCount = 0;
        private Queue<Color> colors;
        private int currentRandomGame;

        public static GameManager Instance;

        public Text TimerText;
        public Text MainText;
        public GameObject InitialPage;
        public GameObject IntermissionPage;

        public string GetCurrentGameName()
        {
            return gameList[currentRandomGame].MinigamePrefab.name;
        }

        public int GetCurrentPlayersCount()
        {
            return this.currentPlayerCount;
        }

        public Dictionary<int, Player> GetPlayersData()
        {
            return this.playersData;
        }

        /// <summary>
        /// Creates prefab for new player which will hold games and other components
        /// </summary>
        /// <param name="gameObject">Reference to player's prefab</param>
        /// <returns>button events reference</returns>
        public ButtonEvents AddNewPlayer(GameObject gameObject)
        {
            this.MainText.text += "\nNew Player joined";
            return this.addFirstTimePlayer(gameObject);
        }

        private async void countdown(int from, int to)
        {
            for (int i = from; i >= to; i--)
            {
                this.TimerText.text = i.ToString();
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
            this.startTheGame();
        }

        private void startTheGame()
        {
            this.InitialPage.SetActive(false);
            for (int i = 0; i < this.currentPlayerCount; i++)
            {
                this.playersData[i].CurrentGamePrefab.SetActive(true);
            }
        }

        private void Awake()
        {
            this.TimerText.text = "";
            this.MainText.text = "Waiting for players..";

            this.countdown(3, 1);
            
            this.initializeGameList();
            this.initiateColors();
            this.selectRandomGame();
            Instance = this;

            this.playersData = new Dictionary<int, Player>();

            for (int i = 0; i < MAXPLAYERS; i++)
            {
                this.playersData.Add(i, new Player()
                {
                    PlayerNumber = i,
                    InjectionInstance = new Dictionary<Type, Component>(),
                    PlayersPrefabReference = null,
                    CurrentGamePrefab = null,
                    GameStateData = new GameStateData()
                });
            }
        }

        private void selectRandomGame()
        {
            if (debugGame != 0)
            {
                this.currentRandomGame = debugGame;
            }
            else
            {
                while (!this.gameList[this.currentRandomGame].Active)
                {
                    var rand = new System.Random();
                    this.currentRandomGame = rand.Next(0, gameList.Count);
                }
            }
        }

        private void initiateColors()
        {
            this.colors = new Queue<Color>();
            colors.Enqueue(Color.green);
            colors.Enqueue(Color.yellow);
            colors.Enqueue(Color.blue);
            colors.Enqueue(Color.magenta);
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

            // Creating a minigame game
            var randomGame = this.createNewMinigame();

            // set new game as player instance's child
            randomGame.transform.SetParent(playerPrefab.transform);

            // adding button events to player's prefab
            var buttonEventsReference = playerPrefab.AddComponent<ButtonEvents>();

            // adding interface for communication between minigame and singleton gamemanager
            var communicationBusReference = playerPrefab.AddComponent<PlayerToManagerCommunicationBus>();
            communicationBusReference.PlayerId = currentPlayerCount;
            var color = this.colors.Dequeue();
            communicationBusReference.PlayerColor = color;
            playersData[currentPlayerCount].PlayerColor = color;

            // sub to events
            communicationBusReference.OnPlayerScored += OnPlayerScored;
            communicationBusReference.OnPlayerDeath += OnPlayerDeath;

            // offsetting scene for player
            playerPrefab.transform.position = new Vector3(0, -300 * currentPlayerCount, -10);
            randomGame.transform.position = playerPrefab.transform.position;

            // adding reference to minigame manager
            var comp = randomGame.GetComponentInChildren(gameList[this.currentRandomGame].MinigameManagerType);
            playersData[currentPlayerCount].InjectionInstance
                .Add(gameList[this.currentRandomGame].MinigameManagerType, comp);

            playersData[currentPlayerCount].CurrentGamePrefab = randomGame;

            // setting game state
            this.resetGameState(currentPlayerCount);
            playersData[currentPlayerCount].GameStateData.Alive = true;
            playersData[currentPlayerCount].GameStateData.CurrentGameScore = 0;
            playersData[currentPlayerCount].GameStateData.TotalScore = 0;

            // getting static viewports
            var viewPorts = Viewports.GetViewports(currentPlayerCount + 1);

            // setting cameras for each player
            for (int i = 0; i < currentPlayerCount + 1; i++)
            {
                playersData[i].CurrentGamePrefab.GetComponentInChildren<Camera>().rect = viewPorts[i];
            }

            currentPlayerCount++;

            // returning button events reference for each player
            return buttonEventsReference;
        }

        private void resetGameState(int playerId)
        {
            playersData[playerId].GameStateData.Alive = true;
            playersData[playerId].GameStateData.CurrentGameScore = 0;
            playersData[playerId].GameStateData.TotalScore = 0;
        }

        private GameObject createNewMinigame()
        {
            // Creating game
            var randomGame = Instantiate(gameList[this.currentRandomGame].MinigamePrefab);

            return randomGame;
        }

        private async void intermissionStart()
        {
            // check if any player has total score over x
            await Task.Delay(TimeSpan.FromSeconds(2));
            // disable games for all players
            for (int i = 0; i < currentPlayerCount; i++)
            {
                Destroy(playersData[i].CurrentGamePrefab);
            }
            // same game for everyone
            this.selectRandomGame();
            this.IntermissionPage.SetActive(true);
        }

        private void setNewGameForEveryPlayer()
        {
            // create new game for each player
            for (int i = 0; i < currentPlayerCount; i++)
            {
                var randomGame = this.createNewMinigame();
                randomGame.transform.SetParent(playersData[i].PlayersPrefabReference.transform);
                randomGame.transform.position = playersData[i].PlayersPrefabReference.transform.position;

                var comp = randomGame
                    .GetComponentInChildren(this.gameList[this.currentRandomGame].MinigameManagerType);

                // check if type already exists
                if (!playersData[i].InjectionInstance
                        .ContainsKey(this.gameList[this.currentRandomGame].MinigameManagerType))
                {
                    playersData[i].InjectionInstance
                       .Add(this.gameList[this.currentRandomGame].MinigameManagerType, comp);
                }

                // reset game state for each player
                
                this.playersData[i].GameStateData.Alive = true;

                playersData[i].CurrentGamePrefab = randomGame;
            }

            // getting static viewports
            var viewPorts = Viewports.GetViewports(currentPlayerCount);
            // set active new game for each player
            for (int i = 0; i < currentPlayerCount; i++)
            {
                playersData[i].CurrentGamePrefab.SetActive(true);

                // setting cameras for each player
                playersData[i].CurrentGamePrefab.GetComponentInChildren<Camera>().rect = viewPorts[i];
            }

        }


        /// <summary>
        /// Checks if all players died
        /// </summary>
        /// <returns>true if all player have died</returns>
        private bool checkIfAllDied()
        {
            for (int i = 0; i < this.currentPlayerCount; i++)
            {
                if (playersData[i].GameStateData.Alive)
                    return false;
            }

            return true;
        }

        public void IntermissionDone()
        {
            this.IntermissionPage.SetActive(false);
            this.setNewGameForEveryPlayer();
        }
    }
}