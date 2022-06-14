using UnityEngine;

namespace Interactable_Objects
{
    public class SmallHomeInteractable : EventInteractable
    {
        #region Private Fields

        private Animator _myAnimator;
        private AudioSource _myAudio;

        // TODO: Use AnimatorParameter
        private static readonly int Blue = Animator.StringToHash("Blue");

        #endregion


        #region MonoBeheaviour

        private void Start()
        {
            _myAnimator = GetComponent<Animator>();
            _myAudio = GetComponent<AudioSource>();
        }

        #endregion


        #region Event Interactable

        protected override void ScriptInteract()
        {
            if (_myAudio.clip != null)
            {
                _myAudio.Play();
            }

            _myAnimator.SetTrigger(Blue);
        }

        #endregion
    }
}