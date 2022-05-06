using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mechanics.Object_Interactions.InteractionScripts
{
    /// <summary>
    /// Class that handles user interactions with objects
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerInteract : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        [Tooltip("Distance in with clicks are accepted")]
        [Min(0)]
        private float clickDistance = 3f;

        [SerializeField]
        [Tooltip("Are collisions with interactable objects count as setting them active?")]
        private bool highlightOnCollision = true;

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
            if (!highlightOnCollision)
            {
                return;
            }
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
            if (!highlightOnCollision)
            {
                return;
            }
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
            if (!context.started || !Mouse.current.leftButton.wasPressedThisFrame)
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
                if (_currentActive != null) // TODO: duplicated
                {
                    _currentActive.RemoveInteraction(this);
                    _currentActive = null;
                }
                Debug.Log(
                    $"<color=green>Player Interaction:</color> Click too far.\nposition={mousePos}{(hit ? $"\tHit {hit.collider.name}" : "")}",
                    hit ? hit.collider.gameObject : gameObject
                );
                return;
            }

            if (hit && hit.collider.CompareTag("Interactable"))
            {
                ClickedInteractable(hit);
            }
            else if (_currentActive != null) // tODO: duplicated.
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
                // TODO: first click highlights, second click interacts?
                // _currentActive.Interact();
            }
        }

        #endregion
    }
}