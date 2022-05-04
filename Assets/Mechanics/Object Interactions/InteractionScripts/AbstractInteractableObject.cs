using System;
using UnityEngine;

namespace Mechanics.Object_Interactions.InteractionScripts
{
    /// <summary>
    /// GameObject that can have an interactions
    /// </summary>
    public abstract class AbstractInteractableObject : MonoBehaviour
    {
        #region Public Properties

        public delegate bool InteractStrategy();

        /// <summary>
        /// The strategy that the object uses when interacted with
        /// </summary>
        public InteractStrategy Strategy { get; set; }

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
        public bool CanInteract { get; set; } = true;

        #endregion

        #region Public Methods

        /// <summary>
        /// Set other as the user that holds this object, if possible.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>true if the object was attached successfully to other</returns>
        public virtual bool SetInteraction(PlayerInteract other)
        {
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
            return CanInteract && Strategy();
        }

        #endregion
    }
}