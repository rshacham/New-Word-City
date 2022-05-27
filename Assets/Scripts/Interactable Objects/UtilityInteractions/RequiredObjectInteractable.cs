using UnityEngine;

namespace Interactable_Objects
{
    /// <summary>
    /// Implementation of PairedInteractable for enablers -
    /// require another object to trigger this enabler to activate the sub.
    /// </summary>
    // TODO: UNUSED - can be moved to mechanics
    class RequiredObjectInteractable : PairedInteractable
    {
        [Header("Required Interactable")]
        [SerializeField]
        private InteractableObject required;

        protected Collider2D _collider2D;

        // private bool _inContactWithRequired;

        protected override void Awake()
        {
            _collider2D = GetComponent<Collider2D>();
            // _collider2D set trigger?
            enabler = true;
            base.Awake();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject == required.gameObject)
            {
                Debug.Log($"<color=yellow>Required Pair</color> Trigger Enter");
                // _inContactWithRequired = true;
                Interact();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject == required.gameObject)
            {
                Debug.Log("<color=yellow>Required Pair</color> Trigger Exit");
                // _inContactWithRequired = false;
                Interact();
            }
        }
    }
}