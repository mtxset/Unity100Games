using System;
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
            this.gameManager = this.GetComponentInParent<MinigameManager>();
            
            this.gameManager.ButtonEvents.OnActionButtonPressed += HandleActionButtonPressed;
            
            this.resetChain();
        }

        private void OnDisable()
        {
            this.gameManager.ButtonEvents.OnActionButtonPressed -= HandleActionButtonPressed;
        }

        private void HandleActionButtonPressed()
        {
            if (this.isFired)
            {
                return;
            }
            
            this.gameManager.ChainEvents.EventFired();
            this.fireChain();
        }

        private void Update()
        {
            if (!isFired || this.gameManager.GameOver)
            {
                return;
            }
            
            this.Chain.transform.localScale = Vector3.Lerp(
                this.Chain.transform.localScale,
                this.targetScale,
                this.ShootSpeed * Time.deltaTime);
        }

        private void resetChain()
        {
            this.isFired = false;
            this.Chain.transform.localScale = new Vector3(1, 0, 1);
            this.targetScale = new Vector3(1, 0, 1);
        }
        
        private void fireChain()
        {
            this.Chain.transform.position = this.ShootPoint.transform.position;

            this.targetScale = new Vector3(1, 60, 1);

            this.isFired = true;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            this.resetChain();
            if (other.gameObject.CompareTag("scorezone"))
            {
                other.gameObject.GetComponent<Ball>().Split();
            }
        }
    }
}