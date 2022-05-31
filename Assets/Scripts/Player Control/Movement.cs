using System;
using Avrahamy;
using Avrahamy.EditorGadgets;
using BitStrap;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player_Control
{
    public class Movement : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        [Tooltip("Enable Movement using this script")]
        private bool enableMovement = true;

        [SerializeField]
        [Tooltip("Maximum movement speed")]
        private float maxSpeed;

        [SerializeField]
        [Tooltip("Maximum acceleration")]
        private float maxAcceleration;

        [SerializeField]
        [Tooltip("Move in isometric fashion")]
        private bool moveIsometric;

        [SerializeField]
        [Tooltip("Should velocity in all directions be the same?")]
        private bool normalizeDirection;

        [SerializeField]
        [Tooltip("The world angle")]
        [Range(0, 180)]
        private int isometricAngle = 120;

        [SerializeField]
        [Tooltip("Should smoothing be done?")]
        private bool smoothing;

        [SerializeField]
        [ConditionalHide("smoothing")]
        [Tooltip("Smoothing of player acceleration/deceleration")]
        private float smoothedTime = 1f;

        [HideInInspector]
        [Header("Material Transparency Tests")]
        [SerializeField]
        private Material peepingMat;

        [Header("Movement Animation")]
        [SerializeField]
        private MovementAnimationParameters parameters;

        #endregion

        #region Private Fields

        private Rigidbody2D _playerRigidBody;

        private Vector2 _isoVector;

        private Vector2 _desiredVelocity;

        private Vector2 _currentDamp;

        private PlayerInput _playerInput;

        private Animator _playerAnimator;

        // TODO: serialize? even as hidden?
        private readonly Quaternion _moveAngle = Quaternion.Euler(0, 0, -45);
        private const string Controller = "Controller";

        #endregion

        #region Public Properties

        public bool IsController => _playerInput != null && _playerInput.currentControlScheme == Controller;

        public bool EnableMovement
        {
            get => enableMovement;
            set => enableMovement = value;
        }

        public Vector2 DesiredVelocity
        {
            get => _desiredVelocity;
            set => _desiredVelocity = value;
        }

        #endregion

        #region MonoBehaviour

        private void OnValidate()
        {
            var angle = Mathf.Deg2Rad * ((180 - isometricAngle) / 2f);
            _isoVector = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _playerAnimator = GetComponent<Animator>();
            _playerRigidBody = GetComponent<Rigidbody2D>();
            Tutorial.PlayerMovement = this;
        }

        private void Update()
        {
            // TODO: Move to fixed update!
            var speed = _playerRigidBody.velocity.magnitude;
            parameters.velocity.Set(_playerAnimator, speed);
            if (speed > 0.02f)
            {
                var velocityNormalized = _playerRigidBody.velocity.normalized;
                parameters.posX.Set(_playerAnimator, velocityNormalized.x);
                parameters.posY.Set(_playerAnimator, velocityNormalized.y);
                DebugLog.Log(velocityNormalized);
            }
        }

        private void FixedUpdate()
        {
            if (smoothing)
            {
                _playerRigidBody.velocity = Vector2.SmoothDamp(
                    _playerRigidBody.velocity,
                    _desiredVelocity,
                    ref _currentDamp,
                    smoothedTime,
                    maxAcceleration,
                    Time.fixedDeltaTime
                );
            }
            else
            {
                _playerRigidBody.velocity = _desiredVelocity;
            }
            // peepingMat.SetVector(PlayerPos, playerRigidBody.position);
        }

        #endregion

        #region Input Callbacks

        public void OnMovement(InputAction.CallbackContext context)
        {
            if (!enableMovement)
            {
                return;
            }

            var movementVector = context.action.ReadValue<Vector2>();
            if (!IsController)
            {
                if (moveIsometric)
                {
                    movementVector = _moveAngle * movementVector;
                }

                movementVector *= _isoVector;
            }

            if (normalizeDirection) // TODO: if we want this, test simply using normalized input!
            {
                movementVector = movementVector.normalized;
            }

            _desiredVelocity = movementVector * maxSpeed;
        }

        #endregion
    }

    [Serializable]
    public struct MovementAnimationParameters
    {
        public FloatAnimationParameter velocity;
        public FloatAnimationParameter posX;
        public FloatAnimationParameter posY;
    }
}