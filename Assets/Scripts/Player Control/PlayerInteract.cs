using System;
using Avrahamy;
using Interactable_Objects;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Object = UnityEngine.Object;

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

        [Space(2)]
        [Header("Tutorial")]
        // [InspectorName("Tutorial UI Images")]
        [SerializeField]
        private TutorialObjects tutorialObjects;

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
        private bool _firstInteraction = true;
        private Movement _myMovement;

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            _myMovement = GetComponent<Movement>();
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
                // TODO: highlight interact objects should be ignored! require moving the property to base class
                if (_firstInteraction && !interactable.HighlightInteract)
                {
                    ShowInteractionKey(interactable);
                }

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
                if (_firstInteraction)
                {
                    UnShowInteractionKey(_currentActive);
                }

                //TODO: Duplicated
                // Debug.Log("<color=cyan>UnHighlight</color>", other);
                _currentActive.RemoveInteraction(this);
                _currentActive = null;
            }
        }

        #endregion

        #region Tutorial

        private void ShowInteractionKey(InteractableObject obj)
        {
            // var key = _myMovement.IsController ? "controller" : "kbm";
            // DebugLog.Log(LogTag.Gameplay, $"Show Interaction Tutorial: {key}", obj);
            var scheme = _myMovement.IsController ? TutorialObjects.Schemes.Controller : TutorialObjects.Schemes.KBM;
            var pos = obj.transform.position + tutorialObjects.Offset;
            tutorialObjects.CreateTutorial(
                obj.transform.position + Vector3.right,
                TutorialScheme.Tutorials.Interact,
                scheme
            );
        }

        private void UnShowInteractionKey(InteractableObject obj)
        {
            // var key = _myMovement.IsController ? "controller" : "kbm";
            // DebugLog.Log(LogTag.Gameplay, $"UnShow Interaction Tutorial: {key}", obj);
            tutorialObjects.RemoveTutorial();
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

            if (_firstInteraction)
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

    /// <summary>
    /// 
    /// </summary> TODO: Refactor to another class - probably static of singleton - to allow access from movements
    [Serializable]
    public class TutorialObjects
    {
        #region Inspector

        [SerializeField]
        private Vector3 offset = new Vector3(1, 1);

        [Space(2)]
        [Header("Schemes")]
        [SerializeField]
        private TutorialScheme controller;

        [SerializeField]
        private TutorialScheme kbm;

        [Space(2)]
        [SerializeField]
        private GameObject tutorialSprite;

        #endregion

        #region Private Fields

        private GameObject _tutorialInstance;

        #endregion

        #region Public Properties

        public enum Schemes
        {
            KBM,
            Controller
        }

        public Vector3 Offset
        {
            get => offset;
            set => offset = value;
        }

        #endregion

        #region Public Methods

        // TODO: add animation slight shake, custom images, pop in/popout, multi tutorials support
        public ref GameObject CreateTutorial(Vector3 position, TutorialScheme.Tutorials type, Schemes scheme)
        {
            if (_tutorialInstance == null)
            {
                _tutorialInstance = Object.Instantiate(tutorialSprite);
            }

            _tutorialInstance.transform.position = position;

            var tutorialScheme = scheme switch
            {
                Schemes.KBM => kbm,
                Schemes.Controller => controller,
                _ => throw new ArgumentOutOfRangeException(nameof(scheme), scheme, null)
            };
            _tutorialInstance.GetComponent<SpriteRenderer>().sprite = type switch
            {
                TutorialScheme.Tutorials.Interact => tutorialScheme.interact,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
            _tutorialInstance.SetActive(true);
            return ref _tutorialInstance;
        }

        public void RemoveTutorial()
        {
            _tutorialInstance.SetActive(false);
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public struct TutorialScheme
    {
        public enum Tutorials
        {
            Interact
        }

        [SerializeField]
        public Sprite interact;
    }
}