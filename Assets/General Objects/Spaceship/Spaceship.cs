using System.Collections;
using System.Collections.Generic;
using Avrahamy;
using UnityEngine;

namespace Interactable_Objects
{
    public class Spaceship : EventInteractable
    {
        #region Private Properties

        private Animator _spaceshipAnimator;

        private AudioSource _spaceshipSound;

        private Vector3 _originalPosition;

        private PolygonCollider2D _spaceshipCollider;

        private MainMenu _mainMenu;

        #endregion

        #region Inspector

        [SerializeField]
        [Tooltip("Speed of spaceship ")]
        private float spaceshipSpeed;

        [SerializeField]
        [Tooltip("Acceleration of spaceship ")]
        private Vector3 spaceshipAcceleration;

        [SerializeField]
        [Tooltip("How much offset should the spaceship have")]
        private float yOffset;

        [SerializeField]
        [Tooltip("How much delay till spaceship appears again")]
        private float resetDelay;
        private SpriteRenderer _mySpriteRenderer;

        #endregion


        #region MonoBehaviour

        void Start()
        {
            _spaceshipAnimator = GetComponent<Animator>();
            _spaceshipSound = GetComponent<AudioSource>();
            _spaceshipCollider = GetComponentInChildren<PolygonCollider2D>();
            _originalPosition = transform.position;
            _mySpriteRenderer = GetComponent<SpriteRenderer>();
            _mainMenu = FindObjectOfType<MainMenu>();
        }

        public void CloseToSpaceship(bool boolean)
        {
            _spaceshipAnimator.SetBool("Close", boolean);
        }

        public void ResetSpaceship()
        {
            _spaceshipAnimator.SetBool("Fly", false);
        }

        public void Land()
        {
            _mySpriteRenderer.sortingLayerID = 0;

        }

        #endregion


        protected override void ScriptInteract()
        {
            if (!_spaceshipAnimator.GetBool("Fly"))
            {
                _mySpriteRenderer.sortingLayerID = SortingLayer.NameToID("Flying things");
                _spaceshipAnimator.SetBool("Fly", true);
                _spaceshipSound.Play();
            }
        }



    }
}