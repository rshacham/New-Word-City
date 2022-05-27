using System;
using Avrahamy;
using Avrahamy.EditorGadgets;
using BitStrap;
using Managers;
using Player_Control;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MenuHighlight : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        private BoolAnimationParameter highlightParameter;

        [SerializeField]
        [HideInInspector]
        private Sprite menuSprite;

#if UNITY_EDITOR
        [Button]
        private void ManualTrigger()
        {
            _curState = !_curState;
            highlightParameter.Set(_myAnimator, _curState);
        }
#endif

        #endregion

        #region Private Fields

        private Image _myImage;
        private Animator _myAnimator;
#if UNITY_EDITOR
        private bool _curState;
#endif

        #endregion

        #region MonoBehaviour

        private void Start()
        {
            _myImage = GetComponent<Image>();
            _myAnimator = GetComponent<Animator>();
            WordsGameManager.OnMeaningFound += OnMeaningFound;
            WordsGameManager.OnWordSwitch += OnWordSwitch;
            CanvasManager.OnCanvasChange += OnCanvasChange;
        }

        #endregion

        #region CallBacks

        private void OnCanvasChange(object sender, bool e)
        {
            if (!e)
            {
                UnHighlight();
            }
        }

        private void OnWordSwitch(object sender, MeaningfulWord e)
        {
            DebugLog.Log(LogTag.UI, "Highlight Button - word switch");
            HighlightMenu();
        }

        private void OnMeaningFound(object sender, MeaningDescriptor e)
        {
            DebugLog.Log(LogTag.UI, "Highlight Button - meaning found");
            HighlightMenu();
        }

        #endregion

        #region Highlighters

        public void HighlightMenu()
        {
            if (CanvasManager.ActiveCanvas.IsOpen)
            {
                return;
            }

            highlightParameter.Set(_myAnimator, true);
#if UNITY_EDITOR
            _curState = true;
#endif
        }

        private void UnHighlight()
        {
            highlightParameter.Set(_myAnimator, false);
#if UNITY_EDITOR
            _curState = true;
#endif
        }

        #endregion
    }
}