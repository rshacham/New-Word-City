using System.Collections;
using System.Collections.Generic;
using Interactable_Objects;
using Player_Control;
using UnityEngine;

namespace Interactable_Objects
{
    public class DropFromTree : EventInteractable
    {

        #region Inspector

        [SerializeField] 
        private Transform movingObject;
        
        #endregion
        
        
        
        #region Private Fields

        private TreeVillage _myTree;

        private Movement _playerMovement;

        private Vector2 _originalMovingPosition;

        #endregion
        
        #region EventInteractable 
        void Start()
        {
            _myTree = GetComponentInParent<TreeVillage>();
            _playerMovement = FindObjectOfType<Movement>();
            _originalMovingPosition = movingObject.position;
        }

        protected override void ScriptInteract()
        {
            _myTree.ONTree = false;
            _playerMovement.EnableMovement = false;
            _playerMovement.transform.parent = movingObject;
            movingObject.GetComponent<Animator>().SetTrigger("Jump");
        }


        public void EndJump()
        {
            _playerMovement.transform.parent = null;
            movingObject.position = _originalMovingPosition;
            _playerMovement.EnableMovement = true;
        }

        
        #endregion
    }
}

