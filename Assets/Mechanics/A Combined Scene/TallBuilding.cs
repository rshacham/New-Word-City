using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TallBuilding : MonoBehaviour
{
    [Header("Hide Tall Objects")]
    [SerializeField]
    private Collider2D transparencyTrigger;

    [SerializeField]
    private SpriteRenderer mySprite;

    [SerializeField]
    [Range(0, 1)]
    private float transparency = 0.5f;

    private Color _oldValue;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            _oldValue = mySprite.color;
            mySprite.color = new Color(_oldValue.r, _oldValue.g, _oldValue.b, transparency);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            mySprite.color = _oldValue;
        }
    }
}