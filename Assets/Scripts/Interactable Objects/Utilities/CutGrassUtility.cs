using System;
using Avrahamy;
using UnityEngine;

namespace Interactable_Objects
{
    class CutGrassUtility : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        [Tooltip("The grass respawn time")]
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

        public void TurnOff()
        {
            offTime.Start();
            _mySpriteRenderer.enabled = false;
        }

        #endregion
    }
}