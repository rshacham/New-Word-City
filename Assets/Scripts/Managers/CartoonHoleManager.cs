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

        private static readonly int Radius = Shader.PropertyToID("_Radius");

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            _myImage = GetComponent<Image>();
            _sharedMaterial = _myImage.material;
            _sharedMaterial.SetFloat(Radius, minMaxRadius.y);
            _originalTransitionDuration = transitionDurationTimer.Duration;
            WordsGameManager.OnWordSwitch += OnWordSwitch;
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

            _t += speed * Moving * Time.deltaTime;
            _t = Mathf.Clamp01(_t);
            _sharedMaterial.SetFloat(Radius, Mathf.Lerp(minMaxRadius.x, minMaxRadius.y, _t));
            // DebugLog.Log(_t, this);
            if (_t >= 1)
            {
                Moving = 0;
            }
            else if (_t <= 0)
            {
                if (WordsGameManager.Tutorial)
                {
                    // GameManager._shared.ThrowPlayerOnWorld();
                    // WordsGameManager.Tutorial = false;
                }
                OnTransitionEnd(this);
                // transitionDurationTimer.EndTime = _originalTransitionDuration;
                transitionDurationTimer.Start();
                Moving = 0;
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
            transitionDurationTimer.Clear();
            Moving = Moving != 0 ? -Moving : _t >= 1 ? -1 : 1;
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

        [Button]
        private void ManualTrigger()
        {
            OnWordSwitch(null, null);
        }

        #endregion
    }
}