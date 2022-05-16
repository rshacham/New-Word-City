using System;
using System.Collections;
using System.Collections.Generic;
using Avrahamy.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    private const string Controller = "Controller";

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

    private Rigidbody2D playerRigidBody;

    private Vector2 _isoVector;

    private readonly Quaternion _moveAngle = Quaternion.Euler(0, 0, -45);

    private Vector2 _desiredVelocity;

    private Vector2 _currentDamp;

    private PlayerInput _playerInput;

    private Animator _playerAnimator;

    [SerializeField]
    private bool smoothing;

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
}