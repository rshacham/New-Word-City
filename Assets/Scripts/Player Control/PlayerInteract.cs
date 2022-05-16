﻿using System;
using Interactable_Objects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Player_Control
{
    /// <summary>
    /// Class that handles user interactions with objects
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerInteract : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        [HideInInspector]
        [Tooltip("Distance in with clicks are accepted")]
        [Min(0)]
        private float clickDistance = 3f;

        [SerializeField]
        [Tooltip("Are collisions with interactable objects count as setting them active?")]
        private bool highlightOnCollision = false;

        [SerializeField]
        [Tooltip(
            "Do proximity triggers highlight objects? Use with caution together with highlightOnCollision")]
        private bool highlightOnProximity = true;

        [SerializeField]
        [Space]
        [Tooltip("Events that are called on interaction")]
        [InspectorName("Events On Interactions")]
        public PlayerInteractEvents interactionEvents = new PlayerInteractEvents();

        #endregion

        #region Public Properties

        // TODO: to ve used with cursor if required
        // public float ClickDistance => clickDistance;

        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Current object that the user is attached to
        /// </summary>
        public InteractableObject CurrentActive => _currentActive;

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

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!highlightOnProximity || !IsActive)
            {
                return;
            }

            if (col.gameObject.CompareTag("InteractableHighlighter"))
            {
                var parentCollider = col.GetComponent<ProximityHighlighter>().ParentCollider;
                CollisionHighlighter(parentCollider);
            }
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (!highlightOnCollision || !IsActive)
            {
                return;
            }

            if (col.gameObject.CompareTag("Interactable"))
            {
                CollisionHighlighter(col.collider);
            }
        }

        // TODO: move to private methods region
        private void CollisionHighlighter(Collider2D col)
        {
            if (_currentActive != null || !IsActive)
            {
                return;
            }

            var interactable = col.gameObject.GetComponentInParent<InteractableObject>();
            if (interactable.SetInteraction(this))
            {
                _currentActive = interactable;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!highlightOnProximity)
            {
                return;
            }

            if (other.gameObject.CompareTag("InteractableHighlighter"))
            {
                var parentCollider = other.GetComponent<ProximityHighlighter>().ParentCollider;
                CollisionUnHighlighter(parentCollider);
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
                CollisionUnHighlighter(other.collider);
            }
        }

        // TODO: move to private methods region
        private void CollisionUnHighlighter(Collider2D other)
        {
            var interactable = other.gameObject.GetComponentInParent<InteractableObject>();
            if (interactable == _currentActive)
            {
                //TODO: Duplicated
                // Debug.Log("<color=cyan>UnHighlight</color>", other);
                _currentActive.RemoveInteraction(this);
                _currentActive = null;
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
            if (!context.started || !IsActive)
            {
                return;
            }

            if (_currentActive == null)
            {
                interactionEvents.onEmptyInteract.Invoke();
                return;
            }

            if (!_currentActive.Interact())
            {
                _currentActive.RemoveInteraction(this);
                _currentActive = null;
            }
            else
            {
                interactionEvents.onInteractableObject.Invoke(_currentActive);
            }
        }

        /// <summary>
        /// Mouseclick Interact - from far away
        /// </summary>
        /// <param name="context"></param>
        public void OnMouseInteract(InputAction.CallbackContext context)
        {
            if (!context.started || !Mouse.current.leftButton.wasPressedThisFrame || !IsActive)
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
            var interactable = hit.collider.GetComponentInParent<InteractableObject>();

            if (_currentActive == interactable)
            {
                if (!_currentActive.Interact())
                {
                    _currentActive.RemoveInteraction(this);
                    _currentActive = null;
                }
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

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class PlayerInteractEvents
    {
        public UnityEvent<InteractableObject> onInteractableObject = new UnityEvent<InteractableObject>();

        public UnityEvent onEmptyInteract = new UnityEvent();
    }
}