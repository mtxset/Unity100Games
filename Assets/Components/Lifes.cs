using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Components
{
    public class Lifes
    {
        private readonly List<GameObject> lifes;

        public Lifes(IEnumerable<GameObject> playerLifeObjects)
        {
            lifes = new List<GameObject>(playerLifeObjects);
            // making it so set life amount in inclusive (3 lives means 3 deaths and gg)
            lifes.Remove(lifes.Last());
        }

        public void ResetLifes()
        {
            foreach (var item in lifes)
            {
                item.SetActive(true);
            }
        }

        /// <summary>
        /// Lose a life
        /// </summary>
        /// <param name="amount">amount to lose</param>
        /// <returns>true if all lives depleted, false otherwise</returns>
        public bool LoseLife(int amount = 1)
        {
            var lostLifes = 0;
            foreach (var item in lifes)
            {
                if (item.activeSelf)
                {
                    item.SetActive(false);
                    lostLifes++;
                }

                if (lostLifes == amount)
                {
                    break;
                }
            }

            return (lostLifes == 0);
        }
    }
}