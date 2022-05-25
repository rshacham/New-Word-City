using System;
using Player_Control;
using UnityEngine;

namespace Interactable_Objects
{
    /// <summary>
    /// Interactable object that is a taxi - pickup and moves player
    /// </summary>
    public class TaxiInteractable : EventInteractable
    {
        #region Inspector

        [Space]
        [Header("Taxi Interaction")]
        // TODO: not serialize - or hash at validate?
        [SerializeField]
        private string enterAnimationTrigger = "TaxiEnter";
        [SerializeField]
        private string exitAnimationTrigger = "TaxiExit";

        #endregion

        #region Private Fields

        #region Private Enums

        private enum TaxiState
        {
            Hold,
            Wait,
            Move,
            Stop
        }

        #endregion

        private TaxiState _waitForPlayer = TaxiState.Hold;

        private Animator _myAnimator;

        #region Constants

        private static readonly int TaxiMove = Animator.StringToHash("TaxiMove");

        private static readonly int TaxiArrive = Animator.StringToHash("TaxiArrive");
        private static readonly int Highlight = Animator.StringToHash("Highlight");

        #endregion

        #endregion

        #region EventInteractable

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
                    _myAnimator.ResetTrigger(Highlight);
                    _myAnimator.Play("CloseDoor");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void RemoveInteraction(PlayerInteract other)
        {
            var player = Player; // TODO: fix this
            base.RemoveInteraction(other);
            Player = player;
        }

        #endregion
    }
}