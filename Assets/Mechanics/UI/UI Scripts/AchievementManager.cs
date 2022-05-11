using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    
    #region Inspector
    /// <summary>
    /// Delay in seconds until the animation starts
    /// </summary>

    [SerializeField] private float animationStartDelay;

    
    /// <summary>
    /// Speed of animation, the higher this is the faster the animation
    /// </summary>
    [SerializeField] private float animationSpeed;

    #endregion
    #region Private Properties
    /// <summary>
    /// Current interpolation value
    /// </summary>
    private float t = 0;
    
    /// <summary>
    /// Starting scale of the achievement gameobject
    /// </summary>
    private Vector3 statringScale;

    /// <summary>
    /// Starting position of the achievement gameobject
    /// </summary>
    private Vector3 startingPosition;
    
    /// <summary>
    /// Script of the Question Marks Maker
    /// </summary>
    private QuestionMarksMaker marksManager;

    /// <summary>
    /// Animation will start only if this is true
    /// </summary>
    private bool startAnimation = false;
    
    /// <summary>
    /// Transform of the achievement gameobject
    /// </summary>
    private RectTransform myTransform;

    /// <summary>
    /// Transform of the question mark related to the achievement gameobject
    /// </summary>
    private RectTransform otherTransform;
    

    #endregion

    #region Private Methods
    private void Start()
    {
        GameObject marksHolder = GameObject.Find("QuestionMarksHolder");
        marksManager = marksHolder.GetComponent<QuestionMarksMaker>();
        myTransform = gameObject.GetComponent<RectTransform>();
        otherTransform = marksManager.questionMarksList[marksManager.NextQuestionMark - 1].GetComponent<RectTransform>();
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
