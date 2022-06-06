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

        [SerializeField]
        [Tooltip("Should the first highlighting event emit particles")]
        private bool particlesOnFirst = true;

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
        private bool _firstTime = true;
        private GameObject _tutorialSprite;
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
            HighlightMenu();
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
            // DebugLog.Log(LogTag.UI, "Highlight Button - word switch");
            HighlightMenu();
        }

        private void OnMeaningFound(object sender, MeaningDescriptor e)
        {
            // DebugLog.Log(LogTag.UI, "Highlight Button - meaning found");
            HighlightMenu();
        }

        #endregion

        #region Highlighters

        public void HighlightMenu()
        {
            if (CanvasManager.CanvasManagerInstance == null)
            {
                return;
            }

            if (CanvasManager.ActiveCanvas.IsOpen)
            {
                return;
            }

            if (_firstTime && particlesOnFirst)
            {
                if (particlesOnFirst)
                {
                    StaticEventsGameManager.OnEmitParticles(this, Vector2.zero);
                }
            }

            var transform1 = transform;
            if (_tutorialSprite == null)
            {
                _tutorialSprite = Tutorial.CreateTutorial(
                    transform1.position + Tutorial.Offset,
                    TutorialScheme.Tutorials.SecondaryInteract);
                _tutorialSprite.transform.parent = transform1;
            }

            highlightParameter.Set(_myAnimator, true);
#if UNITY_EDITOR
            _curState = true;
#endif
        }

        private void UnHighlight()
        {
            if (_firstTime)
            {
                _firstTime = false;
            }

            if (_tutorialSprite != null)
            {
                Tutorial.RemoveTutorial(_tutorialSprite);
                _tutorialSprite = null;
            }

            highlightParameter.Set(_myAnimator, false);
#if UNITY_EDITOR
            _curState = true;
#endif
        }

        #endregion
    }
}