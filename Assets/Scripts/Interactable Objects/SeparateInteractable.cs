using Avrahamy;
using UnityEngine;

namespace Interactable_Objects
{
    // TODO: UNUSED - can be moved to mechanics
    class SeparateInteractable : EventInteractable
    {
        [Space(2)]
        [Header("Separated")]
        [SerializeField]
        private InteractableObject from;

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Interactable") && other.gameObject == from.gameObject)
            {
                DebugLog.Log("Separated from", Color.white, from);
                Interact();
            }
        }
    }
}