using UnityEngine;

namespace Minigames.Dungeon.States
{
    public class SkeletonThrow : StateMachineBehaviour
    {
        public GameObject SwordPrefab;
        private EnemyController enemyController;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            this.enemyController = animator.GetComponentInParent<EnemyController>();
            
            var sword = Instantiate(
                this.SwordPrefab, 
                animator.transform.position, 
                Quaternion.identity,
                animator.transform);
            
            sword.GetComponent<Rigidbody2D>().AddForce(
                Vector2.right * 
                Random.Range(enemyController.ThrowSpeedMinMax.x, enemyController.ThrowSpeedMinMax.y),
                ForceMode2D.Impulse);
            
            enemyController.Sounds.SoundSkeletonAttack.Play();
        }
        
    }
}