using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Player_Control;
using UnityEngine;

namespace Interactable_Objects
{
    public class TreeVillage : EventInteractable
    {
        #region Private Properties

        private Animator _villageAnimator;

        private bool _ladderOpen;

        private Movement _playerScript;

        #endregion

        
        #region MonoBehaviour

        void Start()
        {
            _villageAnimator = GetComponentInParent<Animator>();
            _playerScript = FindObjectOfType<Movement>();
        }

        public void CloseToVillage(bool boolean)
        {
            Debug.Log("hey");
            _villageAnimator.SetBool("Semi", boolean);
        }


        protected override void ScriptInteract()
        {
            if (!_ladderOpen)
            {
                _villageAnimator.SetBool("Open", true);
                _ladderOpen = true;
                return;
            }
            
            _playerScript.SetAnimatorStateTrue("Climb");
        }

        #endregion

    }
}



