using System;
using System.Collections.Generic;
using Avrahamy;
using Avrahamy.Math;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Interactable_Objects
{
    class RandomAnimationInteractable : EventInteractable
    {
        protected static readonly int Interact1 = Animator.StringToHash("Interact");
        
        [SerializeField]
        protected AnimationWithChance[] clips;

        protected int _count = 0;
        protected Animator _myAnimator;
        protected AnimatorOverrideController _animatorOverrideController;
        private AnimationWithChance _clip;


        protected override void Awake()
        {
            base.Awake();
            _myAnimator = GetComponent<Animator>();
            _animatorOverrideController = new AnimatorOverrideController(_myAnimator.runtimeAnimatorController);
            _myAnimator.runtimeAnimatorController = _animatorOverrideController;
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