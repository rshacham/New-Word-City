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

        private Animator _myTreeAnimator;

        private AudioSource _audioSource;

        private AudioSource _villageAudioSource;

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
            _villageAudioSource = _myTree.gameObject.GetComponent<AudioSource>();
            _myTreeAnimator = _myTree.GetComponent<Animator>();
        }

        protected override void ScriptInteract()
        {
            _myTree.ONTree = false;
            _playerMovement.EnableMovement = false;
            _playerMovement.transform.parent = movingObject;
            _playerCollider.isTrigger = true;
            movingObject.GetComponent<Animator>().SetTrigger("Jump");
            _playerMovement.gameObject.GetComponent<Animator>().SetTrigger("Jump");
            _villageAudioSource.volume = 0;
            _audioSource.Play();
        }


        public void EndJump()
        {
            _playerMovement.transform.parent = null;
            movingObject.position = _originalMovingPosition;
            _playerMovement.EnableMovement = true;
            _playerMovement.DesiredVelocity = Vector2.zero;
            _playerCollider.isTrigger = false;
            StartCoroutine(ReturnVillageSound());
        }

        IEnumerator ReturnVillageSound()
        {
            yield return new WaitForSeconds(1f);
            _villageAudioSource.volume = 1;
        }
        
        
        

        
        #endregion
    }
}

