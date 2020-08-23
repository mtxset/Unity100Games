using System;
using System.Threading.Tasks;
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
            this.gameManager = this.GetComponentInParent<MinigameManager>();
            this.initialPosition = this.transform.position;

            this.gameManager.ButtonEvents.OnActionButtonPressed += HandleAction;
        }

        private void OnDisable()
        {
            this.gameManager.ButtonEvents.OnActionButtonPressed -= HandleAction;
        }

        private void HandleAction()
        {
            if (this.gameManager.GameOver || !this.gameManager.AllowHand)
            {
                return;
            }
            
            this.shoveHandAsync();
            this.gameManager.AllowHand = false;
        }

        private async void shoveHandAsync()
        {
            this.transform.position = this.ShoveHandPosition.position;

            await Task.Delay(TimeSpan.FromSeconds(HoldHandForSeconds));

            if (this.gameManager.GameOver)
            {
                return;
            }

            this.transform.position = this.initialPosition;
        }
    }
}