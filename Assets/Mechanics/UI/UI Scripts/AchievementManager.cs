using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    /// <summary>
    /// Speed of fading in, the higher this is the faster the animation
    /// </summary>
    [SerializeField] private float fadingSpeed;

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

    private Image myImage;

    private float transparency = 0;
    
    TMP_Text myText;



    #endregion

    #region Private Methods

    private void Start()
    {
        GameObject marksHolder = GameObject.Find("QuestionMarksHolder");
        marksManager = marksHolder.GetComponent<QuestionMarksMaker>();
        myTransform = gameObject.GetComponent<RectTransform>();
        otherTransform = marksManager.questionMarksList[marksManager.NextQuestionMark - 1]
            .GetComponent<RectTransform>();
        statringScale = myTransform.sizeDelta;
        startingPosition = myTransform.position;
        myImage = gameObject.GetComponent<Image>();
        myText = GetComponentInChildren<TMP_Text>();
        otherTransform.GetComponent<Image>().enabled = false;
    }

    private void OnEnable()
    {
        StartCoroutine(AnimationDelay());
    }

    private void Update()
    { 
        if (t < 1f && !startAnimation)
        {
            t += Time.deltaTime * fadingSpeed;
            transparency = Mathf.Lerp(0f, 255f, t);
            myImage.color = new Color32(255, 255, 255, (byte)transparency);
            myText.color = new Color32(0, 0, 0, (byte)transparency);
            
        }
        if (t < 1f && startAnimation)
        {
            t += Time.deltaTime * animationSpeed;
            myTransform.sizeDelta = Vector3.Lerp(statringScale,
                otherTransform.sizeDelta, t);
            myTransform.position = Vector3.Lerp(startingPosition, otherTransform.position, t);
        }
    }

    IEnumerator AnimationDelay()
    {
        yield return new WaitForSeconds(animationStartDelay);

        myText.enabled = false;
        startAnimation = true;
        t = 0f;

    }


    #endregion


}