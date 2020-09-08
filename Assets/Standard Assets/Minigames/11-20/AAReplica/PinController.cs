using UnityEngine;

namespace Minigames.AAReplica
{
    internal class PinController : MonoBehaviour
    {
        public float ShootSpeed = 50f;

        private bool pinned;
        private Rigidbody2D rigidbody2d;
        private MinigameManager gameManager;
        private CircleCollider2D circleCollider;

        private void Start()
        {
            gameManager = GetComponentInParent<MinigameManager>();
            rigidbody2d = GetComponent<Rigidbody2D>();
            circleCollider = GetComponent<CircleCollider2D>();
        }

        private void Update()
        {
            if (pinned)
            {
                return;
            }

            rigidbody2d.MovePosition(
                rigidbody2d.position + Vector2.up * (ShootSpeed * Time.deltaTime));
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("scorezone"))
            {
                transform.SetParent(collision.transform);
                circleCollider.isTrigger = true;
                pinned = true;
                gameManager.Events.EventScored();
            }
            else if (collision.CompareTag("deadzone"))
            {
                collision.gameObject.SetActive(false);
                gameObject.SetActive(false);
                gameManager.Events.EventHit();
            }
        }
    }
}
