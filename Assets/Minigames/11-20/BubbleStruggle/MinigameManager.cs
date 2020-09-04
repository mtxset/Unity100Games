using System.Collections.Generic;
using Components.UnityComponents.v1;
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
            Balls = new List<GameObject>();
            ChainEvents = new ChainEvents();
            spawnFirstBall();
        }

        private void spawnFirstBall()
        {
            if (GameOver)
            {
                return;
            }
            
            var firstBall = Instantiate(
                FirstBall, BallSpawnPosition.position, Quaternion.identity, transform);
            
            Balls.Add(firstBall);
        }

        public void ResetBalls()
        {
            foreach (var item in Balls)
            {
                Destroy(item);
            }
            
            Balls.Clear();
            spawnFirstBall();
        }
    }
}