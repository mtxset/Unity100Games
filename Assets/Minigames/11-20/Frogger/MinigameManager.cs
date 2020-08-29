using System;
using Components.UnityComponents;
using UnityEngine;

namespace Minigames.Frogger
{
    public class MinigameManager : MinigameManagerDefault
    {
        public float Difficulty;

        public float IncreaseAfter;
        public float IncreaseBy;

        private float timer;
        private void Update()
        {
            if ((timer += Time.deltaTime) >= this.IncreaseAfter)
            {
                this.Difficulty += this.IncreaseBy;
                timer = 0;
            }
        }
    }
}