using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactable_Objects
{

    public class Garbage : EventInteractable
    {

        #region Private Properties

        private Animator _truckAnimator;

        #endregion

        #region MonoBehaviour

        void Start()
        {
            _truckAnimator = GetComponentInParent<Animator>();
        }

        public void CloseToTruck(bool boolean)
        {
            _truckAnimator.SetBool("Close", boolean);
        }


        protected override void ScriptInteract()
        {
            _truckAnimator.SetTrigger("Throw");
        }

        #endregion

    }
}

