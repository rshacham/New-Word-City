using System;
using Avrahamy;
using Player_Control;
using UnityEngine;

namespace Interactable_Objects
{
    /// <summary>
    /// GameObject that can have an interactions
    /// Note: should probably inherit from EventInteractable, it has more robust features set.
    /// </summary>
    // [RequireComponent(typeof(Collider2D))]
    public class InteractableObject : MonoBehaviour
    {
        #region Inspector
        [Header("Word Connection")]
        [Space]
        [SerializeField]
        private bool requiresWord;

        [SerializeField]
        [Tooltip("If in required word chain, reference to earlier object in the chain")]
        private InteractableObject lastLink;
        // TODO: add case where after word complete we want to keep the interaction possible

        [SerializeField]
        [Tooltip("This objects collider")] // TODO: get from code when possible!
        private Collider2D myCollider;

        #endregion

        #region Private Fields

        private bool _wordActive;
        private bool _canInteract = true;

        #endregion

        #region Public Properties

        public event EventHandler InformChain;

        /// <summary>
        /// delegate that represents interaction strategy, to allow various wat to set strategy.
        /// </summary>
        public delegate bool InteractStrategy();

        /// <summary>
        /// This event is called after Interaction is completed. used to notify observers that the
        /// interaction occured.
        /// return value is ignored.
        /// </summary>
        public event EventHandler<InteractableObject> OnInteractionEnd;
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
        /// 
        /// </summary>
        public Collider2D Collider => myCollider;

        /// <summary>
        /// Can this object be used for interactions?
        /// </summary>
        public bool CanInteract
        {
            get => requiresWord ? _wordActive && _canInteract : _canInteract;
            set => _canInteract = value;
        }
        // TODO: when set as false, inform player, if possible.

        public bool UseOnEnd { get; set; } = true;

        #endregion

        #region Public Methods

        /// <summary>
        /// Set other as the user that holds this object, if possible.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>true if the object was attached successfully to other</returns>
        public virtual bool SetInteraction(PlayerInteract other)
        {
            DebugDraw.DrawCross2D(transform.position, 1, CanInteract ? Color.green : Color.red, 1f);
            
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
            if (result && UseOnEnd)
            {
                OnInteractionEnd?.Invoke(this, this);
            }

            return result;
        }

        public virtual void OnInformChain()
        {
            if (!requiresWord)
            {
                return;
            }

            _wordActive = !_wordActive;
            // Debug.Log($"<color=white>{_wordActive ? "Interactable Active" : "Interactable disabled"}</color>", gameObject);
            InformChain?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region MonoBehaviour

        protected virtual void Awake()
        {
            if (requiresWord && lastLink != null)
            {
                lastLink.InformChain += (sender, args) => OnInformChain();
            }

            if (myCollider == null)
            {
                myCollider = GetComponentInChildren<Collider2D>();
            }
        }

        #endregion
    }
}