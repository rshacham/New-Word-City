using System.Collections;
using Managers;
using Player_Control;
using UnityEngine;

namespace Interactable_Objects
{
    public class DropFromTreeInteractable : EventInteractable
    {
        #region Inspector

        [SerializeField]
        private Transform movingObject;

        #endregion

        #region Private Fields

        private TreeVillageInteractable _myTree;

        private AudioSource _audioSource;

        private AudioSource _villageAudioSource;

        private Movement _playerMovement;

        private Vector2 _originalMovingPosition;

        // TODO: to AnimatorParameter
        private static readonly int Jump = Animator.StringToHash("Jump");

        #endregion

        #region EventInteractable

        void Start()
        {
            _myTree = GetComponentInParent<TreeVillageInteractable>();
            _playerMovement = FindObjectOfType<Movement>();
            _originalMovingPosition = movingObject.position;
            _audioSource = GetComponent<AudioSource>();
            _villageAudioSource = _myTree.gameObject.GetComponent<AudioSource>();
            _myTree.GetComponent<Animator>();
        }

        protected override void ScriptInteract()
        {
            _myTree.OnTree = false;
            _playerMovement.EnableMovement = false;
            _playerMovement.transform.parent = movingObject;
            // playerCollider.isTrigger = true;
            StaticEventsGameManager.OnPlayerShouldCollide(this, false);
            movingObject.GetComponent<Animator>().SetTrigger(Jump);
            _playerMovement.gameObject.GetComponent<Animator>().SetTrigger(Jump);
            _villageAudioSource.volume = 0;
            _audioSource.Play();
        }

        #endregion

        #region DropInteractable

        public void EndJump()
        {
            _playerMovement.transform.parent = null;
            movingObject.position = _originalMovingPosition;
            _playerMovement.EnableMovement = true;
            _playerMovement.DesiredVelocity = Vector2.zero;
            // playerCollider.isTrigger = false;
            StaticEventsGameManager.OnPlayerShouldCollide(this, true);
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