using System.Collections;
using UnityEngine;

namespace Interactable_Objects
{
    public class ProtestInteractable : EventInteractable
    {
        #region Inspector

        [SerializeField]
        private AudioClip[] protestorsSounds;

        #endregion

        #region Private Properties

        private Animator[] _protestorsAnimators;

        private AudioSource _protestAudio;

        private int _soundCounter;

        // TODO: to AnimationParameter
        private static readonly int Protest1 = Animator.StringToHash("Protest");

        #endregion


        #region MonoBehaviour

        void Start()
        {
            _protestorsAnimators = GetComponentsInChildren<Animator>();
            _protestAudio = GetComponent<AudioSource>();
        }

        #endregion


        #region EventInteractable

        protected override void ScriptInteract()
        {
            if (_protestAudio.isPlaying)
            {
                return;
            }

            foreach (var protestor in _protestorsAnimators)
            {
                protestor.SetBool(Protest1, true);
            }

            _protestAudio.clip = protestorsSounds[_soundCounter++ % protestorsSounds.Length];
            _protestAudio.Play();
            StartCoroutine(CancelProtest());
        }

        #endregion

        #region Protest

        public void ClostToProtest(bool boolean)
        {
            _protestorsAnimators[0].SetBool(Protest1, boolean);
        }

        private IEnumerator CancelProtest()
        {
            yield return new WaitForSeconds(_protestAudio.clip.length);
            foreach (var protestor in _protestorsAnimators)
            {
                protestor.SetBool(Protest1, false);
            }
        }

        #endregion
    }
}