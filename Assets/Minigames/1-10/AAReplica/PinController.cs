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
            this.gameManager = this.GetComponentInParent<MinigameManager>();
            this.rigidbody2d = this.GetComponent<Rigidbody2D>();
            this.circleCollider = this.GetComponent<CircleCollider2D>();
        }

        private void Update()
        {
            if (pinned)
            {
                return;
            }

            this.rigidbody2d.MovePosition(
                this.rigidbody2d.position + Vector2.up * (this.ShootSpeed * Time.deltaTime));
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("scorezone"))
            {
                this.transform.SetParent(collision.transform);
                this.circleCollider.isTrigger = true;
                this.pinned = true;
                this.gameManager.Events.EventScored(1);
            }
            else if (collision.CompareTag("deadzone"))
            {
                collision.gameObject.SetActive(false);
                this.gameObject.SetActive(false);
                this.gameManager.Events.EventHit();
            }
        }
    }
}
