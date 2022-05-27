using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class CanvasManager : MonoBehaviour
    {
        // TODO: Change this entire class to static class -no need for instance at all.
        public static CanvasManager CanvasManagerInstance { get; private set; }

        public static event EventHandler<bool> OnCanvasChange; 

        [SerializeField]
        private bool writingWord = false;

        public Pokedex ActiveCanvas { get; set; }
        public bool WritingWord
        {
            get => writingWord;
            set => writingWord = value;
        }
        
        public void OpenClose(InputAction.CallbackContext context)
        {
            if (!WritingWord && context.started)
            {
                OnCanvasChange?.Invoke(this, ActiveCanvas.IsOpen);
                ActiveCanvas.OpenClose();
            }
        }

        private void Awake()
        {
            if (CanvasManagerInstance != null)
            {
                Destroy(gameObject);
                return;
            }

            CanvasManagerInstance = this;
        }
    }
}