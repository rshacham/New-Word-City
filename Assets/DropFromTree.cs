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
        
        [SerializeField]
        private BoxCollider2D _playerCollider;

        
        #endregion
        
        
        
        #region Private Fields

        private TreeVillage _myTree;

        private AudioSource _audioSource;

        private Movement _playerMovement;

        private Vector2 _originalMovingPosition;


        #endregion
        
        #region EventInteractable 
        void Start()
        {
            _myTree = GetComponentInParent<TreeVillage>();
            _playerMovement = FindObjectOfType<Movement>();
            _originalMovingPosition = movingObject.position;
            _audioSource = GetComponent<AudioSource>();
        }

        protected override void ScriptInteract()
        {
            _myTree.ONTree = false;
            _playerMovement.EnableMovement = false;
            _playerMovement.transform.parent = movingObject;
            _playerCollider.isTrigger = true;
            movingObject.GetComponent<Animator>().SetTrigger("Jump");
            _playerMovement.gameObject.GetComponent<Animator>().SetTrigger("Jump");
            _audioSource.Play();
        }


        public void EndJump()
        {
            _playerMovement.transform.parent = null;
            movingObject.position = _originalMovingPosition;
            _playerMovement.EnableMovement = true;
            _playerCollider.isTrigger = false;
            

        }

        
        #endregion
    }
}

