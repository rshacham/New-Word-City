using System.Collections;
using Avrahamy;
using Player_Control;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class Tutorial : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        [Tooltip("Delay between the letters")]
        private float letterDelay = 0.1f;

        [SerializeField]
        private float tutorialStartDelay;

        [SerializeField]
        [Tooltip("The Tutorial objects used")]
        private TutorialObjects tutorialObjects;

        [SerializeField]
        [HideInInspector]
        private string[] tutorialsTexts;

        [SerializeField]
        [Tooltip("The tutorial text")]
        private TextMeshProUGUI myText;

        #endregion

        #region Public Properties

        #region Static

        public static Tutorial Instance { get; private set; }

        public static Movement PlayerMovement { get; set; }

        #endregion

        public Image ContinueImage { get; set; }

        public string[] TutorialsTexts
        {
            get => tutorialsTexts;
            set => tutorialsTexts = value;
        }

        public int letterCount = 0;

        public static int CurrentTutorial
        {
            get => Instance._currentTutorial;
            set => Instance._currentTutorial = value;
        }

        public int LetterCount
        {
            get => letterCount;
            set => letterCount = value;
        }

        public TextMeshProUGUI MyText
        {
            get => myText;
            set => myText = value;
        }

        #endregion

        #region Private Fields

        private int _currentTutorial;

        private string _tutorialString = "";

        private bool _isWriting = false;

        private bool _lastTutorial = false;

        private AudioSource _myAudio;

        private static TutorialObjects.Schemes Scheme =>
            PlayerMovement.IsController ? TutorialObjects.Schemes.Controller : TutorialObjects.Schemes.KBM;

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            WordsGameManager.Tutorial = true;
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
        }

        #endregion

        #region Coroutines

        IEnumerator WriteLetters()
        {
            while (true)
            {
                if (CanvasManager.ActiveCanvas.Angle < 0)
                {
                    yield return new WaitForSeconds(letterDelay);
                }

                if (letterCount < _tutorialString.Length && CanvasManager.ActiveCanvas.IsOpen)
                {
                    if (letterCount == 0)
                    {
                        _myAudio.Play();
                        CanvasManager.WordsToWrite++;
                        _isWriting = true;
                    }

                    myText.text += _tutorialString[letterCount++];
                    yield return new WaitForSeconds(letterDelay);
                }

                if (letterCount >= _tutorialString.Length)
                {
                    _myAudio.Stop();
                    myText.text = _tutorialString;
                    ContinueImage.sprite =
                        tutorialObjects.GetForScheme(TutorialScheme.Tutorials.Interact, Scheme);
                    ContinueImage.gameObject.SetActive(true);
                    CanvasManager.WordsToWrite--;
                    _isWriting = false;
                    break;
                }
            }

            if (_currentTutorial == TutorialsTexts.Length - 1
                // && WordsGameManager.Current is {WordComplete: false} &&
                // CanvasManager.WordsToWrite == 0
               )
            {
                _lastTutorial = true;
            }
        }

        #endregion

        #region Public Methods

        public void TutorialContinue()
        {
            if (_isWriting)
            {
                letterCount = _tutorialString.Length;
                return;
            }

            if (WordsGameManager.Instance.CurrentIndex == WordsGameManager.Instance.words.Count - 1 &&
                !GameManager.Shared.EndScenePlayed)
            {
                GameManager.Shared.EndScene();
                return;
            }

            if (_currentTutorial == TutorialsTexts.Length && !_isWriting // TODO: null check Current
                                                          && WordsGameManager.Current.WordComplete)
            {
                DebugLog.Log(LogTag.UI, 1);
                WordsGameManager.SwitchToNextAvailableWord();
                // TODO: uncomment both bellow to allow one press continue at end - Bug fix incoming
                // WordsGameManager.SwitchToNextAvailableWord();
                // _currentTutorial = _currentTutorial > TutorialsTexts.Length ? 0 : _currentTutorial;
                return;
            }

            if (_currentTutorial != TutorialsTexts.Length)
            {
                DebugLog.Log(LogTag.UI, 2);
                ContinueImage.gameObject.SetActive(false);
                myText.text = "";
            }

            if (_currentTutorial == TutorialsTexts.Length - 1 &&
                WordsGameManager.Current is {WordComplete: false} &&
                CanvasManager.WordsToWrite == 0 && _lastTutorial)
            {
                DebugLog.Log(LogTag.UI, 3);
                _lastTutorial = false;
                StaticEventsGameManager.OnPlayerShouldInteract(Instance, true);
                CanvasManager.ActiveCanvas.OpenClose();
                return;
            }

            if (_currentTutorial == TutorialsTexts.Length - 1 &&
                WordsGameManager.Current is {WordComplete: false})
            {
                DebugLog.Log(LogTag.UI, 4);
                return;
            }

            if (_currentTutorial >= TutorialsTexts.Length)
            {
                DebugLog.Log(LogTag.UI, 5);
                _currentTutorial = _currentTutorial > TutorialsTexts.Length ? 0 : _currentTutorial;
                return;
            }

            if (letterCount >= _tutorialString.Length)
            {
                DebugLog.Log(LogTag.UI, 6);
                letterCount = 0;
                _tutorialString = TutorialsTexts[_currentTutorial++];
                // if (_currentTutorial != TutorialsTexts.Length) 
                StartCoroutine(WriteLetters());
            }

            // if (_currentTutorial == TutorialsTexts.Length)
            // {
            //     WordsGameManager.SwitchToNextAvailableWord();
            //     _currentTutorial = _currentTutorial > TutorialsTexts.Length ? 0 : _currentTutorial;
            //     return;
            // }
        }


        public void LastTutorialContinue()
        {
            if (_isWriting)
            {
                letterCount = _tutorialString.Length;
                return;
            }
        }

        #endregion

        #region Public Static Methods

        // TODO: Return by reference?????

        public static GameObject CreateTutorial(Vector3 position, TutorialScheme.Tutorials type)
        {
            if (Instance == null)
            {
                return null;
            }

            DebugLog.Log(LogTag.UI, Scheme);
            return Instance.tutorialObjects.CreateTutorial(position, type, Scheme);
        }

        public static Vector3 Offset
        {
            get
            {
                if (Instance == null)
                {
                    return Vector3.zero;
                }

                return Instance.tutorialObjects.Offset;
            }
            set
            {
                if (Instance == null)
                {
                    return;
                }

                Instance.tutorialObjects.Offset = value;
            }
        }

        public static void RemoveTutorial(GameObject toRemove)
        {
            if (Instance == null)
            {
                return;
            }

            Instance.tutorialObjects.RemoveTutorial(toRemove);
        }

        #endregion
    }
}