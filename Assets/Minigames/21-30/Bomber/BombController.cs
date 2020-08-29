using System;
using System.Threading.Tasks;
using Components;
using UnityEngine;

namespace Minigames.Bomber
{
    public class BombController : AddMinigameManager
    {
        public GameObject BombPrefab;
        public float DropCooldown = 1f;
        public float Force = 5f;
        
        private Rigidbody2D bombRigidBody;
        private Vector2 spawnPostion;
        private bool canBomb;

        private void Start()
        {
            this.MinigameManager.ButtonEvents.OnActionButtonPressed += this.dropTheBomb;
            this.MinigameManager.Events.OnHit += this.HandleHit;
            this.MinigameManager.Events.OnScored += this.HandleScored;
            
            this.bombRigidBody = this.BombPrefab.GetComponent<Rigidbody2D>();
            this.spawnPostion = this.transform.position;
            this.bombRigidBody.simulated = false;
            this.canBomb = true;
        }

        private void OnDisable()
        {
            this.MinigameManager.ButtonEvents.OnActionButtonPressed -= this.dropTheBomb;
            this.MinigameManager.Events.OnHit -= this.HandleHit;
            this.MinigameManager.Events.OnScored -= this.HandleScored;
        }

        private void HandleHit()
        {
            this.resetBomb();
        }

        private void HandleScored(int obj)
        {
            this.resetBomb();
        }

        private void dropTheBomb()
        {

            if (this.MinigameManager.GameOver || !this.canBomb)
            {
                return;
            }

            this.canBomb = false;
            
            this.bombRigidBody.velocity = Vector2.zero;
            this.BombPrefab.SetActive(true);
            this.BombPrefab.transform.position = this.spawnPostion;
            this.bombRigidBody.simulated = true;
            this.bombRigidBody.AddForce(Vector2.down * this.Force, ForceMode2D.Impulse);
        }

        private async void resetBomb()
        {
            await Task.Delay(TimeSpan.FromSeconds(this.DropCooldown));

            this.BombPrefab.SetActive(true);
            this.BombPrefab.transform.position = this.spawnPostion;
            this.bombRigidBody.simulated = false;
            this.canBomb = true;
        }
    }
}