using System.Collections;
using BitStrap;
using Managers;
using Player_Control;
using UI;
using UnityEngine;

namespace Interactable_Objects
{
    public class TrampolineInteractable : PairedAnimationInteractable
    {
        #region Inspector

        [Space]
        [Header("Trampoline Interactable")]
        [SerializeField]
        private float jumpSpeed;

        [SerializeField]
        private BoolAnimationParameter onParam;

        #endregion

        #region Private Fields

        private Movement _playerMovement;

        private AudioSource _myAudio;

        private Animator _myAnimator;

        #endregion

        #region EventInteractable

        private void Start()
        {
            FindObjectOfType<CartoonHoleManager>();
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
                CameraAndTeleportManager.Shared.ChangeFollowPlayer(0);
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
                StartCoroutine(CameraAndTeleportManager.Shared.ThrowPlayerOnWorld());
                // TODO: this should stop the player when landing?
            }
        }

        #endregion

        #region Trampoline

        public void OnTrampoline(bool boolean)
        {
            // _myAnimator.SetBool("On", boolean);
            onParam.Set(_myAnimator, boolean);
        }

        #region Coroutines

        public IEnumerator GetMeaning()
        {
            while (!_playerMovement.FellToWorld)
            {
                yield return new WaitForSeconds(0.2f);
            }

            // yield return new WaitUntil(() => _playerMovement.FellToWorld); // TODO: use this

            UseOnEnd = true;
            Interact();
        }

        #endregion

        #endregion
    }
}