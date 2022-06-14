using Managers;
using UnityEngine;

namespace Interactable_Objects
{
    public class SpaceshipInteractable : EventInteractable
    {
        #region Private Properties

        private SpriteRenderer _mySpriteRenderer;

        private Animator _spaceshipAnimator;

        private AudioSource _spaceshipSound;

        // TODO: create dropdown like the tags in BitStrap
        private const string FlyingThings = "Flying things";

        // TODO: use AnimatorParameters
        private static readonly int Close = Animator.StringToHash("Close");
        private static readonly int Fly = Animator.StringToHash("Fly");

        #endregion


        #region MonoBehaviour

        void Start()
        {
            _spaceshipAnimator = GetComponent<Animator>();
            _spaceshipSound = GetComponent<AudioSource>();
            GetComponentInChildren<PolygonCollider2D>();
            _mySpriteRenderer = GetComponent<SpriteRenderer>();
            FindObjectOfType<OpeningSceneMenu>();
        }

        #endregion


        #region EventInteractable

        protected override void ScriptInteract()
        {
            if (!_spaceshipAnimator.GetBool(Fly))
            {
                _mySpriteRenderer.sortingLayerID = SortingLayer.NameToID(FlyingThings);
                _spaceshipAnimator.SetBool(Fly, true);
                _spaceshipSound.Play();
            }
        }

        #endregion

        #region SpaceshipInteractable

        public void CloseToSpaceship(bool boolean)
        {
            _spaceshipAnimator.SetBool(Close, boolean);
        }

        public void ResetSpaceship()
        {
            _spaceshipAnimator.SetBool(Fly, false);
        }

        public void Land()
        {
            _mySpriteRenderer.sortingLayerID = 0;
        }

        #endregion
    }
}