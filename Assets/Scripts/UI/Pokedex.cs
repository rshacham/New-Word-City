using Avrahamy;
using Interactable_Objects;
using Managers;
using UnityEngine;

namespace UI
{
    public class Pokedex : MonoBehaviour
    {
        private static readonly Vector3 ZAxis = Vector3.forward;

        [SerializeField]
        private MeaningCanvasHolder[] holders;

        [SerializeField]
        private float rotatingSpeed;
        [SerializeField]
        private float targetAngle;

        [SerializeField]
        private RectTransform pivot;

        private bool _isOpening = false;
        private bool _isOpen = false;
        private float _angle = 0;
        private RectTransform _pokedexTransform;


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

        // Start is called before the first frame update
        void Start()
        {
            _pokedexTransform = GetComponent<RectTransform>();
            WordsGameManager.OnMeaningFound += MeaningFound;
            CanvasManager.ActiveCanvas = this;
            if (holders == null)
            {
                // TODO: get holders by code
                DebugLog.LogError("Must have all holders!", this);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (_angle > targetAngle && !_isOpening)
            {
                _pokedexTransform.RotateAround(pivot.transform.position, ZAxis, Time.deltaTime * rotatingSpeed);
                _angle -= rotatingSpeed * Time.deltaTime;
            }

            if (_angle < 0 && _isOpening)
            {
                _pokedexTransform.RotateAround(pivot.transform.position, ZAxis, Time.deltaTime * -rotatingSpeed);
                _angle += rotatingSpeed * Time.deltaTime;
            }
        }

        public void OpenClose()
        {
            _isOpening = !_isOpening;
        }

        public void MeaningFound(object sender, MeaningDescriptor e)
        {
            // TODO: getting meaning index:
            var index = WordsGameManager.Current.Meanings.IndexOf(e);
            DebugLog.Log($"{e} at index {index}");
            holders[index].FoundMeaning(e, sender as InteractableObject);
        }
    }
}