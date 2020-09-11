using Components.UnityComponents.v2;
using UnityEngine;

namespace Minigames.Tetris
{
    public class MinigameManager : MinigameManager2 {
        public GameObject Background;

        public TetrisEvents TetrisEvents;

        public readonly int MaxBlocksHeight = 20;
        public readonly int MaxBlocksWidth = 10;
        public Transform[,] Grid;

        protected override void UnityStart() {
            Grid = new Transform[MaxBlocksWidth, MaxBlocksHeight];
            TetrisEvents = new TetrisEvents();
            Background.transform.localScale = new Vector3(MaxBlocksWidth, MaxBlocksHeight, 1);
        }
    }
}