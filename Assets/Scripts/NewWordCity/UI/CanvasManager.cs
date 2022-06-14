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

        public static int WordsToWrite { get; set; }

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
            if (WordsToWrite == 0 && context.started && ActiveCanvas != null && !ActiveCanvas.EndingCanvas)
            {
                ActiveCanvas.OpenClose();
            }
        }

        public static void OnCanvasChangeHandler()
        {
            OnCanvasChange?.Invoke(CanvasManagerInstance, ActiveCanvas.IsOpen);
        }

        public void ContinueTutorial(InputAction.CallbackContext context)
        {
            if (Pokedex.TutorialHolder == null || !ActiveCanvas.IsOpen || ActiveCanvas.EndingCanvas)
            {
                return;
            }

            if (context.started) //  TODO: && WordsToWrite == 0 ??
            {
                Pokedex.TutorialHolder.TutorialContinue();
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