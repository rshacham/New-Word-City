using Avrahamy;
using BitStrap;
using UnityEngine;

namespace Interactable_Objects
{
    class PairedAnimationInteractable : EventInteractable
    {
        [Header("Paired Animation Interactable")]
        [SerializeField]
        [Nullable]
        [Tooltip("Required Interactable")]
        private InteractableObject other;

        [Space]
        [SerializeField]
        private TriggerAnimationParameter interactTrigger;

        private Animator _myAnimator;

        protected override void Awake()
        {
            base.Awake();
            if (other != null)
            {
                CanInteract = false;
                other.OnInteractionEnd += OnOtherInteractionEnd;
            }

            _myAnimator = GetComponent<Animator>();
        }

        private void OnOtherInteractionEnd(object sender, InteractableObject e)
        {
            CanInteract = true;
            DebugLog.Log("Got valid");
        }

        protected override void ScriptInteract()
        {
            DebugLog.Log("Interacted");
            interactTrigger.Set(_myAnimator);
        }
    }
}