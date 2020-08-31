using Components;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Minigames.Dungeon
{
    public class EnemyController : MonoBehaviour
    {
        public GameObject[] EnemyLifes;
        public Text InfoText;
        public Transform FireBallSpawnPoint;
        public Transform[] MeleeSpawnPoints;
        public Transform[] RangedSpawnPoints;
        
        public Vector2 PreAttackMinMax;
        public Vector2 AttackMinMax;

        public Vector2 ThrowSpeedMinMax;

        public GameObject SpawnEffect;
        public GameObject[] Enemies;
        public Sounds Sounds;
        
        private GameObject currentEnemy;
        private Lifes enemyLifes;
        private MinigameManager gameManager;

        private void Start()
        {
            this.gameManager = this.GetComponentInParent<MinigameManager>();
            this.enemyLifes = new Lifes(this.EnemyLifes);
            this.currentEnemy = this.spawnRandomEnemy();
            
            this.gameManager.Events.OnDodged += HandleDodged;
        }

        private void HandleDodged()
        {
            this.gameManager.Events.EventScored();
            // All lives are lost
            if (this.enemyLifes.LoseLife())
            {
                Destroy(currentEnemy);
                this.currentEnemy = this.spawnRandomEnemy();
                this.enemyLifes.ResetLifes();
            }
        }

        private void changeInfo(int randomEnemyIndex)
        {
            var message = "";
            switch (randomEnemyIndex)
            {
                case 0: // Knight
                    message = "Press RIGHT to shield yourself";
                    break;
                case 1: // Mage
                    message = "Press LEFT to roll";
                    break;
                case 2: // Skeleton
                    message = "Press UP to jump over";
                    break;
            }

            this.InfoText.text = message;
        }
        
        private GameObject spawnRandomEnemy()
        {
            Sounds.SoundEnemySpawn.Play();
            var randomEnemyIndex = Random.Range(0, this.Enemies.Length);

            this.changeInfo(randomEnemyIndex);

            var randomEnemy = Instantiate(
                this.Enemies[randomEnemyIndex], this.transform);
            
            int randomEnemySpawnPointIndex;

            // Setting position
            if (randomEnemy.GetComponent<EnemyType>().SelectEnemyType == 
                EnemyType.EnemyTypeEnum.Melee)
            {
                randomEnemySpawnPointIndex = Random.Range(0, this.MeleeSpawnPoints.Length);
                randomEnemy.transform.position = 
                    this.MeleeSpawnPoints[randomEnemySpawnPointIndex].position;
            }
            else
            {
                randomEnemySpawnPointIndex = Random.Range(0, this.RangedSpawnPoints.Length);
                randomEnemy.transform.position = 
                    this.RangedSpawnPoints[randomEnemySpawnPointIndex].position;
            }
            
            
            // Teleport effect
            Destroy(Instantiate(
                this.SpawnEffect, 
                randomEnemy.transform.position, 
                Quaternion.identity,
                this.transform), 1.1f);

            return randomEnemy;
        }
    }
}