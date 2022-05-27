using System;
using Avrahamy;
using Avrahamy.Audio;
using Avrahamy.Math;
using UnityEngine;

namespace Interactable_Objects
{
    /// <summary>
    /// Class to satisfy my need to have names for everything in the inspector.
    /// </summary>
    [Serializable]
    public class NamedAudio : ISerializationCallbackReceiver // todo: inherit class with chance
    {
        #region Inspector

        [SerializeField]
        [HideInInspector]
        private string name;
        
        [SerializeField]
        [Tooltip("The audio event to use")]
        private AudioEvent audioEvent;

        #endregion

        #region Public Properties

        public AudioEvent Clip
        {
            get => audioEvent;
            set => audioEvent = value;
        }

        public string Name
        {
            get => name;
            set => name = value;
        }

        #endregion

        #region Type Conversions

        public NamedAudio(AudioEvent t)
        {
            audioEvent = t;
            name = audioEvent.Name;
        }

        public static implicit operator AudioEvent(NamedAudio t)
        {
            return t.Clip;
        }
        
        public static implicit operator NamedAudio(AudioEvent t)
        {
            return new NamedAudio(t);
        }
        
        public static implicit operator NamedAudio(AudioClip t)
        {
            return new NamedAudio((SimpleAudioEvent) t);
        }

        #endregion

        #region ISerializationCallbackReceiver

        private void OnValidate()
        {
            name = audioEvent != null ? audioEvent.Name : "AudioClip";
        }

        public void OnBeforeSerialize()
        {
            OnValidate();
        }

        public void OnAfterDeserialize()
        {
            OnValidate();
        }

        #endregion
    }
}