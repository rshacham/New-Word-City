using System.Collections;
using System.Collections.Generic;
using Interactable_Objects;
using UnityEngine;

namespace Interactable_Objects
{
    public class Movie : EventInteractable
    {

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
                _myAudio.Play();
            }
        }
        
        protected override void ScriptInteract()
        {
            _myAnimator.SetTrigger("Cut");
        }
        
    }
}

