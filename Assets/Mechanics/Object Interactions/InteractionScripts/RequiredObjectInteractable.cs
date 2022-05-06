using System;
using UnityEngine;

namespace Mechanics.Object_Interactions.InteractionScripts
{
    class RequiredObjectInteractable : PairedInteractable
    {
        [Header("Required Interactable")]
        [SerializeField]
        private InteractableObject required;
        
        protected Collider2D _collider2D;

        private bool _inContactWithRequired;

        protected override void Awake()
        {
            _collider2D = GetComponent<Collider2D>();
            // _collider2D.enabled = false;
            enabler = true;
            base.Awake();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            Debug.Log($"<color=yellow>Required Pair</color> Trigger Enter", col);
            if (col.gameObject == required.gameObject)
            {
                _inContactWithRequired = true;
                Interact();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Debug.Log("<color=yellow>Required Pair</color> Trigger Exit");
            if (other.gameObject == required.gameObject)
            {
                _inContactWithRequired = false;
                Interact();
            }
        }
    }
}