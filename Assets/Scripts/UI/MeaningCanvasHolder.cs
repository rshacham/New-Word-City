using System.Collections;
using Avrahamy;
using Interactable_Objects;
using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class MeaningCanvasHolder : MonoBehaviour
    {
        [SerializeField]
        private float delay = 0.1f;

        private TextMeshProUGUI _myText;
        private int _letterCount = 0;
        private string _meaningString;

        private AudioSource _myAudio;
        // private bool _startAnimation = false;


        public string MeaningString
        {
            get => _meaningString;
            set => _meaningString = value;
        }

        private void Start()
        {
            _myText = GetComponent<TextMeshProUGUI>();
            _myAudio = GetComponent<AudioSource>();
        }

        public void FoundMeaning(MeaningDescriptor sender, InteractableObject e)
        {
            _meaningString = sender.meaning;
            StartCoroutine(WriteLetters(false));
        }

        IEnumerator WriteLetters(bool reset)
        {
            while (true)
            {
                if (CanvasManager.ActiveCanvas.Angle < 0)
                {
                    yield return new WaitForSeconds(delay);
                }

                if (_letterCount < _meaningString.Length && CanvasManager.ActiveCanvas.Angle >= 0)
                {
                    if (_letterCount == 0)
                    {
                        _myAudio.Play();
                        CanvasManager.wordsToWrite++;
                    }

                    _myText.text += _meaningString[_letterCount++];
                    yield return new WaitForSeconds(delay);
                }

                if (_letterCount >= _meaningString.Length && !reset)
                {
                    _myAudio.Stop();
                    CanvasManager.wordsToWrite--;
                    break;
                }

                if (reset)
                {
                    yield return new WaitForSeconds(2f);
                    _myText.gameObject.SetActive(false);
                }


            }

            if (WordsGameManager.Current.WordComplete && CanvasManager.wordsToWrite == 0)
            {
                DebugLog.Log(LogTag.HighPriority, "Word Completed - Should switch in cool way!!!!", this);
                WordsGameManager.SwitchToNextAvailableWord();
            }
        }
    }
    
    
}