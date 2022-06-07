using System.Collections;
using UnityEngine;

namespace Interactable_Objects
{
    /// <summary>
    /// Drop the beat car interaction 
    /// </summary>
    public class DropCar : EventInteractable
    {
        #region Inspector

        [Header("Sound Interaction")]
        [SerializeField]
        private AudioSource regularClip;

        [SerializeField]
        private AudioSource dropClip;

        [SerializeField]
        private float animationDelay;

        #endregion

        #region Private Fields

        // private bool _firstPlay;
        private Animator _carAnimator;

        // private int _counter;

        #endregion

        #region MonoBehaviour

        void Start()
        {
            _carAnimator = GetComponentInParent<Animator>();
        }

        public void ChangeAnimatorClose(bool boolean)
        {
            // print("yo" + counter++.ToString());
            _carAnimator.SetBool("Close", boolean);
        }
        
        

        #endregion

        #region Coroutines

        /// <summary>
        /// This Enumerator will start playing the default sound again.
        /// </summary>
        private IEnumerator RegularSound()
        {
            yield return new WaitForSeconds(dropClip.clip.length);
            _carAnimator.SetBool("Drop", false);
            yield return new WaitForSeconds(2f);
            // _carAnimator.enabled = false;
            regularClip.loop = true;
            regularClip.Play();


        }

        /// <summary>
        /// This Enumerator will start the animation.
        /// </summary>
        private IEnumerator StartAnimation()
        {
            yield return new WaitForSeconds(animationDelay);
            _carAnimator.enabled = true;
        }

        #endregion

        #region EventInteractable

        protected override void ScriptInteract()
        {
            if (!dropClip.isPlaying)
            {
                regularClip.loop = false;
                dropClip.PlayDelayed(+regularClip.clip.length - regularClip.time - 0.1f);
                _carAnimator.SetBool("Drop", true);
                StartCoroutine(RegularSound());
                // StartCoroutine(StartAnimation());
            }
        }

        #endregion
    }
}