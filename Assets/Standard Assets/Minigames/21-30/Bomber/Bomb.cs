using UnityEngine;

namespace Minigames.Bomber
{
    public class Bomb : MonoBehaviour
    {
        public GameObject Explosion;
        private MinigameManager gameManager;

        private void Start()
        { 
            gameManager = GetComponentInParent<MinigameManager>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var pe = Instantiate(Explosion);
            pe.transform.position = other.transform.position;
                
            Destroy(pe, 3f);
            gameObject.SetActive(false);
            
            if (other.CompareTag("scorezone"))
            {
                gameManager.PlatformHit(other.gameObject);

                gameManager.Events.EventScored();
            }
            else if(other.CompareTag("deadzone"))
            {
                gameManager.Events.EventHit();
            }
        }
    }
}