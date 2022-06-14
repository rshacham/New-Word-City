using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Avrahamy;
using Avrahamy.Utils;
using Managers;
using Mechanics.WordBase;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField]
    private TMP_Text word;
    

    #endregion
    
    #region Private Fields
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
        WordsGameManager.OnMeaningFound += CreateAchievement;
        // WordsGameManager.OnWordSwitch += SetNewWord;
        //TODO: update when word change, see getter for this number?
        SetNewWord(this, WordsGameManager.Current);
        // print(Screen.currentResolution);
        var res = new Resolution
        {
            width = Screen.width,
            height = Screen.height,
            refreshRate = Screen.currentResolution.refreshRate
        };
        // DebugLog.Log(LogTag.Gameplay,res);
        _achievementsHolder = GameObject.Find("AchievementHolder");
    }

    private void SetNewWord(object sender, MeaningfulWord meaningfulWord)
    {
        MarksAmount = meaningfulWord.Meanings.Count;
        word.text = $"Find all of the meanings of the word {meaningfulWord}";
        CreateMarks();
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
            CreateMark(i);
        }
        // GetComponent<Canvas>().
    }

    /// <summary>
    /// Create a single question mark for the UI
    /// </summary>
    public void CreateMark(int i)
    {
        GameObject newMark = (GameObject) Instantiate(questionMark, questionMarksHolder.transform, false);
        string meaning = WordsGameManager.Current.Meanings[i].Meaning;
        meaning = ExceptWords(meaning, "Drop");
        // newMark.GetComponentInChildren<TMP_Text>().text = meaning;
        // var tmpText = newMark.GetComponentInChildren<TMP_Text>();
        
        // RectTransform newTransform = newMark.GetComponent<RectTransform>();
        // var rectPosition = newTransform.position;
        // rectPosition = new Vector3(rectPosition.x + (questionMarksList.Count * distanceBetweenMarks),
        //     rectPosition.y, rectPosition.z);
        // newTransform.position = rectPosition;
        questionMarksList.Add(newMark);
    }
    
    private string ExceptWords(string input, string except){
        string regexp = $"(?!{except})";
        return new Regex(regexp).Replace(input, "?");
    }
    
    /// <summary>                                         
    /// Create a single achievement UI object
    /// TODO - each achievement should have a different picture,
    /// and probably some form of text matching the definition the player discovered(for example "You dropped yourself!")
    /// </summary>                                        
    public void CreateAchievement(object sender, MeaningDescriptor meaning)
    {
        if (NextQuestionMark == questionMarksList.Count)
        {
            DebugLog.Log("You achieved all trophies!");
            return;
        }
        GameObject newObject = Instantiate(achievementObject, _achievementsHolder.transform, false);
        Image achievementImage = newObject.GetComponentInChildren<Image>();
        TMP_Text achievementText = newObject.GetComponentInChildren<TMP_Text>();
        // DebugLog.Log(achievementText);
        achievementImage.sprite = meaning.image;
        achievementText.text = meaning.Meaning;
        AchievementManager manager = newObject.GetComponent<AchievementManager>();
        manager.Index = WordsGameManager.Current.Meanings.IndexOf(meaning);
        newObject.SetActive(true);
        NextQuestionMark++;
        // questionMarksList[manager.Index].GetComponentInChildren<TMP_Text>().text = meaning.Meaning;
    }

    #endregion

}
