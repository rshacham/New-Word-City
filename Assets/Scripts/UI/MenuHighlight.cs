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
        [SerializeField]
        [AnimatorField("_myAnimator")]
        private BoolAnimationParameter highlightParameter;
        
        [SerializeField]
        [HideInInspector]
        private Sprite menuSprite; 
        
        private Image _myImage;
        private Animator _myAnimator;

        private void Start()
        {
            _myImage = GetComponent<Image>();
            _myAnimator = GetComponent<Animator>();
            WordsGameManager.OnMeaningFound += OnMeaningFound;
            WordsGameManager.OnWordSwitch += OnWordSwitch;
            CanvasManager.OnCanvasChange += OnCanvasChange;
        }

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

        public void HighlightMenu()
        {
            if (CanvasManager.ActiveCanvas.IsOpen)
            {
                return;
            }
            highlightParameter.Set(_myAnimator, true);
        }

        private void UnHighlight()
        {
            highlightParameter.Set(_myAnimator, false);
        }
    }
}