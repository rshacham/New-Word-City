using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mechanics.WordBase
{
    public class WordsSceneManager : MonoBehaviour
    {
        [SerializeField]
        internal List<MeaningfulWord> words = new List<MeaningfulWord>();

        internal int MeaningFoundCount;

        internal int CurrentIndex;

        internal MeaningfulWord Current;
        
        #region MonoBehaviour

        private void Awake()
        {
            WordsGameManager.Instance = this;
            WordsGameManager.Current = words[0];
            // TODO if empty, changing based on level, so on and so forth.
            WordsGameManager.RegisterCurrentMeanings();
        }

        private void OnDisable()
        {
            WordsGameManager.Instance = null;
        }

        #endregion
    }
}