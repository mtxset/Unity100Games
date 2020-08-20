using System.Collections.Generic;
using Components.UnityComponents;
using UnityEngine;

namespace Minigames.BubbleStruggle
{
    public class MinigameManager : MinigameManagerDefault
    {
        public GameObject FirstBall;
        public ChainEvents ChainEvents;
        public Transform BallSpawnPosition;

        [HideInInspector]
        public List<GameObject> Balls;

        protected override void UnityStart()
        {
            base.UnityStart();
            this.Balls = new List<GameObject>();
            this.ChainEvents = new ChainEvents();
            this.spawnFirstBall();
        }

        private void spawnFirstBall()
        {
            if (this.GameOver)
            {
                return;
            }
            
            var firstBall = Instantiate(
                FirstBall, this.BallSpawnPosition.position, Quaternion.identity, this.transform);
            
            this.Balls.Add(firstBall);
        }

        public void ResetBalls()
        {
            foreach (var item in this.Balls)
            {
                Destroy(item);
            }
            
            this.Balls.Clear();
            this.spawnFirstBall();
        }
    }
}