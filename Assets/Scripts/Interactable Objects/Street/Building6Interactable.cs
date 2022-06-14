using UnityEngine;


namespace Interactable_Objects
{
    public class Building6Interactable : EventInteractable
    {
        #region Private Fields

        private Animator _myAnimator;

        // TODO: Use AnimatorParameter
        private static readonly int On = Animator.StringToHash("On");
        private static readonly int Animation1 = Animator.StringToHash("Animation");

        #endregion


        #region MonoBehaviour

        void Start()
        {
            _myAnimator = GetComponent<Animator>();
        }

        #endregion


        #region Event Interactable

        protected override void ScriptInteract()
        {
            if (_myAnimator.GetBool(On))
            {
                _myAnimator.SetBool(On, false);
                return;
            }

            _myAnimator.SetBool(On, true);
            _myAnimator.SetInteger(Animation1, Random.Range(1, 5));
        }

        #endregion
    }
}