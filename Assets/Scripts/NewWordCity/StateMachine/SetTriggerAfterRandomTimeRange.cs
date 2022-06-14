using Avrahamy;
using Avrahamy.Math;
using BitStrap;
using UnityEngine;
using FloatRange = Avrahamy.Math.FloatRange;

namespace StateMachine
{
    public class SetTriggerAfterRandomTimeRange : StateMachineBehaviour
    {
        #region Inspector

        [SerializeField]
        [Tooltip("The trigger to move into the initial animation state")]
        private TriggerAnimationParameter startTrigger;

        [SerializeField]
        [Tooltip("Time range to start in")]
        private FloatRange range;

        #endregion
    
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var delay = RandomUtils.Range(range);
            PureCoroutines.DelaySeconds(() => startTrigger.Set(animator), delay);
            // DebugLog.Log(LogTag.LowPriority, $"delay: {delay}", this);
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

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
