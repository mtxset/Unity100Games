using UnityEngine;

namespace Minigames.Cannonizer
{
    public class Soldier: MonoBehaviour
    {

        public AudioSource HitAudio;
        public AudioSource DieAudio;

        private CannonizerManager gameManager;

        private void Start()
        {
            gameManager = GetComponentInParent<CannonizerManager>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            switch (collision.gameObject.tag)
            {
                case "scorezone":
                    gameManager.Events.EventScored();
                    HitAudio.Play();
                    gameManager.EnemySpawnnerReference.RemoveEnemy(gameObject);
                    break;
                case "deadzone":
                    DieAudio.Play();
                    gameManager.Events.EventDeath();
                    break;
            }
        }
    }
}
