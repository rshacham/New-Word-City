using Player_Control;
using UnityEngine;
using UnityEngine.Events;

namespace Interactable_Objects
{
    /// <summary>
    /// Base class for InteractableObjects that enable setting up interactions using events, or script.
    /// </summary>
    public class EventInteractable : InteractableObject
    {
        #region Inspector
        //TODO: add field for "used by composite", if we need something like that

        [Space(2)]
        [Header("Basic Behaviour")]
        [SerializeField]
        [Tooltip("Start this object in interactable state")]
        private bool startInteractable = true;

        [SerializeField]
        [Tooltip("Allow the object to be interacted with multiple times")]
        private bool interactMultipleTimes = true;

        [SerializeField]
        [Tooltip("One click to highlight and interact")]
        private bool highlightInteract = true;

        [Header("Interaction Events")]
        [SerializeField]
        [Tooltip("Will be called after scripted interaction")]
        private UnityEvent<InteractableObject> onInteraction = new UnityEvent<InteractableObject>();

        [SerializeField]
        [Tooltip("Will be called when player first highlights this object")]
        private UnityEvent<InteractableObject> onHighlight = new UnityEvent<InteractableObject>();

        [SerializeField]
        [Tooltip("Will be called when player un-selects this object")]
        private UnityEvent<InteractableObject> onHighlightEnd = new UnityEvent<InteractableObject>();

        [Space]
        [SerializeField]
        [Tooltip("Use the script or just the events. if not inherited - should set to false")]
        private bool useScriptedInteraction = true;

        [SerializeField]
        [Tooltip("the return value of the Interact")]
        // TODO: Notice this does nor work with interactions that have words at the moment!
        private bool stayHighlightAfterInteract = true;

        #endregion

        #region MonoBehaviour

        protected override void Awake()
        {
            base.Awake();
            CanInteract = startInteractable;
            Strategy = () =>
            {
                ScriptInteract();
                onInteraction.Invoke(this);
                CanInteract = interactMultipleTimes;
                return stayHighlightAfterInteract;
            };
        }

        #endregion

        #region InteractableObject

        /// <summary>
        /// Set other as the user that holds this object, if possible.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>true if the object was attached successfully to other</returns>
        public override bool SetInteraction(PlayerInteract other)
        {
            var ret = base.SetInteraction(other);
            if (ret && highlightInteract)
            {
                ret = Interact();
            }

            onHighlight.Invoke(this);
            return ret;
        }

        /// <summary>
        /// un-attach this object from other.
        /// </summary>
        /// <param name="other"></param> // TODO: set return value?
        public override void RemoveInteraction(PlayerInteract other)
        {
            onHighlightEnd.Invoke(this);
            base.RemoveInteraction(other);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// The script that will run on Interact with this object, before the events.
        /// </summary>
        protected virtual void ScriptInteract()
        {
            if (!useScriptedInteraction)
            {
                return;
            }

            var spriteRenderer = GetComponent<SpriteRenderer>();
            var playerRenderer = Player.GetComponent<SpriteRenderer>();
            (playerRenderer.color, spriteRenderer.color) = (spriteRenderer.color, playerRenderer.color);
        }

        #endregion
    }
}