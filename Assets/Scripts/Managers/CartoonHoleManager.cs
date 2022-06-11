using System;
using Avrahamy;
using BitStrap;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class CartoonHoleManager : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        private float speed = 1f;

        [SerializeField]
        private Vector2 minMaxRadius = new Vector2(2, 50);

        [SerializeField]
        private PassiveTimer transitionDurationTimer;

        [SerializeField]
        [Tooltip("If true, the cartoon hole will be closed at the start of the game")]
        private bool startBlue;

        #endregion

        #region Static Events

        public static event EventHandler<CartoonHoleManager> TransitionEnd;

        private static void OnTransitionEnd(CartoonHoleManager e)
        {
            TransitionEnd?.Invoke(null, e);
        }

        #endregion

        #region Public Properties

        public int Moving { get; set; }

        public bool ChangeHole { get; set; } = true;

        public float Speed
        {
            get => speed;
            set => speed = value;
        }

        #endregion

        #region Private Fields

        private Image _myImage;
        private Material _sharedMaterial;
        private float _t = 1;
        private float _originalTransitionDuration;
        private Vector2 _originalMinMaxRadius;
        private bool _durationChanged = false;

        private static readonly int Radius = Shader.PropertyToID("_Radius");

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            _myImage = GetComponent<Image>();
            _sharedMaterial = _myImage.material;
            _sharedMaterial.SetFloat(Radius, minMaxRadius.y);
            _originalTransitionDuration = transitionDurationTimer.Duration;
            _originalMinMaxRadius = minMaxRadius;
            WordsGameManager.OnWordSwitch += OnWordSwitch;
            if (startBlue)
            {
                _t = 0;
                transitionDurationTimer.Start();
                Moving = 0;
                _t = Mathf.Clamp01(_t);
                _sharedMaterial.SetFloat(Radius, Mathf.Lerp(minMaxRadius.x, minMaxRadius.y, _t));
            }
        }

        private void Update()
        {
            if (transitionDurationTimer.IsSet && !transitionDurationTimer.IsActive)
            {
                transitionDurationTimer.Clear();
                Moving = 1;
            }

            if (Moving == 0)
            {
                return;
            }

            if (ChangeHole)
            {
                _t += speed * Moving * Time.deltaTime;
                _t = Mathf.Clamp01(_t);
                _sharedMaterial.SetFloat(Radius, Mathf.Lerp(minMaxRadius.x, minMaxRadius.y, _t));
            }

            // DebugLog.Log(_t, this);
            if (_t >= 1)
            {
                Moving = 0;
                DebugLog.Log(LogTag.UI,"Open", this);
            }
            
            else if (_t <= 0)
            {
                OnTransitionEnd(this);
                if (!GameManager.Shared.EndSceneIsOn)
                {
                    transitionDurationTimer.Start();
                    Moving = 0;

                    if (_durationChanged)
                    {
                        transitionDurationTimer.Duration = _originalTransitionDuration;
                        _durationChanged = false;
                    }
                }

                else
                {
                    ChangeHole = false;
                }
            }

        }

        #endregion

        #region Callback

        private void OnWordSwitch(object sender, MeaningfulWord e)
        {
            if (WordsGameManager.Tutorial)
            {
                WordsGameManager.Tutorial = false;
                return;
            }

            if (WordsGameManager.Instance.CurrentIndex != WordsGameManager.Instance.words.Count - 1)
            {
                transitionDurationTimer.Clear();
                Moving = Moving != 0 ? -Moving : _t >= 1 ? -1 : 1;
            }
            // _sharedMaterial.SetFloat("_StartTime", Time.time);
            // _sharedMaterial.SetInt("_Open", (_moving + 1) / 2);
        }

        #endregion

        #region Public Methods

        public void CloseCircle()
        {
            transitionDurationTimer.Clear();
            // transitionDurationTimer.EndTime = duration;
            Moving = Moving != 0 ? -Moving : _t >= 1 ? -1 : 1;
        }

        public void ChangeMinMax(int x, int y)
        {
            minMaxRadius = new Vector2(x, y);
        }

        public void ChangeDuration(float newDuration)
        {
            transitionDurationTimer.Duration = newDuration;
            _durationChanged = true;
        }

        [Button]
        private void ManualTrigger()
        {
            OnWordSwitch(null, null);
        }

        #endregion
    }
}