using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using Player_Control;
using UnityEngine;

namespace Interactable_Objects
{
    public class Trampolina : PairedAnimationInteractable
    {
        #region Inspector

        [Space]
        [Header("Trampoline Interactable")]
        [SerializeField]
        [Tooltip("How much the circle will be close between tutorial-world transition")]
        private float duration;

        [SerializeField]
        private Vector3 jumpNewPosition;

        [SerializeField]
        private float jumpSpeed;

        #endregion

        #region Private Fields

        private CartoonHoleManager _holeManager;

        private Movement _playerMovement;

        private AudioSource _myAudio;

        private Animator _myAnimator;

        #endregion

        #region EventInteractable

        private void Start()
        {
            _holeManager = FindObjectOfType<CartoonHoleManager>();
            _playerMovement = FindObjectOfType<Movement>();
            _myAudio = GetComponent<AudioSource>();
            _myAnimator = GetComponent<Animator>();
        }

        protected override void ScriptInteract()
        {
            if (!_playerMovement.FellToWorld)
            {
                _myAudio.Play();
                // GameManager._shared.ChangeCamera(1);
                GameManager.Shared.ChangeFollowPlayer(0);
                var transformPosition = transform.position;
                StartCoroutine(
                    _playerMovement.ChangePosition(
                        new Vector3(
                            transformPosition.x,
                            transformPosition.y + 20f,
                            transformPosition.z
                        ),
                        jumpSpeed,
                        false
                    )
                );
                UseOnEnd = false;
                StartCoroutine(GameManager.Shared.ThrowPlayerOnWorld());
                // TODO: this should stop the player when landing?
            }
        }

        #endregion

        #region Trampoline

        public void OnTrampoline(bool boolean)
        {
            _myAnimator.SetBool("On", boolean);
        }

        #region Coroutines

        public IEnumerator GetMeaning()
        {
            while (!_playerMovement.FellToWorld)
            {
                yield return new WaitForSeconds(0.2f);
            }

            UseOnEnd = true;
            Interact();
        }

        #endregion

        #endregion
    }
}