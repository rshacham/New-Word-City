using System;
using System.Collections;
using System.Collections.Generic;
using Player_Control;
using UnityEngine;

namespace Interactable_Objects
{
    public class Trampolina : PairedAnimationInteractable
    {
        /// @see PairedAnimationInteractable.cs

        [Space]
        [Header("Trampoline Interactable")]
        [SerializeField]
        [Tooltip("How much the circle will be close between tutorial-world transition")]
        private float duration;

        [SerializeField]
        private Vector3 jumpNewPosition;

        [SerializeField]
        private float jumpSpeed;

        private CartoonHoleManager _holeManager;

        private Movement _playerMovement;

        private void Start()
        {
            _holeManager = FindObjectOfType<CartoonHoleManager>();
            _playerMovement = FindObjectOfType<Movement>();
        }

        protected override void ScriptInteract()
        {
            StartCoroutine(_playerMovement.ChangePosition(jumpNewPosition, jumpSpeed));
            _holeManager.CloseCircle(duration);
        }
    }
}

