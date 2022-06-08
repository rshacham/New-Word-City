using System;
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
        private WeddingType interactableType;

        [SerializeField]
        private AudioEvent weddingSound;

        [SerializeField]
        private PassiveTimer weddingTimer;

        [SerializeField]
        private BoolAnimationParameter boolParameter;

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
            CelebrateWedding += GuestCelebrateWedding;
            if (interactableType == WeddingType.Guest)
            {
                CanInteract = false;
            }

            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
            base.Awake();
        }

        private void GuestCelebrateWedding(object sender, bool e)
        {
            boolParameter.Set(_animator, e);
        }

        protected override void ScriptInteract()
        {
            if (interactableType != WeddingType.Couple)
            {
                return;
            }

            weddingTimer.Start();
            OnCelebrateWedding(this, true);
            weddingSound.Play(_audioSource);
        }

        protected void Update()
        {
            if (weddingTimer.IsSet && !weddingTimer.IsActive)
            {
                weddingTimer.Clear();
                OnCelebrateWedding(this, false);
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