using Interactable_Objects;
using Managers;
using Player_Control;
using UnityEngine;

namespace StateMachine
{
    public class PlayerTaxiExitState : StateMachineBehaviour
    {
        private PlayerInteract _player;
        private InteractableObject _currentActive;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            _player = animator.GetComponent<PlayerInteract>();
            _player.GetComponent<SpriteRenderer>().enabled = true;
            _currentActive = _player.CurrentActive;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            _player.GetComponent<Movement>().EnableMovement = true;
            _player.transform.parent = null;
            _currentActive.Interact();
            _player.IsActive = true;
            StaticEventsGameManager.OnPlayerShouldCollide(_currentActive, true);
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