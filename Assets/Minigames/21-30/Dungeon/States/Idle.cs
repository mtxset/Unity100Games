using UnityEngine;

namespace Minigames.Dungeon.States
{
    public class Idle : StateMachineBehaviour
    {
        private EnemyController enemyController;
        private float attackAfter;
        private float timer;
        private static readonly int PreAttack = Animator.StringToHash("preAttack");

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            this.timer = 0;
            this.enemyController = animator.GetComponentInParent<EnemyController>();
            
            this.attackAfter = Random.Range(
                enemyController.PreAttackMinMax.x, enemyController.PreAttackMinMax.y);
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if ((timer += Time.fixedDeltaTime) >= this.attackAfter)
            {
                animator.SetTrigger(PreAttack);
                timer = 0;
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.ResetTrigger(PreAttack);
        }

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}
    }
}
