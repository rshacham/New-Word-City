using System.Collections;
using System.Collections.Generic;
using Interactable_Objects;
using UnityEngine;

namespace Interactable_Objects
{
    public class Movie : EventInteractable
    {

        [SerializeField]
        [Tooltip("First is the action sound, second is the cut sound")]
        private AudioClip[] ourSounds;

        private Animator _myAnimator;
        private AudioSource _myAudio;
        void Start()
        {
            _myAnimator = GetComponentInChildren<Animator>();
            _myAudio = GetComponent<AudioSource>();
        }

        public void CloseToMovie()
        {
            if (!_myAudio.isPlaying)
            {
                _myAudio.PlayOneShot(ourSounds[0]);
            }
        }
        
        protected override void ScriptInteract()
        {
            if (_myAudio.isPlaying)
            {
                UseOnEnd = false;
                return;
            }
            
            UseOnEnd = true;
            _myAudio.PlayOneShot(ourSounds[1]);
            _myAnimator.SetTrigger("Cut");
        }
        
    }
}

