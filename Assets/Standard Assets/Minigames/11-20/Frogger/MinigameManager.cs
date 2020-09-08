using Components.UnityComponents.v1;
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
            if ((timer += Time.deltaTime) >= IncreaseAfter)
            {
                Difficulty += IncreaseBy;
                timer = 0;
            }
        }
    }
}