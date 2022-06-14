using System;
using System.Collections;
using System.Collections.Generic;
using Interactable_Objects;
using UnityEngine;

namespace Interactable_Objects
{
    public class DropFromSink : EventInteractable
    {
        #region Private Fields

        private AudioSource _audioSource;
        private bool _interactionActive;
        private Animator _myAnimator;

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
                _myAnimator.SetTrigger("Drop");
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