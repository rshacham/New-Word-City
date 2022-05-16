using Player_Control;
using UnityEngine;

namespace StateMachine
{
    public class PlayerTaxiExitState : StateMachineBehaviour
    {
        private PlayerInteract _player;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            _player = animator.GetComponent<PlayerInteract>();
            _player.GetComponent<SpriteRenderer>().enabled = true;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            _player.GetComponent<Movement>().EnableMovement = true;
            _player.transform.parent = null;
            _player.CurrentActive.Interact();
            _player.IsActive = true;
        }
        //
        // public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo,
        //     int layerIndex)
        // {
        // }
        //
        // public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo,
        //     int layerIndex)
        // {
        // }
        //
        // public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo,
        //     int layerIndex)
        // {
        // }
    }
}