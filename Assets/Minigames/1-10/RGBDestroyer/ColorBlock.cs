using UnityEngine;

namespace Minigames.RGBDestroyer
{
    class ColorBlock : MonoBehaviour
    {
        private MinigameManager gameManager;

        private void Start()
        {
            gameManager = GetComponentInParent<MinigameManager>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (gameManager.GameOver)
            {
                return;
            }
            
            switch (collision.gameObject.tag)
            {
                case "scorezone":
                    var color = GetComponent<SpriteRenderer>().color;
                    if (color == collision.GetComponent<LineRenderer>().startColor)
                    {
                        gameManager.Events.EventScored();
                        gameManager.SoundScored.Play();
                        Destroy(gameObject);
                    }
                    break;
                case "deadzone":
                    Destroy(gameObject);
                    gameManager.Events.EventHit();
                    break;
            }
        }
    }
}
