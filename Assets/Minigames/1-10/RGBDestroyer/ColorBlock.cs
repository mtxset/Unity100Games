using UnityEngine;

namespace Minigames.RGBDestroyer
{
    class ColorBlock : MonoBehaviour
    {
        private MinigameManager gameManager;

        private void Start()
        {
            this.gameManager = GetComponentInParent<MinigameManager>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            switch (collision.gameObject.tag)
            {
                case "scorezone":
                    var color = this.GetComponent<SpriteRenderer>().color;
                    if (color == collision.GetComponent<LineRenderer>().startColor)
                    {
                        this.gameManager.Events.EventScored(1);
                        this.gameManager.SoundScored.Play();
                        Destroy(this.gameObject);
                    }
                    break;
                case "deadzone":
                    Destroy(this.gameObject);
                    this.gameManager.Events.EventHit();
                    break;
            }
        }
    }
}
