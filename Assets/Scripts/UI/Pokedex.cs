using System;
using System.Collections;
using Avrahamy;
using Cinemachine;
using Interactable_Objects;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Pokedex : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        private bool openOnStart;
        [SerializeField]
        private bool stopPlayerInteractionOnStart = true;
        [SerializeField]
        private bool stopPlayerMovementOnStart = true;

        [SerializeField]
        private MeaningCanvasHolder[] holders;

        [SerializeField]
        [HideInInspector]
        private Tutorial tutorialHolder;

        [SerializeField]
        private string[] tutorialStrings;

        [SerializeField]
        private Image space;

        [SerializeField]
        private TextMeshProUGUI tutorialTextObject;

        public Tutorial TutorialHolder
        {
            get => Tutorial.Instance;
            // set => tutorialHolder = value;
        }


        [SerializeField]
        private Animator[] coinAnimators;

        [SerializeField]
        private Animator boardAnimator;

        //TODO - boardInt is equal to the current word index, we should take this from there
        [SerializeField]
        private int boardInt;

        [SerializeField]
        private float rotatingSpeed;

        [SerializeField]
        private float targetAngle;

        [SerializeField]
        private RectTransform pivot;

        #endregion

        #region Private Fields

        private static readonly Vector3 ZAxis = Vector3.forward;
        private bool _isOpening = false;
        private bool _isOpen = false;
        private float _angle = 0;
        private RectTransform _pokedexTransform;

        #endregion

        #region Public Properties

        public bool IsOpen
        {
            get => _isOpen;
            set => _isOpen = value;
        }

        public float Angle
        {
            get => _angle;
            set => _angle = value;
        }

        #endregion

        #region MonoBehaviour

        // Start is called before the first frame update
        void Start()
        {
            _pokedexTransform = GetComponent<RectTransform>();
            WordsGameManager.OnMeaningFound += MeaningFound;
            CanvasManager.ActiveCanvas = this;
            CanvasManager.WordsToWrite = 0; // TODO:
            Tutorial.Instance.TutorialsTexts = tutorialStrings;
            Tutorial.Instance.ContinueImage = space;
            Tutorial.Instance.MyText = tutorialTextObject;
            StartCoroutine(StartTutorial(0.2f));
            // TODO: Start open for all except fly::
            if (openOnStart && !CanvasManager.ActiveCanvas.IsOpen)
            {
                CanvasManager.ActiveCanvas.OpenClose();
            }

            if (stopPlayerInteractionOnStart)
            {
                StaticEventsGameManager.OnPlayerShouldInteract(this, false);
            }

            if (stopPlayerMovementOnStart)
            {
                Tutorial.PlayerMovement.EnableMovement = false;
                Tutorial.PlayerMovement.DesiredVelocity = Vector2.zero;
            }

            if (holders == null)
            {
                // TODO: get holders by code
                DebugLog.LogError("Must have all holders!", this);
            }

            foreach (var coin in coinAnimators)
            {
                coin.SetBool("Found", false);
            }

            boardAnimator.SetInteger("Word", boardInt);
        }

        // Update is called once per frame
        void Update()
        {
            // Debug.Log(_isOpen);
            if (_angle > targetAngle && _isOpening)
            {
                _pokedexTransform.RotateAround(pivot.transform.position, ZAxis, Time.deltaTime * -rotatingSpeed);
                _angle -= rotatingSpeed * Time.deltaTime;
            }

            if (_angle <= targetAngle && _isOpening)
            {
                IsOpen = true;
            }

            if (_angle < 0 && !_isOpening)
            {
                _pokedexTransform.RotateAround(pivot.transform.position, ZAxis, Time.deltaTime * rotatingSpeed);
                _angle += rotatingSpeed * Time.deltaTime;
            }

            if (_angle >= targetAngle && !_isOpening)
            {
                IsOpen = false;
            }
        }

        private void OnDisable()
        {
            WordsGameManager.OnMeaningFound -= MeaningFound;
        }

        #endregion

        #region Public Methods And Callbacks

        public void OpenClose()
        {
            DebugLog.Log("Open log", this);
            _isOpening = !_isOpening;
        }

        public void MeaningFound(object sender, MeaningDescriptor e)
        {
            var index = WordsGameManager.Current.Meanings.IndexOf(e);
            // DebugLog.Log($"{e} at index {index}");
            holders[index].FoundMeaning(e, sender as InteractableObject);
            coinAnimators[index].SetBool("Found", true);
        }

        public void SetAnimator()
        {
            boardAnimator.SetInteger("Word", boardInt);
        }

        IEnumerator StartTutorial(float startDelay)
        {
            while (!CanvasManager.ActiveCanvas.IsOpen)
            {
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(startDelay);
            Tutorial.Instance.TutorialContinue();
        }

        #endregion
    }
}