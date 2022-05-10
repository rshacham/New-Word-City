using Mechanics.Object_Interactions.InteractionScripts;
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

        // protected override void Awake()
        // {
        //     // OnInteractionEnd += OnMoveEnd;
        //     base.Awake();
        // }

        // TODO: define the animation on the player when he is a child of the taxi! then use the animation on this
        //  object instead of on the player!
        protected override void ScriptInteract()
        {
            Debug.Log("<color=yellow>Taxi Enter</color>", this);
            var playerAnimator = Player.GetComponent<Animator>();
            playerAnimator.SetTrigger(enterAnimationTrigger);
            Player.transform.parent = transform;
        }

        public bool OnMoveEnd()
        {
            var playerAnimator = Player.GetComponent<Animator>();
            playerAnimator.SetTrigger(exitAnimationTrigger);
            Debug.Log("<color=yellow>Taxi Exit</color>", this);
            Player.transform.parent = null;
            return true;
        }
    }   
}