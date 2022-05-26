using System;
using System.Collections;
using System.Collections.Generic;
using Avrahamy;
using UnityEngine;
using UnityEngine.InputSystem;


public class CanvasManager : MonoBehaviour
{
    public static CanvasManager _canvasManager;
    private Pokedex activeCanvas;
    public bool writingWord = false;
    
    public Pokedex ActiveCanvas
    {
        get => activeCanvas;
        set => activeCanvas = value;
    }
    private void Awake()
    {
        _canvasManager = this;
    }

    public void OpenClose(InputAction.CallbackContext context)
    {
        if (!writingWord)
        {
            activeCanvas.OpenClose();
        }
    }
}
