using UnityEngine;

namespace Managers
{
    // TODO: merge all opening scene scripts
    public class OpeningSceneMenu : MonoBehaviour
    {
        [SerializeField]
        private AudioClip[] spaceshipSounds;

        private AudioSource _audioSource;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlaySound(int sound)
        {
            _audioSource.Stop();
            _audioSource.PlayOneShot(spaceshipSounds[sound]);
        }
    }
}
