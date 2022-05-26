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
        private TMPro.TextMeshProUGUI  myText;
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
            myText = GetComponent<TMPro.TextMeshProUGUI >();
            print(myText);
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
                
                if (letterCount < meaningString.Length)
                {
                    myText.text += meaningString[letterCount++];
                    yield return new WaitForSeconds(delay);
                }
                else
                {
                    break;
                }
            }
        }
    }
}