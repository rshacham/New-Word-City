using UnityEngine;

namespace Interactable_Objects
{
    public class BeachUtility : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        [Tooltip("First sound is beach sound, Second sound is shark interaction sound")]
        private AudioClip[] beachSounds;

        #endregion
        
        #region Private Fields

        private AudioSource _audioSource;

        #endregion

        private void Start()
        {
            _audioSource = GetComponentInChildren<AudioSource>();
        }
        
        public void ChangeSound(int sound)
        {
            _audioSource.clip = beachSounds[sound];
            _audioSource.Play();
        }
    }
}