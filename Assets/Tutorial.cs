using System.Collections;
using System.Collections.Generic;
using Avrahamy;
using Managers;
using TMPro;
using UI;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
        [SerializeField] 
        private float letterDelay = 0.1f;


        [SerializeField] 
        private float tutorialStartDelay;

        private TextMeshProUGUI _myText;
        
        public int letterCount = 0;

        public int LetterCount
        {
            get => letterCount;
            set => letterCount = value;
        }
        public string tutorialString;
    
    
        [SerializeField]
        private string[] tutorialsTexts;

        private int currentTutorial;

        private AudioSource _myAudio;

        [SerializeField]
        private GameObject _space;
        

        
    private void Start()
        {
            _myText = GetComponent<TextMeshProUGUI>();
            _myAudio = GetComponent<AudioSource>();
            StartCoroutine(StartTutorial(tutorialStartDelay));
        }

    IEnumerator WriteLetters( )
        {
            while (true)
            {
                if (CanvasManager.ActiveCanvas.Angle < 0)
                {
                    yield return new WaitForSeconds(letterDelay);
                }

                if (letterCount < tutorialString.Length && CanvasManager.ActiveCanvas.Angle >= 0)
                {
                    if (letterCount == 0)
                    {
                        _myAudio.Play();
                        CanvasManager.wordsToWrite++;
                    }

                    _myText.text += tutorialString[letterCount++];
                    yield return new WaitForSeconds(letterDelay);
                }

                if (letterCount >= tutorialString.Length)
                {
                    _myAudio.Stop();
                    _space.SetActive(true);
                    CanvasManager.wordsToWrite--;
                    break;
                }




            }

            if (WordsGameManager.Current.WordComplete && CanvasManager.wordsToWrite == 0)
            {
                DebugLog.Log(LogTag.HighPriority, "Word Completed - Should switch in cool way!!!!", this);
                WordsGameManager.SwitchToNextAvailableWord();
            }
        }

    public void TutorialContinue()
    {
        if (_myText.text.Length < tutorialString.Length)
        {
            return;
        }
        
        _space.SetActive(false);
        _myText.text = "";

        
        if (currentTutorial >= tutorialsTexts.Length)
        {
            return;
        }
        
        if (letterCount >= tutorialString.Length)
        {
            letterCount = 0;
            tutorialString = tutorialsTexts[currentTutorial++];
            StartCoroutine(WriteLetters());
        }
    }

    IEnumerator StartTutorial(float startDelay)
    {
        yield return new WaitForSeconds(startDelay);
        TutorialContinue();
    }
}
