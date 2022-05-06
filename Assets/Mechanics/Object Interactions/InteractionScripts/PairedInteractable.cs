using System;
using UnityEngine;

namespace Mechanics.Object_Interactions.InteractionScripts
{
    class PairedInteractable : EventInteractable
    {
        public enum PairType
        {
            Trigger,
            Switch
        }

        #region Inspector

        [Header("PairedInteractable")]
        [SerializeField]
        protected PairedInteractable otherPaired;

        [Space]
        [SerializeField]
        protected bool enabler = true;

        [SerializeField]
        protected PairType enablerType = PairType.Switch;

        #endregion

        #region Private Properties

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
                    otherPaired.CanInteract = false;
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