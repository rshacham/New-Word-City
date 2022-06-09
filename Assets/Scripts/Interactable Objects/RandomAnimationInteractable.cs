using System.Collections.Generic;
using Avrahamy;
using Avrahamy.Audio;
using Avrahamy.Math;
using BitStrap;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

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
        private AudioEvent soundClip;

        [SerializeField]
        protected AnimationWithChance[] clips;

        [SerializeField]
        [AnimatorField("_myAnimator")]
        private TriggerAnimationParameter interactTrigger;

        [SerializeField]
        private string interactionAnimationName = "Interact";

        #endregion

        #region Private Fields

        private static readonly int InteractTrigger = Animator.StringToHash("Interact");

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
                    _myAnimator.SetTrigger(InteractTrigger);
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