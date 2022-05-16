using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TallBuilding : MonoBehaviour
{
    #region Inspector

    [Header("Hide Tall Objects")]
    [SerializeField]
    private Collider2D transparencyTrigger;

    [SerializeField]
    private SpriteRenderer mySprite;

    [SerializeField]
    [Range(0, 1)]
    private float transparency = 0.5f;

    [SerializeField]
    private float fadeTime = 0.5f;

    #endregion

    #region Private Fields

    private float _t;
    private int _direction = 1;
    private bool _notActive = true;
    private Color _normalColor;
    private Color _fadeColor;

    private Color _ColorA;
    private Color _ColorB;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        if (mySprite == null)
        {
            mySprite = GetComponentInParent<SpriteRenderer>();
        }

        if (transparencyTrigger == null)
        {
            transparencyTrigger = GetComponent<Collider2D>();
            transparencyTrigger.isTrigger = true;
        }

        _normalColor = mySprite.color;
        _fadeColor = new Color(_normalColor.r, _normalColor.g, _normalColor.b, transparency);
    }

    private void Update()
    {
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
            _notActive = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // (_ColorA, _ColorB) = (_fadeColor, _normalColor);
            _direction = -1;
            _notActive = false;
        }
    }

    #endregion
}