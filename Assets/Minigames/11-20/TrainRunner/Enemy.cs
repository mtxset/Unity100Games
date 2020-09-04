using UnityEngine;

namespace Minigames.TrainRunner
{
    public class Enemy : MonoBehaviour
    {
        private MinigameManager gameManager;

        private void Start()
        {
            gameManager = GetComponentInParent<MinigameManager>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("deadzone"))
            {
                gameManager.Events.EventHit();
            }
            else if (other.CompareTag("scorezone"))
            {
                gameManager.Events.EventScored();
            }
            
            gameObject.SetActive(false);
        }
    }
}