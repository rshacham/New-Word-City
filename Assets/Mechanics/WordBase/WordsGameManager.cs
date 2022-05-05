using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mechanics.WordBase
{
    public class WordsGameManager : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        private List<MeaningfulWord> words = new List<MeaningfulWord>();

        #endregion

        #region Static Properties

        public static MeaningfulWord Current
        {
            get => _instance._current;
            private set => _instance._current = value;
        }

        public static List<MeaningfulWord> Words => _instance.words;

        #endregion

        #region Private Fields

        #region Static

        private static WordsGameManager _instance = null;

        #endregion

        #region Instance

        private MeaningfulWord _current = null;

        #endregion

        #endregion

        #region Public Methods

        public static void SwitchToWord(string word)
        {
            var newWord = Words.Find(x => x.Word == word);
            if (newWord == null)
            {
                return;
            }

            if (Current != null)
            {
                UnRegisterCurrentMeanings();
            }

            Current = newWord;
            RegisterCurrentMeanings();
        }


        public static void RegisterCurrentMeanings()
        {
            foreach (var descriptor in Current.Meanings)
            {
                descriptor.RegisterMeaning();
            }
        }

        public static void UnRegisterCurrentMeanings()
        {
            foreach (var descriptor in Current.Meanings)
            {
                descriptor.UnRegisterMeaning();
            }
        }

        #endregion


        #region MonoBehaviour

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                Current = words[0]; // TODO if empty, changing based on level, so on and so forth.
                RegisterCurrentMeanings();
                DontDestroyOnLoad(gameObject);
                return;
            }

            Destroy(gameObject);
        }

        #endregion
    }
}