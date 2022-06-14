using System;
using Avrahamy.Audio;
using UnityEngine;

namespace Interactable_Objects.Utilities
{
    /// <summary>
    /// Class to satisfy my need to have names for everything in the inspector.
    /// </summary>
    [Serializable]
    public class NamedAudio : ISerializationCallbackReceiver 
    {
        #region Inspector

        [SerializeField]
        [HideInInspector]
        private string name;
        
        [SerializeField]
        [Tooltip("The audio event to use")]
        private AudioEvent audioEvent; // todo: inherit class with chance - require manual change for all existing

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