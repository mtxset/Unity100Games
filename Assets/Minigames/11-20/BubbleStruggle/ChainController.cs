using UnityEngine;

namespace Minigames.BubbleStruggle
{
    public class ChainController : MonoBehaviour
    {
        public GameObject Chain;
        public Transform ShootPoint;
        public float ShootSpeed = 3f;
        
        private MinigameManager gameManager;
        private bool isFired; 
        private Vector3 targetScale;

        private void Start()
        {
            gameManager = GetComponentInParent<MinigameManager>();
            
            gameManager.ButtonEvents.OnActionButtonPressed += HandleActionButtonPressed;
            
            resetChain();
        }

        private void OnDisable()
        {
            gameManager.ButtonEvents.OnActionButtonPressed -= HandleActionButtonPressed;
        }

        private void HandleActionButtonPressed()
        {
            if (isFired)
            {
                return;
            }
            
            gameManager.ChainEvents.EventFired();
            fireChain();
        }

        private void Update()
        {
            if (!isFired || gameManager.GameOver)
            {
                return;
            }
            
            Chain.transform.localScale = Vector3.Lerp(
                Chain.transform.localScale,
                targetScale,
                ShootSpeed * Time.deltaTime);
        }

        private void resetChain()
        {
            isFired = false;
            Chain.transform.localScale = new Vector3(1, 0, 1);
            targetScale = new Vector3(1, 0, 1);
        }
        
        private void fireChain()
        {
            Chain.transform.position = ShootPoint.transform.position;

            targetScale = new Vector3(1, 60, 1);

            isFired = true;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            resetChain();
            if (other.gameObject.CompareTag("scorezone"))
            {
                other.gameObject.GetComponent<Ball>().Split();
            }
        }
    }
}