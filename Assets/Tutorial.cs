using System;
using System.Collections;
using System.Collections.Generic;
using Avrahamy;
using Managers;
using Player_Control;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    private float letterDelay = 0.1f;

    [SerializeField]
    private float tutorialStartDelay;

    [SerializeField]
    private TutorialObjects tutorialObjects;

    [SerializeField]
    [HideInInspector]
    private string[] tutorialsTexts;

    [SerializeField]
    private TextMeshProUGUI myText;

    public static Tutorial Instance { get; private set; }

    public static Movement PlayerMovement { get; set; }

    public string[] TutorialsTexts
    {
        get => tutorialsTexts;
        set => tutorialsTexts = value;
    }

    public Image ContinueImage { get; set; }

    private string _tutorialString = "";

    private bool _isWriting = false;

    private bool _changeWord = false;

    public int letterCount = 0;

    public int LetterCount
    {
        get => letterCount;
        set => letterCount = value;
    }

    private int _currentTutorial;

    private AudioSource _myAudio;

    private static TutorialObjects.Schemes Scheme =>
        PlayerMovement.IsController ? TutorialObjects.Schemes.Controller : TutorialObjects.Schemes.KBM;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        // myText = GetComponent<TextMeshProUGUI>();
        _myAudio = GetComponent<AudioSource>();
        StartCoroutine(StartTutorial(tutorialStartDelay));
    }

    IEnumerator WriteLetters()
    {
        while (true)
        {
            if (CanvasManager.ActiveCanvas.Angle < 0)
            {
                yield return new WaitForSeconds(letterDelay);
            }

            if (letterCount < _tutorialString.Length && CanvasManager.ActiveCanvas.Angle >= 0)
            {
                if (letterCount == 0)
                {
                    _myAudio.Play();
                    _isWriting = true;
                    CanvasManager.wordsToWrite++;
                }

                myText.text += _tutorialString[letterCount++];
                yield return new WaitForSeconds(letterDelay);
            }

            if (letterCount >= _tutorialString.Length)
            {
                _myAudio.Stop();
                // space.SetActive(true);
                ContinueImage.sprite = tutorialObjects.GetForScheme(TutorialScheme.Tutorials.Interact, Scheme);
                ContinueImage.gameObject.SetActive(true);
                CanvasManager.wordsToWrite--;
                _isWriting = false;
                break;
            }
        }

        // if (WordsGameManager.Current.WordComplete && CanvasManager.wordsToWrite == 0 && _changeWord)
        // {
        //     DebugLog.Log(LogTag.HighPriority, "Word Completed - Should switch in cool way!!!!", this);
        //
        //     // WordsGameManager.SwitchToNextAvailableWord();
        // }
    }

    public void TutorialContinue()
    {
        if (_currentTutorial > TutorialsTexts.Length)
        {
            WordsGameManager.SwitchToNextAvailableWord();
        }

        if (_currentTutorial == TutorialsTexts.Length && !_isWriting
                                                      && WordsGameManager.Current.WordComplete)
        {
            _currentTutorial++;
            return;
        }

        if (_isWriting)
        {
            return;
        }

        if (_currentTutorial != TutorialsTexts.Length)
        {
            // space.SetActive(false);
            ContinueImage.gameObject.SetActive(false);
            myText.text = "";
        }

        if (_currentTutorial == TutorialsTexts.Length - 1 && !WordsGameManager.Current.WordComplete)
        {
            return;
        }


        if (_currentTutorial >= TutorialsTexts.Length)
        {
            return;
        }

        if (letterCount >= _tutorialString.Length)
        {
            letterCount = 0;
            _tutorialString = TutorialsTexts[_currentTutorial++];
            StartCoroutine(WriteLetters());
        }
    }

    IEnumerator StartTutorial(float startDelay)
    {
        yield return new WaitForSeconds(startDelay);
        TutorialContinue();
    }

    public static ref GameObject CreateTutorial(Vector3 position, TutorialScheme.Tutorials type)
    {
        if (Instance == null)
        {
            var instance = new GameObject("TutorialManager");
            instance.AddComponent<Tutorial>(); // todo: does this call awake by itself?
        }

        return ref Instance.tutorialObjects.CreateTutorial(position, type, Scheme);
    }

    public static Vector3 Offset
    {
        get
        {
            if (Instance == null)
            {
                var instance = new GameObject("TutorialManager");
                instance.AddComponent<Tutorial>(); // todo: does this call awake by itself?
            }

            return Instance.tutorialObjects.Offset;
        }
        set
        {
            if (Instance == null)
            {
                var instance = new GameObject("TutorialManager");
                instance.AddComponent<Tutorial>(); // todo: does this call awake by itself?
            }

            Instance.tutorialObjects.Offset = value;
        }
    }

    public static void RemoveTutorial()
    {
        if (Instance == null)
        {
            var instance = new GameObject("TutorialManager");
            instance.AddComponent<Tutorial>(); // todo: does this call awake by itself?
        }

        Instance.tutorialObjects.RemoveTutorial();
    }
}