using UnityEngine;

namespace Minigames.MathTheTarget
{
    public class Dart : MonoBehaviour
    {
        public GameObject Crosshair;
        public GameObject DartPrefab;
        public float DeaccelerationSpeed;
        public float DartLaunchVelocity;
        
        private Vector3 initialPostion;
        private bool shooting;
        private Rigidbody dartRigidbody;
        private MinigameManager gameManager;

        private void Start()
        {
            this.gameManager = this.GetComponentInParent<MinigameManager>();

            this.dartRigidbody = this.DartPrefab.GetComponent<Rigidbody>();
            this.initialPostion = this.DartPrefab.transform.position;
            
            this.gameManager.ButtonEvents.OnActionButtonPressed += HandleActionButton;
        }

        private void OnDisable()
        {
            this.gameManager.ButtonEvents.OnActionButtonPressed -= HandleActionButton;
        }

        private void HandleActionButton()
        {
            if (this.shooting)
            {
                return;
            }
            
            this.DartPrefab.transform.position += new Vector3(
                this.Crosshair.transform.localPosition.x,
                this.Crosshair.transform.localPosition.y,
                0);
            this.dartRigidbody.velocity = new Vector3(0,0, this.DartLaunchVelocity);
            this.gameManager.DartEvents.EventShoot();
            this.shooting = true;
        }

        private void FixedUpdate()
        {
            if (shooting)
            {
                this.dartRigidbody.velocity -= 
                    new Vector3(0, 0, this.DeaccelerationSpeed * Time.fixedDeltaTime);
            }
            else
            {
                this.dartRigidbody.velocity = Vector3.zero;
                this.DartPrefab.transform.position = this.initialPostion;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("scorezone"))
            {
                var target = other.GetComponent<TargetPoints>();
                this.gameManager.Events.EventScored(target.Points);
            }
            else if (other.gameObject.CompareTag("deadzone"))
            {
                this.gameManager.Events.EventHit();
            }
            
            this.gameManager.DartEvents.EventDartReset(); 
            this.shooting = false;
        }
    }
}