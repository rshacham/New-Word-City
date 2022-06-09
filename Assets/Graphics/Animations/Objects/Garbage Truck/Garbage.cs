using System;
using System.Collections;
using System.Collections.Generic;
using Avrahamy.Audio;
using BitStrap;
using Player_Control;
using UnityEngine;

namespace Interactable_Objects
{

    public class Garbage : EventInteractable
    {
        
        #region Inspector

        [SerializeField]
        [Tooltip("First clip is the fall sound, second is the fly buzz sound")]
        private AudioClip[] mySounds;

        [SerializeField]
        private BoolAnimationParameter highlightParam;
        [SerializeField]
        private BoolAnimationParameter throwParam;
        
        [SerializeField]
        private GarbageSounds mySoundsAgain;
        
        #endregion

        #region Private Properties

        private Animator _truckAnimator;

        private AudioSource _myAudio;

        #endregion

        #region MonoBehaviour

        void Start()
        {
            _truckAnimator = GetComponentInParent<Animator>();
            _myAudio = GetComponent<AudioSource>();
        }

        public void CloseToTruck(bool boolean)
        {
            _truckAnimator.SetBool("Close", boolean);
            // highlightParam.Set(_truckAnimator, boolean);
        }

        // TODO: use this instead
        // public override bool SetInteraction(PlayerInteract other)
        // {
        //     var ret = base.SetInteraction(other);
        //     if (ret)
        //     {
        //         highlightParam.Set(_truckAnimator, true);
        //     }
        //     return ret;
        // }
        // public override void RemoveInteraction(PlayerInteract other)
        // {
        //     base.SetInteraction(other);
        //     highlightParam.Set(_truckAnimator, false);
        // }



        public void StartBuzzSound()
        {
            _myAudio.Stop();
            _myAudio.clip = mySounds[1];
            _myAudio.loop = true;
            _myAudio.Play();
            // mySoundsAgain.buzzSound.Play(_myAudio);
        }


        protected override void ScriptInteract()
        {
            // if (!_truckAnimator.GetBool(throwParam))
            if (!_truckAnimator.GetBool("Throw"))
            {
                _myAudio.Play();
                // mySoundsAgain.fallSound.Play(_myAudio);
                _truckAnimator.SetBool("Throw", true);
                // throwParam.Set(_truckAnimator, true);
            }
        }

        #endregion
    }

    [Serializable]
    public class GarbageSounds
    {
        public AudioEvent fallSound;
        public AudioEvent buzzSound;
    }
}

