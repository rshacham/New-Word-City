using UnityEngine;


namespace Interactable_Objects
{
    public class SharkInteractable : EventInteractable
    {
        #region Inspector

        [SerializeField]
        [Tooltip("First sprite is when not close, Second sprite is when close")]
        private Sprite[] tubeSprites;

        [SerializeField]
        [Tooltip("SpriteRenderer of tube that player should be close to so interaction can be made")]
        private SpriteRenderer tubeRenderer;

        #endregion

        #region Private Fields

        private Animator _sharkAnimator;

        private BeachUtility _beach;

        // TODO: use ANimatorParameters
        private static readonly int Movement = Animator.StringToHash("Movement");
        private static readonly int Animation1 = Animator.StringToHash("Animation");

        #endregion


        #region EventInteractable

        private void Start()
        {
            _sharkAnimator = GetComponent<Animator>();
            _beach = FindObjectOfType<BeachUtility>();
        }

        #endregion


        #region EventInteractable

        protected override void ScriptInteract()
        {
            _sharkAnimator.SetTrigger(Movement);
            _sharkAnimator.SetBool(Animation1, true);
            _beach.ChangeSound(1);
        }

        #endregion


        #region Event Callbacks

        public void StopInteraction()
        {
            _beach.ChangeSound(0);
            _sharkAnimator.SetBool(Animation1, false);
        }


        public void CloseToTube(int sprite)
        {
            tubeRenderer.sprite = tubeSprites[sprite];
        }

        #endregion
    }
}