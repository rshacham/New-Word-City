using UnityEngine;

namespace Interactable_Objects
{
    public class AirplaneUtility : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        private int airplaneAnimation;

        #endregion

        #region Private Fields

        private Animator _airplaneAnimator;

        // TODO: use AnimatorProperty
        private static readonly int Animation1 = Animator.StringToHash("Animation");

        #endregion

        #region MonoBehaviour

        void Start()
        {
            _airplaneAnimator = GetComponent<Animator>();
            _airplaneAnimator.SetInteger(Animation1, airplaneAnimation);
        }

        #endregion
    }
}