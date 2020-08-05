using UnityEngine;

namespace Assets.Minigames.Cannonizer
{
    public class Soldier: MonoBehaviour
    {

        public AudioSource HitAudio;
        public AudioSource DieAudio;

        private CannonizerManager gameManager;

        private void Start()
        {
            this.gameManager = GetComponentInParent<CannonizerManager>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            switch (collision.gameObject.tag)
            {
                case "scorezone":
                    this.gameManager.Events.EventScored();
                    this.HitAudio.Play();
                    this.gameManager.EnemySpawnnerReference.RemoveEnemy(this.gameObject);
                    break;
                case "deadzone":
                    this.DieAudio.Play();
                    this.gameManager.Events.EventDeath();
                    break;
            }
        }
    }
}
