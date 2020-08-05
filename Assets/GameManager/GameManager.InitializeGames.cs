using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameManager
{
    public partial class GameManager
    {
        private void initializeGameList()
        {
            gameList = new List<Minigame>();

            gameList.Add(new Minigame // 0
            {
                MinigamePrefab = Resources.Load("DroneFly") as GameObject,
                MinigameManagerType = typeof(Minigames.DroneFly.MinigameManager),
            });

            gameList.Add(new Minigame // 1
            {
                MinigamePrefab = Resources.Load("PingPong") as GameObject,
                MinigameManagerType = typeof(Minigames.PingPong.MinigameManager)
            });

            gameList.Add(new Minigame // 2
            {
                MinigamePrefab = Resources.Load("Cannonizer") as GameObject,
                MinigameManagerType = typeof(Minigames.Cannonizer.CannonizerManager)
            });

            gameList.Add(new Minigame // 3
            {
                MinigamePrefab = Resources.Load("Barreling") as GameObject,
                MinigameManagerType = typeof(Minigames.Barreling.MinigameManager)
            });

            gameList.Add(new Minigame // 4
            {
                MinigamePrefab = Resources.Load("Flythrough") as GameObject,
                MinigameManagerType = typeof(Minigames.Flythrough.MinigameManager)
            });

            gameList.Add(new Minigame // 5
            {
                MinigamePrefab = Resources.Load("ZeFall") as GameObject,
                MinigameManagerType = typeof(Minigames.ZeFall.MinigameManager),
                Active = false
            });

            gameList.Add(new Minigame // 6
            {
                MinigamePrefab = Resources.Load("InfiniteTunnels") as GameObject,
                MinigameManagerType = typeof(Minigames.InfiniteTunnels.MinigameManager)
            });

            gameList.Add(new Minigame // 7
            {
                MinigamePrefab = Resources.Load("FallingRocks") as GameObject,
                MinigameManagerType = typeof(Minigames.FallingRocks.MinigameManager)
            });

            gameList.Add(new Minigame // 8
            {
                MinigameManagerType = typeof(Minigames.RGBDestroyer.MinigameManager),
                MinigamePrefab = Resources.Load("RGBDestroyer") as GameObject
            });   
            
            gameList.Add(new Minigame // 9
            {
                MinigameManagerType = typeof(Minigames.ColorJumper.MinigameManager),
                MinigamePrefab = Resources.Load("ColorJumper") as GameObject
            });

        }
    }
}
