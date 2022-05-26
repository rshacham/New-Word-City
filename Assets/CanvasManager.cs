using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class CanvasManager : MonoBehaviour
{
    public static CanvasManager _canvasManager;
    private Pokedex activeCanvas;
    
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
        activeCanvas.OpenClose();
    }
}
