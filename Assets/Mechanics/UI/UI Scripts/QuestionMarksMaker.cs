using System;
using System.Collections;
using System.Collections.Generic;
using Avrahamy.Utils;
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

    
    /// <summary>
    /// A basic game object for achievement UI representation 
    /// </summary>
    [SerializeField] private GameObject achievementObject;
    

    #endregion
    
    #region Private Properties
    /// <summary>                                                  
    /// The game object that will holds the achievements           
    /// </summary>                                                 
    private GameObject _achievementsHolder;


    #endregion

    #region Public Properties
    
    /// <summary>
    /// How many marks should we make?
    /// default value is for testing, when integrating we'll take this value from the WordManager
    /// </summary>
    public int MarksAmount { get; set; } = 3;

    /// <summary>
    /// Index of the next free question mark
    /// </summary>

    public int NextQuestionMark { get; set; } = 0;
    
    /// <summary>
    /// A list that will hold all the question marks we instantiated, sorted by index
    /// </summary>
    public List<GameObject> questionMarksList = new List<GameObject>();

    #endregion
    
    #region Private Methods

    private void Start()
    {
        CreateMarks();
        print(Screen.currentResolution);
        _achievementsHolder = GameObject.Find("AchievementHolder");
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Create question marks according to the amount of definitions hidden inside the level.
    /// </summary>
    public void CreateMarks()
    {
        for (int i = 0; i < MarksAmount; i++)
        {
            CreateMark();
        }
    }

    /// <summary>
    /// Create a single question mark for the UI
    /// </summary>
    public void CreateMark()
    {
        GameObject newMark = (GameObject) Instantiate(questionMark, questionMarksHolder.transform, false);
        RectTransform newTransform = newMark.GetComponent<RectTransform>();
        var rectPosition = newTransform.position;
        rectPosition = new Vector3(rectPosition.x + (questionMarksList.Count * distanceBetweenMarks),
            rectPosition.y, rectPosition.z);
        newTransform.position = rectPosition;
        questionMarksList.Add(newMark);
    }
    
    /// <summary>                                         
    /// Create a single achievement UI object
    /// TODO - each achievement should have a different picture,
    /// and probably some form of text matching the definition the player discovered(for example "You dropped yourself!")
    /// </summary>                                        
    public void CreateAchievement()
    {
        if (NextQuestionMark == questionMarksList.Count)
        {
            Debug.Log("You achieved all trophies!");
            return;
        }
        Instantiate(achievementObject, _achievementsHolder.transform, false).SetActive(true);
        NextQuestionMark++;
    }

    #endregion

}
