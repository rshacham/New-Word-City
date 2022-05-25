using System;
using System.Collections.Generic;
using Avrahamy;
using Avrahamy.Audio;
using Avrahamy.Math;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Interactable_Objects
{
    class RandomAnimationInteractable : EventInteractable
    {
        protected static readonly int Interact1 = Animator.StringToHash("Interact");

        [Header("Animations")]
        [SerializeField]
        private AudioEvent soundClip;
        
        [SerializeField]
        protected AnimationWithChance[] clips;

        protected int _count = 0;
        protected Animator _myAnimator;
        protected AnimatorOverrideController _animatorOverrideController;
        private AnimationWithChance _clip;
        private AudioSource _myAudioSource;


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
                    _animatorOverrideController["Interact"] = _clip;
                    _myAnimator.SetTrigger(Interact1);
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
        
        public bool Animated { get; set; }
    }

    [Serializable]
    public class AnimationWithChance : RandomUtils.ClassWithChance
    {
        [SerializeField]
        public AnimationClip clip;

        [SerializeField]
        public bool connectedToWord = true;

        public static implicit operator AnimationClip(AnimationWithChance t)
        {
            return t.clip;
        }
    }
}