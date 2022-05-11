using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    
    #region Inspector

    [SerializeField] private float animationStartDelay;

    #endregion
    #region Private Properties

    private QuestionMarksMaker marksManager;
    
    private bool startAnimation = false;
    

    #endregion

    #region Private Methods
    private void Awake()
    {
        GameObject marksHolder = GameObject.Find("QuestionMarksHolder");
        marksManager = marksHolder.GetComponent<QuestionMarksMaker>();
    }

    private void OnEnable()
    {
        StartCoroutine(AnimationDelay());
    }

    #endregion



    IEnumerator AnimationDelay()
    {
        yield return new WaitForSeconds(animationStartDelay);
        startAnimation = true;

    }
}
