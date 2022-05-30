using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class CanvasManager : MonoBehaviour
    {
        #region Public Static

        public static CanvasManager CanvasManagerInstance { get; private set; }

        public static event EventHandler<bool> OnCanvasChange;

        public static Pokedex ActiveCanvas
        {
            get => CanvasManagerInstance._activeCanvas;
            set => CanvasManagerInstance._activeCanvas = value;
        }

        public static bool WritingWord
        {
            get => CanvasManagerInstance.writingWord;
            set => CanvasManagerInstance.writingWord = value;
        }

        public static int wordsToWrite = 0;

        public static int WordsToWrite
        {
            get => wordsToWrite;
            set => wordsToWrite = value;
        }

        public static int TutorialTextAmount

        {
            get => TutorialTextAmount;
            set => TutorialTextAmount = value;
        }
        
        
        public static int TutorialCurrentIndex
        {
            get => TutorialCurrentIndex;
            set => TutorialCurrentIndex = value;
        }

        #endregion

        #region Inspector

        [SerializeField]
        private bool writingWord;

        #endregion

        #region Private Fields

        private Pokedex _activeCanvas;

        #endregion

        #region Input Callbacks

        private void OpenClose(InputAction.CallbackContext context)
        {
            if (wordsToWrite == 0 && context.started)
            {
                OnCanvasChange?.Invoke(this, ActiveCanvas.IsOpen);
                ActiveCanvas.OpenClose();
            }
        }

        private void ContinueTutorial(InputAction.CallbackContext context)
        {
            if (true)
            {
                return;
            }
            if (context.started && TutorialCurrentIndex <= TutorialTextAmount)
            {
                
            }
        }

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            if (CanvasManagerInstance != null)
            {
                Destroy(gameObject);
                return;
            }

            CanvasManagerInstance = this;
        }
        
        

        #endregion
    }
}