using UnityEngine;
using UnityEngine.InputSystem;

namespace Mechanics.Object_Interactions.InteractionScripts
{
    /// <summary>
    /// Class that handles user interactions with objects
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class PlayerInteract : MonoBehaviour
    {
        #region Private Fields

        private Collider2D _collider2D;
        /// <summary>
        /// Current object that the user is attached to
        /// </summary>
        private AbstractInteractableObject _currentActive;

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            _collider2D = GetComponent<Collider2D>();
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.collider.CompareTag("Interactable"))
            {
                if (_currentActive != null)
                {
                    return;
                }

                var interactable = col.gameObject.GetComponent<AbstractInteractableObject>();
                if (interactable.SetInteraction(this))
                {
                    _currentActive = interactable;
                }
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.collider.CompareTag("Interactable"))
            {
                var interactable = other.gameObject.GetComponent<AbstractInteractableObject>();
                if (interactable == _currentActive)
                {
                    _currentActive.RemoveInteraction(this);
                    _currentActive = null;
                }
            }
        }

        #endregion

        #region Input Callbacks

        /// <summary>
        /// Keyboard Interact
        /// </summary>
        /// <param name="context"></param>
        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.started && _currentActive != null)
            {
                _currentActive.Interact();
            }
        }

        /// <summary>
        /// Mouseclick Interact - from far away
        /// </summary>
        /// <param name="context"></param>
        public void OnMouseInteract(InputAction.CallbackContext context)
        {
            if (!context.started)
            {
                return;
            }

            Vector2 mousePos = Camera.main!.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            var hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit && hit.collider.CompareTag("Interactable"))
            {
                ClickedInteractable(hit);
            }
            else if (_currentActive != null)
            {
                _currentActive.RemoveInteraction(this);
                _currentActive = null;
            }
        }

        /// <summary>
        /// Handle clicking interactable
        /// </summary>
        /// <param name="hit"></param>
        private void ClickedInteractable(RaycastHit2D hit)
        {
            var interactable = hit.collider.GetComponent<AbstractInteractableObject>();

            if (_currentActive == interactable)
            {
                _currentActive.Interact();
            }
            else if (interactable.SetInteraction(this))
            {
                if (_currentActive != null)
                {
                    _currentActive.RemoveInteraction(this);
                }

                _currentActive = interactable;
                _currentActive.Interact();
            }
        }

        #endregion
    }
}