using Player_Control;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mechanics.Object_Moves_Player
{
    public class PlayerTaxiState : StateMachineBehaviour
    {
        private PlayerInteract _player;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            _player = animator.GetComponent<PlayerInteract>();
            _player.IsActive = false;
            var m = _player.GetComponent<Movement>();
            m.EnableMovement = false;
            m.DesiredVelocity = Vector2.zero;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            _player.GetComponent<SpriteRenderer>().enabled = false;
            _player.transform.parent = _player.CurrentActive.transform;
            _player.CurrentActive.Interact();
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