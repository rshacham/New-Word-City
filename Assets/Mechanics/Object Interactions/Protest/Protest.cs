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

        #endregion
        
        #region Inspector
        
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
            foreach (var protestor in _protestorsAnimators)
            {
                protestor.SetBool("Protest", true);
            }
            _protestAudio.Play();
            StartCoroutine(CancelProtest());
        }


    }

}



