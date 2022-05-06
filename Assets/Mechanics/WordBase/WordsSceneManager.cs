using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mechanics.WordBase
{
    public class WordsSceneManager : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        internal List<MeaningfulWord> words = new List<MeaningfulWord>();

        #endregion

        #region Internal Values

        internal int MeaningFoundCount;

        // TODO: setter auto modulo
        internal int CurrentIndex;

        internal MeaningfulWord Current;

        #endregion

        #region Public Methods

        public void LoadScene(string scene)
        {
            SceneManager.LoadScene(scene);
        }

        #endregion
        
        #region MonoBehaviour

        private void Awake()
        {
            WordsGameManager.Instance = this;
            WordsGameManager.SwitchToNextAvailableWord();
            // WordsGameManager.Current = words[0];
            // TODO if empty, changing based on level, so on and so forth.
            // WordsGameManager.RegisterCurrentMeanings();
        }

        private void OnDisable()
        {
            WordsGameManager.Instance = null;
        }

        #endregion
    }
}