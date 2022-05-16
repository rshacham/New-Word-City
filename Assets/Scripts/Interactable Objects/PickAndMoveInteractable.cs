using Player_Control;
using UnityEngine;

namespace Interactable_Objects
{
    /// <summary>
    /// InteractableObject that sticks itself to the user.
    /// </summary>
    // TODO: change movement to animation/body/follow script/something else.
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

        public override void RemoveInteraction(PlayerInteract other)
        {
            ScriptInteract();
            base.RemoveInteraction(other);
        }

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
        #endregion

    }
}