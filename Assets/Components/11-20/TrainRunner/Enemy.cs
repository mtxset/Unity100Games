using UnityEngine;

namespace Minigames.TrainRunner
{
    public class Enemy : MonoBehaviour
    {
        private MinigameManager gameManager;

        private void Start()
        {
            this.gameManager = this.GetComponentInParent<MinigameManager>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("deadzone"))
            {
                this.gameManager.Events.EventHit();
            }
            else if (other.CompareTag("scorezone"))
            {
                this.gameManager.Events.EventScored();
            }
            
            this.gameObject.SetActive(false);
        }
    }
}