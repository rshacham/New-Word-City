using System.Collections;
using System.Collections.Generic;
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

        
        #endregion


        #region MonoBehaviour

        void Start()
        {
            _spaceshipAnimator = GetComponent<Animator>();
            _spaceshipSound = GetComponent<AudioSource>();
            _spaceshipCollider = GetComponentInChildren<PolygonCollider2D>();
            _originalPosition = transform.position;
        }

        public void CloseToSpaceship(bool boolean)
        {
            _spaceshipAnimator.SetBool("Close", boolean);
        }

        public void StartResetCoroutine()
        {
            StartCoroutine(ResetSpaceship());
        }

        public IEnumerator ResetSpaceship()
        {
            yield return new WaitForSeconds(resetDelay);
            gameObject.SetActive(true);
            transform.position = _originalPosition;
            _spaceshipAnimator.SetBool("Fly", false);
            _spaceshipCollider.enabled = true;
        }
        
        #endregion


        protected override void ScriptInteract()
        {
            _spaceshipCollider.enabled = false;
            _spaceshipAnimator.SetTrigger("Fly");
        }

    }
}


