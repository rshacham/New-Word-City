using System.Collections;
using System.Collections.Generic;
using Interactable_Objects;
using UnityEngine;


namespace Interactable_Objects
{
    public class Building6 : EventInteractable
    {
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
            if (_myAnimator.GetBool("On"))
            {
                _myAnimator.SetBool("On", false);
                return;
            }

            int a = Random.Range(1, 5);
            Debug.Log(a);
            _myAnimator.SetBool("On", true);
            _myAnimator.SetInteger("Animation", Random.Range(1,5));
        }

        
        #endregion
    }
}
