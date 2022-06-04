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

        private bool _onTree = false;

        #endregion
        
        #region Inspector
        
        [SerializeField] 
        [Tooltip("Speed of move position animation")]
        private float animationSpeed;
        
        
        [SerializeField] 
        [Tooltip("New position after climbing up the tree")]
        private Vector3 onTreePosition;
        
        [SerializeField] 
        [Tooltip("New position after climbing down tree")]
        private Vector3 _offTreePosition;
        
        #endregion


        #region MonoBehaviour

        void Start()
        {
            _villageAnimator = GetComponentInParent<Animator>();
            _playerScript = FindObjectOfType<Movement>();
        }

        public void CloseToVillage(bool boolean)
        {
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

            if (!_onTree && _playerScript.EnableMovement)
            {
                StartCoroutine(_playerScript.ChangePosition(onTreePosition, animationSpeed));
                _onTree = true;
                return;
            }
            

            if (_onTree && _playerScript.EnableMovement)
            {
                StartCoroutine(_playerScript.ChangePosition(_offTreePosition, animationSpeed));
                _onTree = false;
            }
        }

        #endregion

    }
}



