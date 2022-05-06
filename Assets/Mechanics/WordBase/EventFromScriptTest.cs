using System;
using System.Linq;
using UnityEngine;

namespace Mechanics.WordBase
{
    public class EventFromScriptTest : MonoBehaviour
    {
        public static void DisplayAllCompleted()
        {
            var words = WordsGameManager.Completed.Aggregate("", (current, word) => current + $"{word}, ");
            Debug.Log(words);
        }

        private EventFromScriptTest _instance;

        public EventFromScriptTest Instance => _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                return;
            }
            Destroy(gameObject);
        }
    }
}