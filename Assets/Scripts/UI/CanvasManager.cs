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

        public static int WordsToWrite { get; set; } = 0;

        public static int TutorialTextAmount { get; set; }


        public static int TutorialCurrentIndex { get; set; }

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
            if (WordsToWrite == 0 && context.started)
            {
                OnCanvasChange?.Invoke(this, ActiveCanvas.IsOpen);
                ActiveCanvas.OpenClose();
            }
        }

        public void ContinueTutorial(InputAction.CallbackContext context)
        {
            if (ActiveCanvas.TutorialHolder == null || !ActiveCanvas.IsOpen)
            {
                print("hi");
                return;
            }

            if (context.started && WordsToWrite == 0)
            {
                ActiveCanvas.TutorialHolder.TutorialContinue();
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