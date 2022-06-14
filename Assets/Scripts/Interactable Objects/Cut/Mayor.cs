using System.Collections;
using System.Collections.Generic;
using Interactable_Objects;
using UnityEngine;

namespace Interactable_Objects
{
    public class Mayor : EventInteractable
    {
        #region Private Fields

        private Animator _mayorAnimator;

        private AudioSource _mayorAudioSource;

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
            _mayorAnimator.SetBool("Close", boolean);
        }

        #region Coroutines

        public IEnumerator FinishAnimation()
        {
            yield return new WaitForSeconds(_mayorAudioSource.clip.length);
            _mayorAnimator.SetBool("Cut", false);
        }

        #endregion

        #endregion

        #region EventInteractable

        protected override void ScriptInteract()
        {
            _mayorAnimator.SetBool("Cut", true);
            _mayorAudioSource.Play();
            StartCoroutine(FinishAnimation());
        }

        #endregion
    }
}