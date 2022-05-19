using System.Collections.Generic;
using Avrahamy;
using Avrahamy.Audio;
using Avrahamy.Math;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Interactable_Objects
{
    [RequireComponent(typeof(AudioSource))]
    class RandomSoundClipsInteractable : EventInteractable
    {
        [Header("Sound Interaction")]
        [SerializeField]
        [Tooltip("List Of AudioEvents to pick from. Use RandomWithWeights to get that functionality")]
        protected List<NamedAudio> clips = new List<NamedAudio>();

        private AudioSource _mySource;

        #region EventInteractable

        protected override void Awake()
        {
            base.Awake();
            _mySource = GetComponent<AudioSource>();
        }

        protected override void ScriptInteract()
        {
            if (_mySource.isPlaying)
            {
                return;
            }
            var idx = Random.Range(0, clips.Count);
            clips[idx].Clip.Play(_mySource);
        }

        #endregion
    }
}