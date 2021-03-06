using Interactable_Objects;
using Managers;
using Player_Control;
using UnityEngine;

namespace StateMachine
{
    public class PlayerTaxiState : StateMachineBehaviour
    {
        private PlayerInteract _player;
        private InteractableObject _currentActive;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            _player = animator.GetComponent<PlayerInteract>();
            _currentActive = _player.CurrentActive;
            _player.IsActive = false;
            var m = _player.GetComponent<Movement>();
            m.EnableMovement = false;
            m.DesiredVelocity = Vector2.zero;
            StaticEventsGameManager.OnPlayerShouldCollide(_currentActive, false);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _player.transform.parent = _currentActive.transform;
            _player.GetComponent<SpriteRenderer>().enabled = false; // TODO:?
            _currentActive.Interact();
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