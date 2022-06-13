using System;
using System.Collections;
using System.Collections.Generic;
using Player_Control;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class OpenSceneSprite : MonoBehaviour
{
    [SerializeField]
    private TutorialObjects sprites;

    private SpriteRenderer _myRenderer;
    private PlayerInput _playerInput;
    private bool _first = true;
    private const string Controller = "Controller";

    private bool IsController => _playerInput != null && _playerInput.currentControlScheme == Controller;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _myRenderer = GetComponent<SpriteRenderer>();
        OnDevice();
        // _myRenderer.sprite = sprites.GetForScheme()
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started && _first)
        {
            _first = false;
            FindObjectOfType<MenuSpaceship>().GetComponent<Animator>().Play("Menu Leave");
        }
    }
    
    public void OnDevice()
    {
        if (_myRenderer == null)
        {
            _myRenderer = GetComponent<SpriteRenderer>();
        }
        var scheme = !IsController ? TutorialObjects.Schemes.KBM : TutorialObjects.Schemes.Controller;
        _myRenderer.sprite = sprites.GetForScheme(TutorialScheme.Tutorials.Interact, scheme);
    }
    
}