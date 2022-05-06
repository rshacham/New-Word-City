using UnityEngine;

namespace Mechanics.Object_Interactions.InteractionScripts
{
    /// <summary>
    /// InteractableObject that sticks itself to the user.
    /// </summary>
    /// TODO: dont allow to remove as the attached object if stuck to player?
    [RequireComponent(typeof(Rigidbody2D))]
    public class PickAndMoveInteractable : EventInteractable
    {
        #region Private Fields

        private Rigidbody2D _rigidbody2D;

        private bool _connected;

        #endregion

        #region MonoBehaviour

        protected override void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            base.Awake();
        }

        #endregion

        #region EventInteractable

        /// <summary>
        /// The script that will run on Interact with this object, before the events.
        /// </summary>
        protected override void ScriptInteract()
        {
            if (_connected)
            {
                _rigidbody2D.transform.parent = null;
                _connected = false;
                _rigidbody2D.useFullKinematicContacts = true;
                return;
            }

            _rigidbody2D.useFullKinematicContacts = false;
            _rigidbody2D.transform.parent = Player.GetComponent<Rigidbody2D>().transform;
            _connected = true;
        }
    }

    #endregion
}