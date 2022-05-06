using System;
using System.Linq;
using UnityEngine;

namespace Mechanics.WordBase
{
    /// <summary>
    /// Activate scripts for events from prefab - doesnt require the object to be in the scene
    /// </summary>
    public class EventFromScriptTest : MonoBehaviour
    {
        #region Static methods For Events

        /// <summary>
        /// Debug: display all completed words.
        /// </summary>
        public static void DisplayAllCompleted()
        {
            var words = WordsGameManager.Completed.Aggregate("", (current, word) => current + $"{word}, ");
            Debug.Log(words);
        }

        #endregion

        private EventFromScriptTest _instance;

        // public EventFromScriptTest Instance => _instance;

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