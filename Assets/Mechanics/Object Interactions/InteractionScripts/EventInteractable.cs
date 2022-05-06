using UnityEngine;
using UnityEngine.Events;

namespace Mechanics.Object_Interactions.InteractionScripts
{
    /// <summary>
    /// Base class for InteractableObjects that enable setting up interactions using events, or script.
    /// </summary>
    public class EventInteractable : InteractableObject
    {
        #region Inspector

        [Header("Basic Events")]
        [SerializeField]
        private bool startInteractable = true;

        [SerializeField]
        private bool interactMultipleTimes = true;
        
        [Header("Interaction Events")]
        [SerializeField]
        private UnityEvent onInteraction = new UnityEvent();

        [SerializeField]
        private UnityEvent<InteractableObject> onHighlight =
            new UnityEvent<InteractableObject>();

        [SerializeField]
        private UnityEvent<InteractableObject> onHighlightEnd =
            new UnityEvent<InteractableObject>();

        [Space]
        [SerializeField]
        [Tooltip("Use the script or just the events. if not inherited - should set to false")]
        private bool useDebugScriptInteract = true;

        #endregion

        #region MonoBehaviour

        protected virtual void Awake()
        {
            CanInteract = startInteractable;
            Strategy = () =>
            {
                ScriptInteract();
                onInteraction.Invoke();
                CanInteract = interactMultipleTimes;
                return true;
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
            onHighlight.Invoke(this);
            return base.SetInteraction(other);
        }

        /// <summary>
        /// un-attach this object from other.
        /// </summary>
        /// <param name="other"></param>
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
            if (!useDebugScriptInteract)
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