using UnityEngine;

namespace Interactable_Objects
{
    /// <summary>
    /// Create dependency 2 paired object.
    /// Sub can interact only after main informs it.
    /// TODO: functionality is mostly not required because events can be used instead.
    /// </summary>
    class PairedInteractable : EventInteractable
    {
        #region Enums

        /// <summary>
        /// Type of control:
        /// </summary>
        public enum PairType
        {
            Trigger,
            Switch
        }

        #endregion

        #region Inspector

        [Header("PairedInteractable")]
        [SerializeField]
        [Tooltip("The other paired object. required.")]
        protected PairedInteractable otherPaired;

        [Space]
        [SerializeField]
        [Tooltip("Is this object the enabler?")]
        protected bool enabler = true;

        [SerializeField]
        [Tooltip("What type of enabler this object is")]
        protected PairType enablerType = PairType.Switch;

        #endregion

        #region Protected Properties

        // TODO: remove, can be swapped directly with CanInteract?
        protected bool Enabled
        {
            get => enabler || CanInteract;
            set => CanInteract = value;
        }

        #endregion

        #region MonoBehaviour

        protected override void Awake()
        {
            base.Awake();
            if (otherPaired == null)
            {
                Debug.LogError("Must have pair!");
                CanInteract = false;
            }
            else
            {
                CanInteract = enabler;
            }
        }

        private void OnValidate()
        {
            if (otherPaired != null)
            {
                otherPaired.otherPaired = this;
                if (enabler)
                {
                    otherPaired.enabler = false;
                    otherPaired.CanInteract = false; // TODO: Enabled= false, if kept
                }
            }
        }

        #endregion

        #region EventInteractable

        protected override void ScriptInteract()
        {
            if (enabler)
            {
                otherPaired.Enabled = enablerType switch
                {
                    PairType.Switch => !otherPaired.Enabled,
                    PairType.Trigger => true,
                    _ => true
                };
                return;
            }

            base.ScriptInteract();
        }

        #endregion
    }
}