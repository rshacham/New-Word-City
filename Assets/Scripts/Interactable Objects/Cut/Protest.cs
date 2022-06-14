using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactable_Objects
{
    public class Protest : EventInteractable
    {
        
        #region Private Properties

        private Animator[] _protestorsAnimators;

        private AudioSource _protestAudio;
        
        private int _soundCounter;

        #endregion
        
        #region Inspector
        
        [SerializeField] private AudioClip[] _protestorsSounds;
        
        #endregion


        #region MonoBehaviour

        void Start()
        {
            _protestorsAnimators =  GetComponentsInChildren<Animator>();
            _protestAudio = GetComponent<AudioSource>();
        }

        public void ClostToProtest(bool boolean)
        {
            _protestorsAnimators[0].SetBool("Protest", boolean);
        }

        private IEnumerator CancelProtest()
        {
            yield return new WaitForSeconds(_protestAudio.clip.length);
            foreach (var protestor in _protestorsAnimators)
            {
                protestor.SetBool("Protest", false);
            }
        }
        
        #endregion


        protected override void ScriptInteract()
        {
            if (_protestAudio.isPlaying)
            {
                return;
            }
            foreach (var protestor in _protestorsAnimators)
            {
                protestor.SetBool("Protest", true);
            }

            _protestAudio.clip = _protestorsSounds[_soundCounter++ % _protestorsSounds.Length];
            _protestAudio.Play();
            StartCoroutine(CancelProtest());
        }


    }

}



