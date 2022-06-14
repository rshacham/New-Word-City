using UnityEngine;

namespace Interactable_Objects
{
    public class CowInteractable : EventInteractable
    {
        #region Inspector

        [Header("Sound Clips Looping:")]
        [SerializeField]
        private AudioSource firstSound;

        [SerializeField]
        private AudioSource secondSound;

        #endregion

        #region Private Fields

        private int _counter;

        #endregion

        #region EventInteractable

        protected override void ScriptInteract()
        {
            if (firstSound.isPlaying && secondSound.isPlaying)
            {
                return;
            }

            switch (++_counter % 2)
            {
                case 0:
                    firstSound.PlayOneShot(firstSound.clip);
                    return;
                case 1:
                    secondSound.PlayOneShot(secondSound.clip);
                    return;
            }
        }

        #endregion
    }
}