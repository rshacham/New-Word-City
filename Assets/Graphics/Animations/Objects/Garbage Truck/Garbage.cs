using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactable_Objects
{

    public class Garbage : EventInteractable
    {
        
        #region Inspector

        [SerializeField]
        [Tooltip("First clip is the fall sound, second is the fly buzz sound")]
        private AudioClip[] _mySounds;
        
        #endregion

        #region Private Properties

        private Animator _truckAnimator;

        private AudioSource _myAudio;

        #endregion

        #region MonoBehaviour

        void Start()
        {
            _truckAnimator = GetComponentInParent<Animator>();
            _myAudio = GetComponent<AudioSource>();
        }

        public void CloseToTruck(bool boolean)
        {
            _truckAnimator.SetBool("Close", boolean);
        }

        public void StartBuzzSound()
        {
            _myAudio.Stop();
            _myAudio.clip = _mySounds[1];
            _myAudio.loop = true;
            _myAudio.Play();
        }


        protected override void ScriptInteract()
        {
            if (!_truckAnimator.GetBool("Throw"))
            {
                _myAudio.Play();
                _truckAnimator.SetBool("Throw", true);
            }
        }

        #endregion

    }
}

