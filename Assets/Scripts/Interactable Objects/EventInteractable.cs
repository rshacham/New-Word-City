using System;
using Player_Control;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Interactable_Objects
{
    /// <summary>
    /// Base class for InteractableObjects that enable setting up interactions using events, or script.
    /// </summary>
    public class EventInteractable : InteractableObject
    {
        #region Inspector

        [Header("Event Behaviour")]
        [SerializeField]
        [FormerlySerializedAs("onInteraction")]
        [FormerlySerializedAs("onHighlight")]
        [FormerlySerializedAs("onHighlightEnd")]
        [Tooltip("Events to be called in different parts of the interactions")]
        protected InteractionEvents interactionEvents;

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
                interactionEvents.onInteraction.Invoke(this);
                CanInteract = interactMultipleTimes;
                return stayHighlightAfterInteract;
            };
            OnInteractionEnd += (sender, interactable) =>
                interactionEvents.onHighlightEnd.Invoke(interactable);
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
            if (ret && HighlightInteract)
            {
                ret = Interact();
            }

            interactionEvents.onHighlight.Invoke(this);
            return ret;
        }

        /// <summary>
        /// un-attach this object from other.
        /// </summary>
        /// <param name="other"></param> // TODO: set return value?
        public override void RemoveInteraction(PlayerInteract other)
        {
            interactionEvents.onHighlightEnd.Invoke(this);
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

    [Serializable]
    public struct InteractionEvents
    {
        [SerializeField]
        [Tooltip("Will be called during interaction - after scripted interaction")]
        public UnityEvent<InteractableObject> onInteraction;

        [SerializeField]
        [Tooltip("Will Be Called after  - if UseOnEnd is set to true (default)")]
        public UnityEvent<InteractableObject> onInteractionEnd;

        [SerializeField]
        [Tooltip("Will be called when player first highlights this object")]
        public UnityEvent<InteractableObject> onHighlight;

        [SerializeField]
        [Tooltip("Will be called when player un-selects this object")]
        public UnityEvent<InteractableObject> onHighlightEnd;
    }
}