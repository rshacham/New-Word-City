using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactable_Objects
{

    public class Garbage : EventInteractable
    {

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

