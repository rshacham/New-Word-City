using UnityEngine;

namespace Interactable_Objects
{
    public class DropFromSinkInteractable : EventInteractable
    {
        #region Private Fields

        private AudioSource _audioSource;
        private bool _interactionActive;
        private Animator _myAnimator;
        
        // TODO: use AnimatorParameter
        private static readonly int Drop = Animator.StringToHash("Drop");

        #endregion

        #region MonoBehaviour

        private void Start()
        {
            _myAnimator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
        }

        #endregion

        #region EventInteractable

        protected override void ScriptInteract()
        {
            if (!_interactionActive)
            {
                _interactionActive = true;
                _myAnimator.SetTrigger(Drop);
            }
        }

        #endregion

        #region Event Callbacks

        public void InteractionOff()
        {
            _interactionActive = false;
        }

        public void PlayDropSound()
        {
            _audioSource.PlayOneShot(_audioSource.clip);
        }

        #endregion
    }
}