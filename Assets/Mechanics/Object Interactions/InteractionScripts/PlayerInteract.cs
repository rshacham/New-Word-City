using System;
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
        #region Inspector

        [SerializeField]
        private float clickDistance = 3f;

        #endregion

        #region Private Fields

        private Rigidbody2D _rigidbody2D;
        private Collider2D _collider2D;

        /// <summary>
        /// Current object that the user is attached to
        /// </summary>
        private InteractableObject _currentActive;

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
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

                var interactable = col.gameObject.GetComponent<InteractableObject>();
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
                var interactable = other.gameObject.GetComponent<InteractableObject>();
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
            Debug.DrawLine(_rigidbody2D.position, 
                mousePos,
                hit ? Color.green : Color.yellow, 
                1f);
            if (Vector2.SqrMagnitude(mousePos - _rigidbody2D.position) > clickDistance * clickDistance)
            {
                Debug.Log(
                    $"<color=yellow>Player Interaction:</color> Click too far.\nposition={mousePos}{(hit ? $"\tHit {hit.collider.name}" : "")}",
                    hit ? hit.collider.gameObject : gameObject
                );
                return;
            }

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
            var interactable = hit.collider.GetComponent<InteractableObject>();

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