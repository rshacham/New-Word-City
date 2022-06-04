using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactable_Objects
{
    public class TreeVillage : EventInteractable
    {
        #region Private Properties

        private Animator _villageAnimator;

        [SerializeField] Animator _playerAnimator;

        #endregion

        #region MonoBehaviour

        void Start()
        {
            _villageAnimator = GetComponentInParent<Animator>();
        }

        public void CloseToVillage(bool boolean)
        {
            Debug.Log("hey");
            _villageAnimator.SetBool("Semi", boolean);
        }


        protected override void ScriptInteract()
        {
            _villageAnimator.SetBool("Open", true);
        }

        #endregion

    }
}



