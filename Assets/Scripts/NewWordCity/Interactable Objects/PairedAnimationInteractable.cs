using Avrahamy.EditorGadgets;
using BitStrap;
using UnityEngine;

namespace Interactable_Objects
{
    public class PairedAnimationInteractable : EventInteractable
    {
        #region Inspector

        [Header("Paired Animation Interactable")]
        [SerializeField]
        [Nullable]
        [Tooltip("Required Interactable")]
        private InteractableObject other;

        [SerializeField]
        [Tooltip("Does this interactable use animation set in this script?")]
        private bool animated;

        [Space]
        [SerializeField]
        [ConditionalHide("animated")]
        private TriggerAnimationParameter interactTrigger;

        #endregion

        #region Private Fields

        private Animator _myAnimatorPaired;

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

            _myAnimatorPaired = GetComponent<Animator>();
        }

        protected override void ScriptInteract()
        {
            if (animated)
            {
                interactTrigger.Set(_myAnimatorPaired);
            }
            // DebugLog.Log("hey");
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