using System.Collections;
using UnityEngine;

namespace Minigames.CatchApple
{
    public class HandController : MonoBehaviour
    {
        public Transform ShoveHandPosition;
        public float HoldHandForSeconds;

        private Vector2 initialPosition;
        private float timer;
        private MinigameManager gameManager;

        public void Start()
        {
            gameManager = GetComponentInParent<MinigameManager>();
            initialPosition = transform.position;

            gameManager.ButtonEvents.OnActionButtonPressed += HandleAction;
        }

        private void OnDisable()
        {
            gameManager.ButtonEvents.OnActionButtonPressed -= HandleAction;
        }

        private void HandleAction()
        {
            if (gameManager.GameOver || !gameManager.AllowHand)
            {
                return;
            }
            
            StartCoroutine(shoveHand());
            gameManager.AllowHand = false;
        }

        private IEnumerator shoveHand()
        {
            transform.position = ShoveHandPosition.position;

            yield return new WaitForSeconds(HoldHandForSeconds);
            
            transform.position = initialPosition;
        }
    }
}