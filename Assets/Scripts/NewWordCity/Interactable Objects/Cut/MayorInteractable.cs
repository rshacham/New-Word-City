using System.Collections;
using UnityEngine;

namespace Interactable_Objects
{
    public class MayorInteractable : EventInteractable
    {
        
        #region Private Fields

        private Animator _mayorAnimator;

        private AudioSource _mayorAudioSource;
        
        // TODO: switch to AnimationTriggerParameters
        private static readonly int Close = Animator.StringToHash("Close");
        private static readonly int Cut = Animator.StringToHash("Cut");

        #endregion

        #region MonoBehaviours

        void Start()
        {
            _mayorAnimator = GetComponent<Animator>();
            _mayorAudioSource = GetComponent<AudioSource>();
        }

        #endregion

        #region Public Methods

        public void CloseToMayor(bool boolean)
        {
            _mayorAnimator.SetBool(Close, boolean); // TODO: OnSetInteraction
        }

        #region Coroutines

        private IEnumerator FinishAnimation()
        {
            yield return new WaitForSeconds(_mayorAudioSource.clip.length);
            _mayorAnimator.SetBool(Cut, false);
        }

        #endregion

        #endregion

        #region EventInteractable

        protected override void ScriptInteract()
        {
            _mayorAnimator.SetBool(Cut, true);
            _mayorAudioSource.Play();
            StartCoroutine(FinishAnimation());
        }

        #endregion
    }
}