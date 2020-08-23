using System.Collections.Generic;
using UnityEngine;

namespace GameManager
{
    public partial class GameManager
    {
        private void initializeGameList()
        {
            gameList = new List<Minigame>
            {
                new Minigame // 0
                {
                    MinigamePrefab = Resources.Load("DroneFly") as GameObject,
                    MinigameManagerType = typeof(Minigames.DroneFly.MinigameManager),
                },

                new Minigame // 1
                {
                    MinigamePrefab = Resources.Load("PingPong") as GameObject,
                    MinigameManagerType = typeof(Minigames.PingPong.MinigameManager)
                },

                new Minigame // 2
                {
                    MinigamePrefab = Resources.Load("Cannonizer") as GameObject,
                    MinigameManagerType = typeof(Minigames.Cannonizer.CannonizerManager)
                },

                new Minigame // 3
                {
                    MinigamePrefab = Resources.Load("Barreling") as GameObject,
                    MinigameManagerType = typeof(Minigames.Barreling.MinigameManager)
                },

                new Minigame // 4
                {
                    MinigamePrefab = Resources.Load("Flythrough") as GameObject,
                    MinigameManagerType = typeof(Minigames.Flythrough.MinigameManager)
                },

                new Minigame // 5
                {
                    MinigamePrefab = Resources.Load("ZeFall") as GameObject,
                    MinigameManagerType = typeof(Minigames.ZeFall.MinigameManager),
                    Active = false
                },

                new Minigame // 6
                {
                    MinigamePrefab = Resources.Load("InfiniteTunnels") as GameObject,
                    MinigameManagerType = typeof(Minigames.InfiniteTunnels.MinigameManager)
                },

                new Minigame // 7
                {
                    MinigamePrefab = Resources.Load("FallingRocks") as GameObject,
                    MinigameManagerType = typeof(Minigames.FallingRocks.MinigameManager)
                },

                new Minigame // 8
                {
                    MinigameManagerType = typeof(Minigames.RGBDestroyer.MinigameManager),
                    MinigamePrefab = Resources.Load("RGBDestroyer") as GameObject
                },

                new Minigame // 9
                {
                    MinigameManagerType = typeof(Minigames.ColorJumper.MinigameManager),
                    MinigamePrefab = Resources.Load("ColorJumper") as GameObject
                },

                new Minigame // 10
                {
                    MinigameManagerType = typeof(Minigames.AAReplica.MinigameManager),
                    MinigamePrefab = Resources.Load("AAReplica") as GameObject
                },              
                
                new Minigame // 11
                {
                    MinigameManagerType = typeof(Minigames.AvoidRocket.MinigameManager),
                    MinigamePrefab = Resources.Load("AvoidRocket") as GameObject,
                    Active = false
                },
                
                new Minigame // 12
                {
                    MinigamePrefab = Resources.Load("FallingBlocks") as GameObject,
                    MinigameManagerType = typeof(Minigames.FallingBlocks.MinigameManager)
                },               
                
                new Minigame // 13
                {
                    MinigamePrefab = Resources.Load("TrainRunner") as GameObject,
                    MinigameManagerType = typeof(Minigames.TrainRunner.MinigameManager),
                    Active = false
                },
                
                new Minigame // 14
                {
                    MinigamePrefab = Resources.Load("Frogger") as GameObject,
                    MinigameManagerType = typeof(Minigames.Frogger.MinigameManager),
                },
                
                new Minigame // 15
                {
                    MinigamePrefab = Resources.Load("BubbleStruggle") as GameObject,
                    MinigameManagerType = typeof(Minigames.BubbleStruggle.MinigameManager),
                },
                
                new Minigame() // 16
                {
                    MinigamePrefab = Resources.Load("MissTheTarget") as GameObject,
                    MinigameManagerType = typeof(Minigames.MissTheTarget.MinigameManager),
                },
                
                new Minigame() // 17
                {
                    MinigamePrefab = Resources.Load("CatchApple") as GameObject,
                    MinigameManagerType = typeof(Minigames.CatchApple.MinigameManager),
                },
                
                new Minigame() // 18
                {
                    MinigamePrefab = Resources.Load("RoadDodger") as GameObject,
                    MinigameManagerType = typeof(Minigames.RoadDodger.MinigameManager),
                },
                
                new Minigame() // 19
                {
                    MinigamePrefab = Resources.Load("MathTheTarget") as GameObject,
                    MinigameManagerType = typeof(Minigames.MathTheTarget.MinigameManager),
                },
            };

        }
    }
}
