using System;
using System.Collections;
using Avrahamy;
using Avrahamy.Audio;
using Avrahamy.EditorGadgets;
using BitStrap;
using Player_Control;
using UnityEngine;

namespace Interactable_Objects
{
    class WeddingInteractable : EventInteractable
    {
        #region Inspector

        [SerializeField]
        [Tooltip("The type this object is: Couple controls all the rest")]
        private WeddingType interactableType;

        [SerializeField]
        [HideInInspector]
        private SimpleAudioEvent weddingSound;

        [SerializeField]
        [Tooltip("The boolean parameter that controls the animation")]
        private BoolAnimationParameter boolParameter;
        
        [SerializeField]
        [HideInInspector]
        private PassiveTimer weddingTimer = new PassiveTimer();

        [SerializeField]
        [Tooltip("Delay till the wedding sound clip begins. Only the Couple has any control over this")]
        private PassiveTimer soundDelay;

        [SerializeField] 
        [Tooltip("Gameobject of heart that appears above the couple when interaction can be made")]
        private GameObject[] hearts;

        #endregion

        #region Events

        private static event EventHandler<bool> CelebrateWedding;

        private static void OnCelebrateWedding(object sender, bool e)
        {
            CelebrateWedding?.Invoke(sender, e);
        }

        #endregion

        #region Private Fields

        private Animator _animator;
        private AudioSource _audioSource;

        #endregion

        #region EventInteractable

        protected override void Awake()
        {
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
            
            CelebrateWedding += GuestCelebrateWedding;
            if (interactableType == WeddingType.Guest)
            {
                CanInteract = false;
            }
            else
            {
                weddingTimer.Duration = _audioSource.clip.length;
            }
            
            base.Awake();
        }


        // IEnumerator _delayClip()
        // {
        //     yield return new WaitForSeconds(soundDelay);
        //     _audioSource.Play();
        // }

        private void GuestCelebrateWedding(object sender, bool e)
        {
            boolParameter.Set(_animator, e);
        }

        protected override void ScriptInteract()
        {
            if (interactableType != WeddingType.Couple || _audioSource.isPlaying)
            {
                return;
            }

            weddingTimer.Start();
            soundDelay.Start();
            OnCelebrateWedding(this, true);
        }

        public void CloseToCouple(bool isClose)
        {
            foreach (var heart in hearts)
            {
                heart.SetActive(isClose);
            }
        }

        protected void Update()
        {
            if (weddingTimer.IsSet && !weddingTimer.IsActive)
            {
                weddingTimer.Clear();
                OnCelebrateWedding(this, false);
            }
            if (soundDelay.IsSet && !soundDelay.IsActive)
            {
                soundDelay.Clear();
                // weddingSound.Play(_audioSource);
                _audioSource.Play();
            }
        }

        #endregion
    }
    
    

    public enum WeddingType
    {
        Guest,
        Couple
    }
}