using System;
using Avrahamy;
using UnityEngine;

namespace Interactable_Objects
{
    class CutGrassUtility : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        [Tooltip("Time for grass respawn")]
        private PassiveTimer offTime;

        #endregion

        #region Private Fields

        private SpriteRenderer _mySpriteRenderer;

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            _mySpriteRenderer = GetComponentInParent<SpriteRenderer>();
        }

        private void Update()
        {
            if (offTime.IsSet && !offTime.IsActive)
            {
                offTime.Clear();
                _mySpriteRenderer.enabled = true;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Turn off the grass
        /// </summary>
        public void TurnOff()
        {
            offTime.Start();
            _mySpriteRenderer.enabled = false;
        }

        #endregion
    }
}