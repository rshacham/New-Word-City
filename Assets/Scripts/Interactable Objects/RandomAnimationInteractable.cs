using Avrahamy.Audio;
using Avrahamy.Math;
using BitStrap;
using Interactable_Objects.Utilities;
using UnityEngine;

namespace Interactable_Objects
{
    /// <summary>
    /// Choose random animation from list
    /// </summary>
    class RandomAnimationInteractable : EventInteractable
    {
        #region Inspector

        [Header("Animations Interactable")]
        [SerializeField]
        [Tooltip("The sound clip used by this object")]
        private AudioEvent soundClip;

        [SerializeField]
        [Tooltip("List of animations to choose from")]
        protected AnimationWithChance[] clips;

        [SerializeField]
        [AnimatorField("_myAnimator")]
        [Tooltip("The trigger that starts the animation in the animation controller")]
        private TriggerAnimationParameter interactTrigger;

        [SerializeField]
        [Tooltip("The name of the animation clip to replace")]
        private string interactionAnimationName = "Interact";

        #endregion

        #region Private Fields

        private int _count;
        private Animator _myAnimator;
        private AnimatorOverrideController _animatorOverrideController;
        private AnimationWithChance _clip;
        private AudioSource _myAudioSource;

        #endregion

        #region Public Properties

        public bool Animated { get; set; }

        #endregion

        #region EventInteractable

        protected override void Awake()
        {
            base.Awake();
            _myAnimator = GetComponent<Animator>();
            _animatorOverrideController = new AnimatorOverrideController(_myAnimator.runtimeAnimatorController);
            _myAnimator.runtimeAnimatorController = _animatorOverrideController;
            _myAudioSource = GetComponent<AudioSource>();
        }

        protected override void ScriptInteract()
        {
            if (Animated)
            {
                return;
            }

            switch (_count)
            {
                case 0:
                    Animated = true;
                    UseOnEnd = false;
                    _count = 1;
                    _clip = clips.ChooseRandomWithChancesC();
                    _animatorOverrideController[interactionAnimationName] = _clip;
                    // _myAnimator.SetTrigger(InteractTrigger);
                    interactTrigger.Set(_myAnimator);
                    soundClip.Play(_myAudioSource);
                    break;
                default:
                    if (_clip.connectedToWord)
                    {
                        UseOnEnd = true;
                    }

                    _count = 0;
                    break;
            }
        }

        #endregion
    }
}