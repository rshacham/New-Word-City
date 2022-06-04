using System.Runtime.CompilerServices;
using Interactable_Objects;
using Managers;
using Player_Control;
using UnityEngine;


namespace StateMachine
{
    public class OnClimbEnter : StateMachineBehaviour
    {
        private PlayerInteract _player;
        private InteractableObject _currentActive;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            Debug.Log("hey");
            _player = animator.GetComponent<PlayerInteract>();
            _currentActive = _player.CurrentActive;
            _player.IsActive = false;
            var m = _player.GetComponent<Movement>();
            m.EnableMovement = false;
            m.DesiredVelocity = Vector2.zero;
            StaticEventsGameManager.OnPlayerShouldCollide(_currentActive, false);
        }
        
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            _player.GetComponent<Movement>().EnableMovement = true;
            _player.transform.parent = null;
            _player.IsActive = true;
            StaticEventsGameManager.OnPlayerShouldCollide(_currentActive, true);
        }

    }
    
    
    

}

