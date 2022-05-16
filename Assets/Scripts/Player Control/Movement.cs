using UnityEngine;
using UnityEngine.InputSystem;

namespace Player_Control
{
    public class Movement : MonoBehaviour
    {
        // TODO: serialize? even as hidden?
        private const string Controller = "Controller";

        #region Inspector

        [SerializeField]
        private bool enableMovement = true;

        [SerializeField]
        private float maxSpeed;

        [SerializeField]
        private float maxAcceleration;

        [SerializeField]
        private float smoothedTime = 1f;

        [SerializeField]
        private bool moveIsometric;

        [SerializeField]
        [Range(0, 180)]
        private int isometricAngle = 120;

        [SerializeField]
        private bool smoothing;

        #endregion

        #region Private Fields

        private Rigidbody2D playerRigidBody;

        private Vector2 _isoVector;

        private Vector2 _desiredVelocity;

        private Vector2 _currentDamp;

        private PlayerInput _playerInput;

        private Animator _playerAnimator;

        private readonly Quaternion _moveAngle = Quaternion.Euler(0, 0, -45);

        #endregion
        
        #region Public Properties

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
            playerRigidBody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            var speed = playerRigidBody.velocity.magnitude;
            var angle = Vector2.SignedAngle(new Vector2(1f, 0f), playerRigidBody.velocity.normalized);
            _playerAnimator.SetFloat("Velocity", speed);
            if (speed > 0.02f)
            {
                _playerAnimator.SetFloat("Angle", angle);
                _playerAnimator.SetFloat("PosX", playerRigidBody.velocity.normalized.x);
                _playerAnimator.SetFloat("PosY", playerRigidBody.velocity.normalized.y);
            }
        }

        private void FixedUpdate()
        {
            if (smoothing)
            {
                playerRigidBody.velocity = Vector2.SmoothDamp(
                    playerRigidBody.velocity,
                    _desiredVelocity,
                    ref _currentDamp,
                    smoothedTime,
                    maxAcceleration,
                    Time.fixedDeltaTime
                );
            }
            else
            {
                playerRigidBody.velocity = _desiredVelocity;
            }
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
            if (_playerInput.currentControlScheme != Controller)
            {
                if (moveIsometric)
                {
                    movementVector = _moveAngle * movementVector;
                }

                movementVector *= _isoVector;
            }

            _desiredVelocity = movementVector * maxSpeed;
        }

        #endregion
    }
}