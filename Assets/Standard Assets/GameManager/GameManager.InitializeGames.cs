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
                    MinigamePrefab = Resources.Load("RGBDestroyer") as GameObject,
                    Active = false
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

                new Minigame // 16
                {
                    MinigamePrefab = Resources.Load("MissTheTarget") as GameObject,
                    MinigameManagerType = typeof(Minigames.MissTheTarget.MinigameManager),
                },

                new Minigame // 17
                {
                    MinigamePrefab = Resources.Load("CatchApple") as GameObject,
                    MinigameManagerType = typeof(Minigames.CatchApple.MinigameManager),
                },

                new Minigame // 18
                {
                    MinigamePrefab = Resources.Load("RoadDodger") as GameObject,
                    MinigameManagerType = typeof(Minigames.RoadDodger.MinigameManager),
                    Active = false
                },

                new Minigame // 19
                {
                    MinigamePrefab = Resources.Load("MathTheTarget") as GameObject,
                    MinigameManagerType = typeof(Minigames.MathTheTarget.MinigameManager),
                },

                new Minigame // 20
                {
                    MinigamePrefab = Resources.Load("Bomber") as GameObject,
                    MinigameManagerType = typeof(Minigames.Bomber.MinigameManager),
                },

                new Minigame // 21
                {
                    MinigamePrefab = Resources.Load("Dungeon") as GameObject,
                    MinigameManagerType = typeof(Minigames.Dungeon.MinigameManager),
                },

                new Minigame // 22
                {
                    MinigamePrefab = Resources.Load("FallForever") as GameObject,
                    MinigameManagerType = typeof(Minigames.FallForever.MinigameManager),
                },

                new Minigame // 23
                {
                    MinigamePrefab = Resources.Load("Snake") as GameObject,
                    MinigameManagerType = typeof(Minigames.Snake.MinigameManager),
                },

                new Minigame // 24
                {
                    MinigamePrefab = Resources.Load("Tetris") as GameObject,
                    MinigameManagerType = typeof(Minigames.Tetris.MinigameManager),
                },

                new Minigame // 25
                {
                    MinigamePrefab = Resources.Load("Minigolf") as GameObject,
                    MinigameManagerType = typeof(Minigames.Minigolf.MinigameManager),
                },

                new Minigame { // 26
                    MinigamePrefab = Resources.Load("CountObjects") as GameObject,
                    MinigameManagerType = typeof(Minigames.CountObjects.MinigameManager),
                },

                new Minigame { // 27 Preparation room
                    MinigamePrefab = Resources.Load("Preparation") as GameObject,
                    MinigameManagerType = typeof(Minigames.Preparation.MinigameManager),
                    Active = false
                },

                new Minigame { // 28 Intermission
                    MinigamePrefab = Resources.Load("Intermission") as GameObject,
                    MinigameManagerType = typeof(Minigames.Intermission.MinigameManager),
                    Active = false
                },

                new Minigame { // 29 HitClowd
                    MinigamePrefab = Resources.Load("HitClowd") as GameObject,
                    MinigameManagerType = typeof(Minigames.HitClowd.MinigameManager),
                },

                new Minigame { // 30 Rex
                    MinigamePrefab = Resources.Load("Rex") as GameObject,
                    MinigameManagerType = typeof(Minigames.Rex.MinigameManager)
                },

                new Minigame { // 31 Bingo
                    MinigamePrefab = Resources.Load("Bingo") as GameObject,
                    MinigameManagerType = typeof(Minigames.Bingo.MinigameManager),
                    Active = false // terrible idea and boring game
                },

                new Minigame { // 32 DoubleTrouble
                    MinigamePrefab = Resources.Load("DoubleTrouble") as GameObject,
                    MinigameManagerType = typeof(Minigames.DoubleTrouble.MinigameManager)
                },

								new Minigame { // 33 RepeatColors
										MinigamePrefab = Resources.Load("RepeatColors") as GameObject,
                    MinigameManagerType = typeof(Minigames.RepeatColors.MinigameManager)
								},

								new Minigame { // 34 Breakout
										MinigamePrefab = Resources.Load("Breakout") as GameObject,
                    MinigameManagerType = typeof(Minigames.Breakout.MinigameManager)
								},
            };

        }
    }
}
