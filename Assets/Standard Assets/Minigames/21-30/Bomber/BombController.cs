using System.Collections;
using Components;
using UnityEngine;

namespace Minigames.Bomber
{
    public class BombController : AddMinigameManager2
    {
        public GameObject BombPrefab;
        public float DropCooldown = 1f;
        public float Force = 5f;
        
        private Rigidbody2D bombRigidBody;
        private Vector2 spawnPostion;
        private bool canBomb;

        private void Start()
        {
            MinigameManager.ButtonEvents.OnActionButtonPressed += dropTheBomb;
            MinigameManager.Events.OnHit += HandleHit;
            MinigameManager.Events.OnScored += HandleScored;
            
            bombRigidBody = BombPrefab.GetComponent<Rigidbody2D>();
            spawnPostion = transform.position;
            bombRigidBody.simulated = false;
            canBomb = true;
        }

        private void OnDisable()
        {
            MinigameManager.ButtonEvents.OnActionButtonPressed -= dropTheBomb;
            MinigameManager.Events.OnHit -= HandleHit;
            MinigameManager.Events.OnScored -= HandleScored;
        }

        private void HandleHit()
        {
            StartCoroutine(resetBomb());
        }

        private void HandleScored(int obj)
        {
            StartCoroutine(resetBomb());
        }

        private void dropTheBomb()
        {

            if (MinigameManager.GameOver || !canBomb)
            {
                return;
            }

            canBomb = false;
            
            bombRigidBody.velocity = Vector2.zero;
            BombPrefab.SetActive(true);
            BombPrefab.transform.position = spawnPostion;
            bombRigidBody.simulated = true;
            bombRigidBody.AddForce(Vector2.down * Force, ForceMode2D.Impulse);
        }

        private IEnumerator resetBomb()
        {
            yield return new WaitForSeconds(DropCooldown);

            BombPrefab.SetActive(true);
            BombPrefab.transform.position = spawnPostion;
            bombRigidBody.simulated = false;
            canBomb = true;
        }
    }
}