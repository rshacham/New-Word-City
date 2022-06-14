using System;
using Avrahamy;
using Interactable_Objects;
using Interactable_Objects.Utilities;
using Managers;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

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
        private bool highlightOnCollision;

        [SerializeField]
        [Tooltip("Do proximity triggers highlight objects? Use with caution together with highlightOnCollision")]
        private bool highlightOnProximity = true;

        [SerializeField]
        [Tooltip("Start with the ability to interact?")]
        private bool startActive;

        [SerializeField]
        [Space]
        [Tooltip("Events that are called on interaction")]
        [InspectorName("Events On Interactions")]
        public PlayerInteractEvents interactionEvents;

        #endregion

        #region Public Properties

        // TODO: to ve used with cursor if required
        // public float ClickDistance => clickDistance;


        public bool IsActive
        {
            get => startActive;
            set => startActive = value;
        }

        /// <summary>
        /// Current object that the user is attached to
        /// </summary>
        public InteractableObject CurrentActive => _currentActive;

        #endregion

        #region Private Fields

        private Rigidbody2D _rigidbody2D;

        /// <summary>
        /// Current object that the user is attached to
        /// </summary>
        private InteractableObject _currentActive;

        private bool _firstInteraction = true;
        private GameObject _tutorialSprite;
        private bool _getInteraction;

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            GetComponent<Collider2D>();
            StaticEventsGameManager.PlayerShouldInteract += (sender, b) => IsActive = b;
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

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!_getInteraction || !highlightOnProximity || !IsActive)
            {
                return;
            }
            
            if (other.gameObject.CompareTag("InteractableHighlighter"))
            {
                var parentCollider = other.GetComponent<ProximityHighlighter>().ParentCollider;
                CollisionHighlighter(parentCollider);
            }
        }

        // TODO: move to private methods region
        private void CollisionHighlighter(Collider2D col)
        {
            if (!IsActive) // _currentActive != null || 
            {
                return;
            }

            var interactable = col.gameObject.GetComponentInParent<InteractableObject>();
            if (interactable == _currentActive)
            {
                return;
            }
            if (interactable.SetInteraction(this))
            {
                if (_currentActive != null)
                {
                    UnHighlighterFromObject(_currentActive);
                }

                // TODO: highlight interact objects should be ignored! require moving the property to base class
                if (_firstInteraction && !interactable.HighlightInteract)
                {
                    ShowInteractionKey(interactable);
                }

                _currentActive = interactable;
                _getInteraction = false;
            }
            // else  // TODO: add this functionality - require testing existing objects.
            // {
            //     interactable.RemoveInteraction(this);
            // }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!highlightOnProximity || !IsActive)
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
            if (!highlightOnCollision || !IsActive)
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
            UnHighlighterFromObject(interactable);
        }

        private void UnHighlighterFromObject(InteractableObject interactable)
        {
            if (interactable == _currentActive)
            {
                if (_firstInteraction && !interactable.HighlightInteract)
                {
                    UnShowInteractionKey(_currentActive);
                }

                // DebugLog.Log("<color=cyan>UnHighlight</color>", interactable);
                _currentActive.RemoveInteraction(this);
                _currentActive = null;
                _getInteraction = true;
            }
        }

        #endregion

        #region Tutorial

        private void ShowInteractionKey(InteractableObject obj)
        {
            var pos = obj.transform.position + Tutorial.Offset;
            _tutorialSprite = Tutorial.CreateTutorial(pos, TutorialScheme.Tutorials.Interact);
        }

        private void UnShowInteractionKey(InteractableObject obj)
        {
            Tutorial.RemoveTutorial(_tutorialSprite);
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

            if (_firstInteraction && !_currentActive.HighlightInteract) // TODO:
            {
                _firstInteraction = false;
                UnShowInteractionKey(_currentActive);
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

                DebugLog.Log(
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
    /// Hold events for the player
    /// </summary>
    [Serializable]
    public struct PlayerInteractEvents
    {
        public UnityEvent<InteractableObject> onInteractableObject;

        public UnityEvent onEmptyInteract;
    }
}