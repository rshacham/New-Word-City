using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionMarksMaker : MonoBehaviour
{
    #region Inspector
    
    /// <summary>
    /// Distance on the X axis, between each question mark
    /// </summary>
    [SerializeField] private int distanceBetweenMarks;
    
    /// <summary>
    /// Prefab for the question mark
    /// </summary>
    [SerializeField] private GameObject questionMark;

    /// <summary>
    /// A game object that will hold all the question marks
    /// </summary>
    [SerializeField] private GameObject questionMarksHolder;

    #endregion

    #region Private Properties



    #endregion

    #region Public Properties
    
    /// <summary>
    /// How many marks should we make?
    /// </summary>
    public int MarksAmount { get; set; } = 3; // default value, when integrating we'll take this value from the WordManager

    /// <summary>
    /// Index of the next free question mark
    /// </summary>

    public int NextQuestionMark { get; set; } = 0;
    
    /// <summary>
    /// A list that will hold all the question marks we instantiated, sorted by index
    /// </summary>
    public List<GameObject> questionMarksList = new List<GameObject>();

    #endregion

    #region Public Methods

    public void CreateMarks()
    {
        for (int i = 0; i < MarksAmount; i++)
        {
            GameObject newMark = (GameObject) Instantiate(questionMark, questionMarksHolder.transform, false);
            RectTransform newTransform = newMark.GetComponent<RectTransform>();
            var rectPosition = newTransform.position;
            rectPosition = new Vector3(rectPosition.x + (questionMarksList.Count * distanceBetweenMarks),
                rectPosition.y, rectPosition.z);
            newTransform.position = rectPosition;
            questionMarksList.Add(newMark);
        }
    }

    #endregion

    private void Start()
    {
        CreateMarks();
    }
}
