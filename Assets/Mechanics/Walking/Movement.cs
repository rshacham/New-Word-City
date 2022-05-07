using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    private Rigidbody2D playerRigidBody;
    private bool movingRight;
    private bool movingLeft;
    private bool movingUp;
    private bool movingDown;

    [SerializeField] private float maxSpeed;
    [SerializeField] private float speedPerFrame;


    private void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
    }

    public void MoveRight(InputAction.CallbackContext context)
    {
        Vector2 oldVelocity = playerRigidBody.velocity;
        if (context.started && !movingLeft)
        {
            movingRight = true;
        }

        if (movingRight)
        {
            playerRigidBody.velocity = new Vector2(maxSpeed, oldVelocity.y);
        }

        if (context.canceled)
        {
            playerRigidBody.velocity = new Vector2(0, oldVelocity.y);
            movingRight = false;
        }
    }
    
    public void MovevLeft(InputAction.CallbackContext context)
    {
        Vector2 oldVelocity = playerRigidBody.velocity;
        if (context.started && !movingRight)
        {
            movingLeft = true;
        }

        if (movingLeft)
        {
            playerRigidBody.velocity = new Vector2(-maxSpeed, oldVelocity.y);
        }

        if (context.canceled)
        {
            playerRigidBody.velocity = new Vector2(0, oldVelocity.y);
            movingLeft = false;
        }
    }
    
    public void MoveUp(InputAction.CallbackContext context)
    {
        Vector2 oldVelocity = playerRigidBody.velocity;
        if (context.started && !movingDown)
        {
            movingUp = true;
        }

        if (movingUp)
        {
            playerRigidBody.velocity = new Vector2(oldVelocity.x, maxSpeed);
        }

        if (context.canceled)
        {
            playerRigidBody.velocity = new Vector2(oldVelocity.x, 0);
            movingUp = false;
        }
    }
    
    public void MoveDown(InputAction.CallbackContext context)
    {
        Vector2 oldVelocity = playerRigidBody.velocity;
        if (context.started && !movingUp)
        {
            movingDown = true;
        }

        if (movingDown)
        {
            playerRigidBody.velocity = new Vector2(oldVelocity.x, -maxSpeed);
        }

        if (context.canceled)
        {
            playerRigidBody.velocity = new Vector2(oldVelocity.x, 0);
            movingDown = false;
        }
    }


}

