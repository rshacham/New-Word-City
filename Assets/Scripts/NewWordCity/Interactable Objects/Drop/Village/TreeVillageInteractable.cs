using Player_Control;
using UnityEngine;
using UnityEngine.Serialization;

namespace Interactable_Objects
{
    public class TreeVillageInteractable : EventInteractable
    {
        #region Inspector

        [FormerlySerializedAs("animationSpeed")]
        [SerializeField]
        [Tooltip("Speed of climbing animation")]
        private float climbingSpeed;

        [SerializeField]
        [Tooltip("Speed of falling animation")]
        private float fallingSpeed;

        [SerializeField]
        [Tooltip("New position after climbing up the tree")]
        private Vector3 onTreePosition;

        [SerializeField]
        [Tooltip("New position after climbing down tree")]
        private Vector3 offTreePosition;

        [SerializeField]
        [Tooltip(
            "First clip is for opening the ladder. Second clip is for climbing up. Third clip is for going down." +
            " Fourth is for closing the ladder.")]
        private AudioClip[] villageClips;

        #endregion

        #region Private Properties

        private Animator _villageAnimator;

        private AudioSource _villageSound;

        private bool _ladderOpen;

        private Movement _playerScript;

        private bool _onTree;

        // TODO: use AnimatorParameter
        private static readonly int Semi = Animator.StringToHash("Semi");
        private static readonly int CanDrop = Animator.StringToHash("CanDrop");
        private static readonly int Open = Animator.StringToHash("Open");

        #endregion

        #region Public Properties

        public bool OnTree
        {
            get => _onTree;
            set => _onTree = value;
        }

        #endregion

        #region MonoBehaviour

        void Start()
        {
            _villageAnimator = GetComponentInParent<Animator>();
            _villageSound = GetComponent<AudioSource>();
            _playerScript = FindObjectOfType<Movement>();
        }

        #endregion

        #region Public Methods

        public void CloseToVillage(bool boolean)
        {
            if (!OnTree && !_ladderOpen)
            {
                _villageAnimator.SetBool(Semi, boolean);
            }
        }


        public void OpenCloseLadder(bool boolean)
        {
            _villageAnimator.SetBool(CanDrop, boolean);
            if (boolean)
            {
                _villageSound.PlayOneShot(villageClips[3]);
                return;
            }

            _villageSound.PlayOneShot(villageClips[0]);
        }

        #endregion

        #region EventInteractable

        protected override void ScriptInteract()
        {
            if (!_ladderOpen)
            {
                _villageAnimator.SetBool(Open, true);
                _villageAnimator.SetBool(Semi, false);
                _ladderOpen = true;
                _villageSound.PlayOneShot(villageClips[0]);
                return;
            }

            if (!_onTree && _playerScript.EnableMovement)
            {
                StartCoroutine(_playerScript.ChangePosition(onTreePosition, climbingSpeed));
                _onTree = true;
                _villageSound.PlayOneShot(villageClips[1]);
                return;
            }

            if (_onTree && _playerScript.EnableMovement)
            {
                StartCoroutine(_playerScript.ChangePosition(offTreePosition, fallingSpeed));
                _villageSound.PlayOneShot(villageClips[2]);
                _onTree = false;
            }
        }

        #endregion
    }
}