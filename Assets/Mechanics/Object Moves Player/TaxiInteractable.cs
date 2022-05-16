using System;
using Interactable_Objects;
using UnityEngine;

namespace Mechanics.Object_Moves_Player
{
    public class TaxiInteractable : EventInteractable
    {
        [Space]
        [Header("Taxi Interaction")]
        [SerializeField]
        private string enterAnimationTrigger = "TaxiEnter";
        [SerializeField]
        private string exitAnimationTrigger = "TaxiExit";

        private enum TaxiState
        {
            Hold,
            Wait,
            Move,
            Stop
        }

        private TaxiState _waitForPlayer = TaxiState.Hold;

        private Animator _myAnimator;

        private static readonly int TaxiMove = Animator.StringToHash("TaxiMove");

        private static readonly int TaxiArrive = Animator.StringToHash("TaxiArrive");

        protected override void Awake()
        {
            _myAnimator = GetComponent<Animator>();
            base.Awake();
        }

        // TODO: define the animation on the player when he is a child of the taxi! then use the animation on this
        //  object instead of on the player!
        // TODO: do all of this from state behaviour machine?
        protected override void ScriptInteract()
        {
            Animator playerAnimator;
            switch (_waitForPlayer)
            {
                case TaxiState.Hold:
                    UseOnEnd = false;
                    Debug.Log("<color=yellow>Taxi Enter</color>", this);
                    _waitForPlayer = TaxiState.Wait;
                    playerAnimator = Player.GetComponent<Animator>();
                    playerAnimator.SetTrigger(enterAnimationTrigger);
                    break;
                case TaxiState.Wait:
                    Debug.Log("<color=yellow>Taxi Move</color>", this);
                    _waitForPlayer = TaxiState.Move;
                    _myAnimator.SetTrigger(TaxiMove);
                    break;
                case TaxiState.Move:
                    Debug.Log("<color=yellow>Taxi Exit</color>", this);
                    _waitForPlayer = TaxiState.Stop;
                    playerAnimator = Player.GetComponent<Animator>();
                    playerAnimator.SetTrigger(exitAnimationTrigger);
                    break;
                case TaxiState.Stop:
                    UseOnEnd = true;
                    Debug.Log("<color=yellow>Taxi Hold</color>", this);
                    _waitForPlayer = TaxiState.Hold;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}