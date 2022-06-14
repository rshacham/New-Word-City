using BitStrap;
using UnityEngine;

namespace Interactable_Objects.Utilities
{
    /// <summary>
    /// Set fade effect to tall objects that hide the player
    /// </summary>
    public class TallBuildingUtility : MonoBehaviour
    {
        #region Inspector

        [Header("Hide Tall Objects")]
        [SerializeField]
        [Tooltip("Trigger to start the transparency")]
        private Collider2D transparencyTrigger;

        // [SerializeField]
        // private bool usePeepingMaterial;

        [SerializeField]
        [Tooltip("The tall sprite")]
        private SpriteRenderer mySprite;

        [SerializeField]
        [Range(0, 1)]
        [Tooltip("The alpha value to go to")]
        private float transparency = 0.5f;

        [SerializeField]
        [Tooltip("Time to reach the transparency target")]
        private float fadeTime = 0.5f;

        [SerializeField]
        [TagSelector]
        [Tooltip("The tag used by the player")]
        private string playerTag = "Player";

        #endregion

        #region Private Fields

        private float _t;
        private int _direction = 1;
        private bool _notActive = true;
        private Color _normalColor;
        private Color _fadeColor;

        // private static readonly int PlayerBehind = Shader.PropertyToID("_PlayerBehind");
        // private static readonly int PlayerPos = Shader.PropertyToID("_PlayerPos");
        // private Material _sharedMaterial;

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            if (mySprite == null)
            {
                mySprite = GetComponentInParent<SpriteRenderer>();
            }

            // if (usePeepingMaterial)
            // {
            //     _sharedMaterial = mySprite.sharedMaterial;
            // }

            if (transparencyTrigger == null)
            {
                transparencyTrigger = GetComponent<Collider2D>();
            }

            transparencyTrigger.isTrigger = true;

            _normalColor = mySprite.color;
            _fadeColor = new Color(_normalColor.r, _normalColor.g, _normalColor.b, transparency);
        }

        private void Update()
        {
            // if (usePeepingMaterial)
            // {
            //     var playerPos = _sharedMaterial.GetVector(PlayerPos);
            //     DebugLog.Log(playerPos, this);
            //     mySprite.material.SetVector(PlayerPos, playerPos);
            // }
            if (_notActive)
            {
                return;
            }

            _t += _direction * Time.deltaTime / fadeTime;
            _t = Mathf.Clamp(_t, 0, 1);
            mySprite.color = Color.Lerp(_normalColor, _fadeColor, _t);
            _notActive = _t >= 1 || _t <= 0;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                // (_ColorA, _ColorB) = (_normalColor, _fadeColor);
                _direction = 1;
                // if (usePeepingMaterial)
                // {
                //     mySprite.material.SetInt(PlayerBehind, 1);
                // }
                // else
                // {
                //     _notActive = false;    
                // }
                _notActive = false;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag(playerTag))
            {
                // (_ColorA, _ColorB) = (_fadeColor, _normalColor);
                _direction = -1;
                // if (usePeepingMaterial)
                // {
                //     mySprite.material.SetInt(PlayerBehind, 0);
                // }
                // else
                // {
                //     _notActive = false;    
                // }
                _notActive = false;
            }
        }

        #endregion

        #region BitStrap

        [Button(true)]
        private void ManualTrigger()
        {
            _direction = -_direction;
            _notActive = false;
        }

        #endregion
    }
}