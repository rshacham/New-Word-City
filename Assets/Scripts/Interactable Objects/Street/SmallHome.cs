using System.Collections;
using System.Collections.Generic;
using Interactable_Objects;
using UnityEngine;

namespace Interactable_Objects
{
    public class SmallHome : EventInteractable
    {
        #region Private Fields

        private Animator _myAnimator;
        private AudioSource _myAudio;

        #endregion


        #region MonoBeheaviour

        private void Start()
        {
            _myAnimator = GetComponent<Animator>();
            _myAudio = GetComponent<AudioSource>();
        }

        #endregion
        
        
        #region Event Interactable

        protected override void ScriptInteract()
        {
            if (_myAudio.clip != null)
            {
                _myAudio.Play();
            }
            _myAnimator.SetTrigger("Blue");
        }

        
        #endregion
    }
}

