using System.Collections;
using System.Collections.Generic;
using Avrahamy.Audio;
using BitStrap;
using Interactable_Objects;
using UnityEngine;

namespace Interactable_Objects
{
    public class SmallHome : EventInteractable
    {
        [SerializeField]
        [Tooltip("You can use this instead of string convertion")]
        private TriggerAnimationParameter blueTrigger;

        [SerializeField]
        [Tooltip("the audio this house uses")]
        private AudioEvent audioEvent;
        
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
            // audioEvent.Play(_myAudio);
            // blueTrigger.Set(_myAnimator);
        }

        
        #endregion
    }
}

