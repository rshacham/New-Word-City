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
        
        public static int CurrentTutorial => Instance._currentTutorial;

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
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Update()
        {
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

            if (_currentTutorial > TutorialsTexts.Length)
            {
                WordsGameManager.SwitchToNextAvailableWord();
            }

            if (_currentTutorial == TutorialsTexts.Length && !_isWriting // TODO: null check Current
                                                          && WordsGameManager.Current.WordComplete)
            {
                _currentTutorial++;
                return;
            }

            if (_currentTutorial != TutorialsTexts.Length)
            {
                ContinueImage.gameObject.SetActive(false);
                myText.text = "";
            }

            if (_currentTutorial == TutorialsTexts.Length - 1 &&
                WordsGameManager.Current is {WordComplete: false} &&
                CanvasManager.WordsToWrite == 0 && _lastTutorial)
            {
                _lastTutorial = false;
                StaticEventsGameManager.OnPlayerShouldInteract(Instance, true);
                CanvasManager.ActiveCanvas.OpenClose();
                return;
            }

            if (_currentTutorial == TutorialsTexts.Length - 1 &&
                WordsGameManager.Current is {WordComplete: false})
            {
                return;
            }

            if (_currentTutorial >= TutorialsTexts.Length)
            {
                _currentTutorial = _currentTutorial > TutorialsTexts.Length ? 0 : _currentTutorial;
                return;
            }

            if (letterCount >= _tutorialString.Length)
            {
                letterCount = 0;
                _tutorialString = TutorialsTexts[_currentTutorial++];
                StartCoroutine(WriteLetters());
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