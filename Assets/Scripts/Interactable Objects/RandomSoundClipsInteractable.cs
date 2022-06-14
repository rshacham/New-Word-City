using System.Collections.Generic;
using Avrahamy.Math;
using Interactable_Objects.Utilities;
using UnityEngine;

namespace Interactable_Objects
{
    /// <summary>
    /// Choose random Audio Event to play as interaction
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class RandomSoundClipsInteractable : EventInteractable
    {
        #region Inspector

        [Header("Sound Interaction")]
        [SerializeField]
        [Tooltip("List Of AudioEvents to pick from. Use RandomWithWeights to get that functionality")]
        protected List<NamedAudio> clips = new List<NamedAudio>();

        #endregion

        #region Private Fields

        private AudioSource _mySource;

        #endregion

        #region EventInteractable

        protected override void Awake()
        {
            base.Awake();
            _mySource = GetComponent<AudioSource>();
        }

        protected override void ScriptInteract()
        {
            if (_mySource.isPlaying || clips.Count == 0)
            {
                return;
            }

            clips.ChooseRandom().Clip.Play(_mySource);
        }

        #endregion
    }
}