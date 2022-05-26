using System;
using System.Collections;
using System.Linq;
using Avrahamy;
using Interactable_Objects;
using TMPro;
using UnityEngine;

namespace Managers.UI
{
    public class MeaningCanvasHolder : MonoBehaviour
    {
        private TMPro.TextMeshProUGUI myText;
        private int letterCount = 0;
        private string meaningString;
        private bool startAnimation = false;
        
        [SerializeField] private float delay = 0.1f;

        public string MeaningString
        {
            get => meaningString;
            set => meaningString = value;
        }

        private void Start()
        {
            myText = GetComponent<TMPro.TextMeshProUGUI>();
        }

        private void Update()
        {

        }

        public void FoundMeaning(MeaningDescriptor sender, InteractableObject e)
        {
            meaningString = sender.meaning;
            StartCoroutine(WriteLetters());
        }

        IEnumerator WriteLetters()
        {
            while (true)
            {
                if (CanvasManager._canvasManager.ActiveCanvas.Angle < 0)
                {
                    yield return new WaitForSeconds(delay);
                }

                if (letterCount < meaningString.Length && CanvasManager._canvasManager.ActiveCanvas.Angle >= 0)
                {
                    if (letterCount == 0)
                    {
                        CanvasManager._canvasManager.writingWord = true;
                    }
                    myText.text += meaningString[letterCount++];
                    yield return new WaitForSeconds(delay);
                }
                if (letterCount == meaningString.Length)
                {
                    CanvasManager._canvasManager.writingWord = false;
                    break;
                }

            }
        }
    }
}