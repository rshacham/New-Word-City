using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Mechanics.WordBase
{
    public static class WordsGameManager
    {
        #region Static Properties

        public static MeaningfulWord Current
        {
            get => Active ? Instance.Current : null;
            internal set
            {
                if (Active)
                {
                    Instance.Current = value;
                }
            }
        }

        public static List<MeaningfulWord> Words => Active ? Instance.words : null;

        public static int MeaningFoundCount
        {
            get => Active ? Instance.MeaningFoundCount : -1;
            set
            {
                if (!Active)
                {
                    return;
                }

                Instance.MeaningFoundCount = value;
                // TODO: work with active
                if (Current != null && Instance.MeaningFoundCount == Current.Meanings.Count)
                {
                    Current.WordComplete = true;
                    Completed.Add(Current);
                    Debug.Log($"<color=blue>{Current}</color>: All Meanings Found!");
                    // TODO: switch Word Method based on game design
                    SwitchToNextAvailableWord();
                    // Instance.CurrentIndex++;
                    // Instance.CurrentIndex %= Words.Count;
                    // SwitchToWord(Instance.CurrentIndex);
                }
            }
        }

        public static bool Active { get; private set; }

        public static WordsSceneManager Instance
        {
            get => _instance;
            internal set
            {
                _instance = value;
                Active = value != null;
            }
        }

        public static HashSet<MeaningfulWord> Completed
        {
            get => _completed;
            set => _completed = value;
        }

        #endregion

        #region Private Fields

        private static WordsSceneManager _instance;

        private static HashSet<MeaningfulWord> _completed = new HashSet<MeaningfulWord>();

        #endregion

        #region Public Methods

        public static void SwitchToNextAvailableWord()
        {
            if (!Active)
            {
                return;
            }
            while (Instance.CurrentIndex < Words.Count)
            {
                if (Words[Instance.CurrentIndex].WordComplete || Completed.Contains(Words[Instance.CurrentIndex]))
                {
                    Instance.CurrentIndex++;
                    continue;
                }
                SwitchToWord(Instance.CurrentIndex);
                return;
            }
            Instance.CurrentIndex %= Words.Count;
            Active = false;
        }
        public static void SwitchToWord(int newWord)
        {
            if (!Active || newWord < -1 || newWord > Words.Count)
            {
                return;
            }
            //TODO: check if in completed

            if (Current != null)
            {
                MeaningFoundCount = 0;
                UnRegisterCurrentMeanings();
            }

            if (Words[newWord].WordComplete || Completed.Contains(Words[newWord]))
            {
                Current = null;
                return;
            }

            Current = Words[newWord];
            Instance.CurrentIndex = newWord;
            RegisterCurrentMeanings();
            MeaningFoundCount = Current.MeaningFoundCount;
            Debug.Log($"New word: <color=blue>{Current}</color>");
        }

        public static void SwitchToWord(string word)
        {
            if (!Active)
            {
                return;
            }

            var newWord = Words.FindIndex(x => x.Word == word);
            SwitchToWord(newWord);
        }


        public static void RegisterCurrentMeanings()
        {
            if (!Active)
            {
                return;
            }

            foreach (var descriptor in Current.Meanings)
            {
                descriptor.RegisterMeaning();
            }
        }

        public static void UnRegisterCurrentMeanings()
        {
            if (!Active)
            {
                return;
            }

            foreach (var descriptor in Current.Meanings)
            {
                descriptor.UnRegisterMeaning();
            }
        }

        #endregion
    }
}