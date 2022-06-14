using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactable_Objects
{
    public class Airplane : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        private int airplaneAnimation;
        
        #endregion

        #region Private Fields

        private Animator _airplaneAnimator;

        #endregion

        #region MonoBehaviour

        void Start()
        {
            _airplaneAnimator = GetComponent<Animator>();
            _airplaneAnimator.SetInteger("Animation", airplaneAnimation);
        }

        #endregion

    }
}