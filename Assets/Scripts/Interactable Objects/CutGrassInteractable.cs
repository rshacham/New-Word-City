using System;
using Avrahamy;
using UnityEngine;

namespace Interactable_Objects
{
    class CutGrassInteractable : MonoBehaviour
    {
        [SerializeField]
        private PassiveTimer offTime;
        
        private SpriteRenderer _mySpriteRenderer;

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

        public void TurnOff()
        {
            offTime.Start();
            _mySpriteRenderer.enabled = false;
        }
    }
}