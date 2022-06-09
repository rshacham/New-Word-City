using System.Collections;
using System.Collections.Generic;
using Avrahamy;
using BitStrap;
using Interactable_Objects;
using UnityEngine;


namespace Interactable_Objects
{
    public class Building6Interactable : EventInteractable
    {
        // TODO: should really just switch to randomAnimationInteractable
        [SerializeField]
        [Tooltip("The parameter that control the on/off animation")]
        private BoolAnimationParameter onParameter;

        [SerializeField]
        [Tooltip("The parameter that chooses the animation")]
        private IntAnimationParameter animationParameter;
        
        #region Private Fields

        private Animator _myAnimator;

        #endregion


        #region MonoBehaviour

        void Start()
        {
            _myAnimator = GetComponent<Animator>();
        }

        #endregion


        #region Event Interactable

        protected override void ScriptInteract()
        {
            
            // if (_myAnimator.GetBool(onParameter))
            if (_myAnimator.GetBool("On"))
            {
                _myAnimator.SetBool("On", false);
                // onParameter.Set(_myAnimator, false);
                return;
            }

            int a = Random.Range(1, 5);
            // DebugLog.Log(LogTag.LowPriority, $"chosen anim: {a}. TODO RandomAnimationInteractable...");
            DebugLog.Log(LogTag.LowPriority, a);
            _myAnimator.SetBool("On", true);
            // onParameter.Set(_myAnimator, true);
            _myAnimator.SetInteger("Animation", Random.Range(1, 5)); // todo: You draw random a and then dont use it?
            // animationParameter.Set(_myAnimator, Random.Range(1, 5));
        }

        #endregion
    }
}