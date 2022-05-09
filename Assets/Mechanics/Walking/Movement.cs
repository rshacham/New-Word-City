using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    private const string Controller = "Controller";

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

    [SerializeField]
    private bool smoothing;

    private void OnValidate()
    {
        var angle = Mathf.Deg2Rad * ((180 - isometricAngle) / 2f);
        _isoVector = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        playerRigidBody = GetComponent<Rigidbody2D>();
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