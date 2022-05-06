using System;
using Mechanics.Cursor_Change;
using UnityEngine;

namespace Mechanics.Object_Interactions.InteractionScripts
{
    /// <summary>
    /// GameObject that can have an interactions
    /// Note: should probably inherit from EventInteractable, it has more robust features set.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class InteractableObject : MonoBehaviour
    {
        #region Public Properties

        /// <summary>
        /// delegate that represents interaction strategy, to allow various wat to set strategy.
        /// </summary>
        public delegate bool InteractStrategy();

        /// <summary>
        /// This event is called after Interaction is completed. used to notify observers that the
        /// interaction occured.
        /// return value is ignored.
        /// </summary>
        public event InteractStrategy OnInteractionEnd;
        //TODO: The event shouldn't be InteractStrategy, or if it is, the return value should be checked somehow?
        // TODO: change this to EventHandler?

        /// <summary>
        /// The strategy that the object uses when interacted with
        /// </summary>
        public InteractStrategy Strategy { get; set; } = () => true;

        /// <summary>
        /// Currently in held by user?
        /// </summary>
        public bool InContact { get; set; }

        /// <summary>
        /// The user currently holding this object
        /// </summary>
        public PlayerInteract Player { get; set; }

        /// <summary>
        /// Can this object be used for interactions?
        /// </summary>
        public bool CanInteract { get; set; } =
            true; // TODO: when set as false, inform player, if possible.

        #endregion

        #region Public Methods

        /// <summary>
        /// Set other as the user that holds this object, if possible.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>true if the object was attached successfully to other</returns>
        public virtual bool SetInteraction(PlayerInteract other)
        {
#if UNITY_EDITOR
            var transform1 = transform;
            var transformPosition = transform1.position;
            var transformUp = transform1.up;
            var transformRight = transform1.right;
            Debug.DrawLine(transformPosition + transformUp,
                transformPosition - transformUp,
                CanInteract ? Color.green : Color.red,
                1f);
            Debug.DrawLine(transformPosition + transformRight,
                transformPosition - transformRight,
                CanInteract ? Color.green : Color.red,
                1f);
#endif
            if (!CanInteract)
            {
                return false;
            }


            Player = other;
            InContact = true;
            return true;
        }

        /// <summary>
        /// un-attach this object from other.
        /// </summary>
        /// <param name="other"></param>
        public virtual void RemoveInteraction(PlayerInteract other)
        {
            Player = null;
            InContact = false;
        }

        /// <summary>
        /// Interact with this object.
        /// </summary>
        /// <returns></returns>
        public bool Interact()
        {
            var result = CanInteract && Strategy();
            if (result)
            {
                OnInteractionEnd?.Invoke();
            }

            return result;
        }

        #endregion
    }
}