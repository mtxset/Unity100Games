using UnityEngine;

namespace Minigames.MathTheTarget
{
    public class Dart : MonoBehaviour
    {
        public GameObject Mark;
        public GameObject Target;
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
            gameManager = GetComponentInParent<MinigameManager>();

            dartRigidbody = DartPrefab.GetComponent<Rigidbody>();
            initialPostion = DartPrefab.transform.position;
            
            gameManager.ButtonEvents.OnActionButtonPressed += HandleActionButton;
        }

        private void OnDisable()
        {
            gameManager.ButtonEvents.OnActionButtonPressed -= HandleActionButton;
        }

        private void HandleActionButton()
        {
            if (shooting)
            {
                return;
            }
            
            DartPrefab.transform.position += new Vector3(
                Crosshair.transform.localPosition.x,
                Crosshair.transform.localPosition.y,
                0);
            dartRigidbody.velocity = new Vector3(0,0, DartLaunchVelocity);
            gameManager.DartEvents.EventShoot();
            shooting = true;
        }

        private void FixedUpdate()
        {
            if (gameManager.GameOver)
            {
                return;
            }
            
            if (shooting)
            {
                dartRigidbody.velocity -= 
                    new Vector3(0, 0, DeaccelerationSpeed * Time.fixedDeltaTime);
            }
            else
            {
                dartRigidbody.velocity = Vector3.zero;
                DartPrefab.transform.position = initialPostion;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("scorezone"))
            {
                var target = other.GetComponent<TargetPoints>();
                leaveAMark(other.transform, transform.position);
                gameManager.Events.EventScored(target.Points);
            }
            else if (other.gameObject.CompareTag("deadzone"))
            {
                gameManager.Events.EventHit();
            }
            
            gameManager.DartEvents.EventDartReset(); 
            shooting = false;
        }
        
        private void leaveAMark(Transform setParentTo, Vector3 position)
        {
            var mark = Instantiate(Mark, position, Quaternion.identity, setParentTo);
            mark.transform.localScale /= setParentTo.localScale.x;
        }
    }
}