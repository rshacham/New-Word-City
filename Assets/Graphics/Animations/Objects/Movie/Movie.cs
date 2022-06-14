using System.Collections;
using System.Collections.Generic;
using BitStrap;
using Interactable_Objects;
using Interactable_Objects.Utilities;
using UnityEngine;

namespace Interactable_Objects
{
    public class Movie : EventInteractable
    {

        [SerializeField]
        [Tooltip("First is the action sound, second is the cut sound")]
        private AudioClip[] ourSounds;
        
        [SerializeField]
        private NamedAudio[] ourSoundsAgain;

        [SerializeField]
        private TriggerAnimationParameter cutParam;

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
                // ourSoundsAgain[0].Clip.Play(_myAudio);
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
            // ourSoundsAgain[1].Clip.Play(_myAudio);
            _myAnimator.SetTrigger("Cut");
            // cutParam.Set(_myAnimator);
        }
        
    }
}

