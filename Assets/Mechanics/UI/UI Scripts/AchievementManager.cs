using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    
    #region Inspector

    [SerializeField] private float animationStartDelay;

    [SerializeField] private float animationSpeed;

    #endregion
    #region Private Properties
    private float t = 0;

    private Vector3 statringScale;

    private Vector3 startingPosition;
    
    private QuestionMarksMaker marksManager;
    
    private bool startAnimation = false;
    
    private RectTransform myTransform;

    private RectTransform otherTransform;
    

    #endregion

    #region Private Methods
    private void Start()
    {
        GameObject marksHolder = GameObject.Find("QuestionMarksHolder");
        marksManager = marksHolder.GetComponent<QuestionMarksMaker>();
        myTransform = gameObject.GetComponent<RectTransform>();
        otherTransform = marksManager.questionMarksList[marksManager.NextQuestionMark].GetComponent<RectTransform>();
        statringScale = myTransform.sizeDelta;
        startingPosition = myTransform.position;
    }

    private void OnEnable()
    {
        StartCoroutine(AnimationDelay());
    }

    private void Update()
    {
        if (t < 1f && startAnimation)
        {
            t += Time.deltaTime * animationSpeed;
            myTransform.sizeDelta = Vector3.Lerp(statringScale,
                otherTransform.sizeDelta, t);
            myTransform.position = Vector3.Lerp(startingPosition, otherTransform.position, t);
        }
    }

    private void DoAnimation()
    {
        float time = 0;
        while (time < animationStartDelay)
        {
            print("hey");
            time += Time.deltaTime;
        }

        time = 0;
        print("hey");
    }

    #endregion
    
    



    IEnumerator AnimationDelay()
    {
        yield return new WaitForSeconds(animationStartDelay);
        startAnimation = true;

    }
}
