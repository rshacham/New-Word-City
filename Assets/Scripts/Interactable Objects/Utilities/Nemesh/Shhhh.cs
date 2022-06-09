using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shhhh : MonoBehaviour
{
    #region Private Fields

    private AudioSource _audioSource;
    private InputAction _soundAction;

    #endregion

    #region Monobehaviour

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _soundAction = new InputAction("ActivateSound");
        _soundAction.AddBinding("<Keyboard>/numpadPlus").WithInteraction("hold");
        _soundAction.AddBinding("<Gamepad>/leftShoulder").WithInteraction("hold");
    }

    private void OnEnable()
    {
        _soundAction.Enable();
        _soundAction.performed += OnSound;
    }

    private void OnDisable()
    {
        _soundAction.performed -= OnSound;
        _soundAction.Disable();
    }

    #endregion

    #region Callbacks

    private void OnSound(InputAction.CallbackContext context)
    {
        _audioSource.enabled = !_audioSource.enabled;
    }

    #endregion
}
