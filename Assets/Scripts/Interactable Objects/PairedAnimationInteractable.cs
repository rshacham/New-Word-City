using Avrahamy;
using BitStrap;
using UnityEngine;

namespace Interactable_Objects
{
    class PairedAnimationInteractable : EventInteractable
    {
        #region Inspector

        [Header("Paired Animation Interactable")]
        [SerializeField]
        [Nullable]
        [Tooltip("Required Interactable")]
        private InteractableObject other;

        [Space]
        [SerializeField]
        private TriggerAnimationParameter interactTrigger;

        #endregion

        #region Private Fields

        private Animator _myAnimator;

        #endregion

        #region EventInteractable

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

        protected override void ScriptInteract()
        {
            interactTrigger.Set(_myAnimator);
        }

        #endregion

        #region Callbacks

        private void OnOtherInteractionEnd(object sender, InteractableObject e)
        {
            CanInteract = true;
        }

        #endregion
    }
}