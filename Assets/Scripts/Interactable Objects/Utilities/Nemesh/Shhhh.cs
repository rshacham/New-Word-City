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
    private InputAction _fpsAction;

    private float updateInterval = 0.5f; //How often should the number update
    private float _accum = 0.0f;
    private int _frames = 0;
    private float _timeLeft;
    private float _fps;
    private bool _showFps;

    private readonly GUIStyle _textStyle = new GUIStyle();

    #endregion

    #region Monobehaviour

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _soundAction = new InputAction("ActivateSound");
        _soundAction.AddBinding("<Keyboard>/numpadPlus").WithInteraction("hold");
        _soundAction.AddBinding("<Gamepad>/leftShoulder").WithInteraction("hold");
        _fpsAction = new InputAction("ShowFps");
        _fpsAction.AddBinding("<Keyboard>/numpadMinus").WithInteraction("hold");
        _fpsAction.AddBinding("<Gamepad>/rightShoulder").WithInteraction("hold");
    }

    private void Start()
    {
        _timeLeft = updateInterval;

        _textStyle.fontStyle = FontStyle.Bold;
        _textStyle.normal.textColor = Color.black;
    }

    private void OnEnable()
    {
        _soundAction.Enable();
        _soundAction.performed += OnSound;
        _fpsAction.Enable();
        _fpsAction.performed += OnFps;
    }

    private void OnDisable()
    {
        _soundAction.performed -= OnSound;
        _soundAction.Disable();
        _fpsAction.performed -= OnFps;
        _fpsAction.Disable();
    }


    private void Update()
    {
        _timeLeft -= Time.deltaTime;
        _accum += Time.timeScale / Time.deltaTime;
        ++_frames;

        // Interval ended - update GUI text and start new interval
        if (_timeLeft <= 0.0)
        {
            // display two fractional digits (f2 format)
            _fps = (_accum / _frames);
            _timeLeft = updateInterval;
            _accum = 0.0f;
            _frames = 0;
        }
    }

    private void OnGUI()
    {
        if (!_showFps)
        {
            return;
        }

        //Display the fps and round to 2 decimals
        GUI.Label(new Rect(5, 5, 100, 25), _fps.ToString("F2") + "FPS", _textStyle);
    }

    #endregion

    #region Callbacks

    private void OnSound(InputAction.CallbackContext context)
    {
        _audioSource.enabled = !_audioSource.enabled;
    }

    private void OnFps(InputAction.CallbackContext context)
    {
        _showFps = !_showFps;
    }

    #endregion
}