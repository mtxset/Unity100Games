using UnityEngine;

namespace Minigames.Dungeon.States
{
    public class MageFireball : StateMachineBehaviour
    {
        public GameObject FireBallPrefab;
        private EnemyController enemyController;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            enemyController = animator.GetComponentInParent<EnemyController>();
            
            var fireBall = Instantiate(
                FireBallPrefab, 
                enemyController.FireBallSpawnPoint.position,
                FireBallPrefab.transform.rotation,
                animator.transform);
            
            fireBall.GetComponent<Rigidbody2D>().AddForce(
                Vector2.down * 
                Random.Range(enemyController.ThrowSpeedMinMax.x, enemyController.ThrowSpeedMinMax.y),
                ForceMode2D.Impulse);
        }
    }
}