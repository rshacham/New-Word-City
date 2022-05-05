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

        public static int MeaningFoundCount
        {
            get => _instance._meaningFoundCount;
            set
            {
                _instance._meaningFoundCount = value;
                if (Current != null && _instance._meaningFoundCount == Current.Meanings.Count)
                {
                    Current.WordComplete = true;
                    Debug.Log($"<color=blue>{Current}</color>: All Meanings Found!");
                    // TODO: switch Word Method based on game design
                    _instance._currentIndex++;
                    _instance._currentIndex %= Words.Count;
                    SwitchToWord(_instance._currentIndex);
                }
            }
        }

        #endregion

        #region Private Fields

        #region Static

        private static WordsGameManager _instance = null;

        #endregion

        #region Instance

        private int _meaningFoundCount;

        private int _currentIndex;

        private MeaningfulWord _current = null;

        #endregion

        #endregion

        #region Public Methods

        public static void SwitchToWord(int newWord)
        {
            if (newWord < -1 || newWord > Words.Count)
            {
                return;
            }

            if (Current != null)
            {
                MeaningFoundCount = 0;
                UnRegisterCurrentMeanings();
            }

            if (Words[newWord].WordComplete)
            {
                Current = null;
                return;
            }

            Current = Words[newWord];
            _instance._currentIndex = newWord;
            RegisterCurrentMeanings();
            MeaningFoundCount = Current.MeaningFoundCount;
            Debug.Log($"New word: <color=blue>{Current}</color>");
        }

        public static void SwitchToWord(string word)
        {
            var newWord = Words.FindIndex(x => x.Word == word);
            SwitchToWord(newWord);
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