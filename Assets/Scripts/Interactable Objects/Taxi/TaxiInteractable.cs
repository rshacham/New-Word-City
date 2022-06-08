using System;
using Avrahamy;
using BitStrap;
using Managers;
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
        // [Header("Taxi Player Triggers")]
        [Header("Taxi Interaction")]
        [Space]
        [SerializeField]
        [Tooltip("The player enter taxi trigger")]
        [AnimatorField("_playerAnimator")]
        private TriggerAnimationParameter taxiEnter;

        [SerializeField]
        [AnimatorField("_playerAnimator")]
        [Tooltip("The player exit taxi trigger")]
        private TriggerAnimationParameter taxiExit;

        [Space]
        [Header("Taxi Triggers")]
        [SerializeField]
        [Tooltip("Trigger for the taxi to move")]
        private TriggerAnimationParameter taxiMove;

        [SerializeField]
        [Tooltip("Trigger to reset taxi highlight")]
        private BoolAnimationParameter taxiHighlight;

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
        private Animator _playerAnimator;
        private Animator _myAnimator;
        private CartoonHoleManager _holeManager;

        #endregion

        #region EventInteractable

        protected override void Awake()
        {
            _myAnimator = GetComponent<Animator>();
            _holeManager = FindObjectOfType<CartoonHoleManager>();
            base.Awake();
        }

        // TODO: define the animation on the player when he is a child of the taxi! then use the animation on this
        //  object instead of on the player!
        // TODO: do all of this from state behaviour machine?
        protected override void ScriptInteract()
        {
            switch (_waitForPlayer)
            {
                case TaxiState.Hold:
                    UseOnEnd = false;
                    _holeManager.ChangeMinMax(0, 70);
                    _holeManager.Moving = 1;
                    DebugLog.Log("<color=yellow>Taxi Enter</color>", this);
                    _waitForPlayer = TaxiState.Wait;
                    _playerAnimator = Player.GetComponent<Animator>();
                    // _playerAnimator.SetTrigger(enterAnimationTrigger);
                    taxiEnter.Set(_playerAnimator);
                    break;
                case TaxiState.Wait:
                    DebugLog.Log("<color=yellow>Taxi Move</color>", this);
                    _waitForPlayer = TaxiState.Move;
                    taxiMove.Set(_myAnimator);
                    break;
                case TaxiState.Move:
                    DebugLog.Log("<color=yellow>Taxi Exit</color>", this);
                    _waitForPlayer = TaxiState.Stop;
                    taxiExit.Set(_playerAnimator);
                    break;
                case TaxiState.Stop:
                    _holeManager.ChangeMinMax(0, 30);
                    UseOnEnd = true;
                    DebugLog.Log("<color=yellow>Taxi Hold</color>", this);
                    _waitForPlayer = TaxiState.Hold;
                    // _myAnimator.ResetTrigger(taxiHighlight.Index);
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
            taxiHighlight.Set(_myAnimator, false);
        }

        public override bool SetInteraction(PlayerInteract other)
        {
            taxiHighlight.Set(_myAnimator, true);
            return base.SetInteraction(other);
        }

        #endregion
    }
}