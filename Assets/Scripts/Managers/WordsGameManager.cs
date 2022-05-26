using System;
using System.Collections.Generic;
using Mechanics.WordBase;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// Static manager: To handle any action that need the words in the scene, completed words, and passing
    /// word data between scenes.
    /// </summary>
    public static class WordsGameManager
    {
        #region Static Properties

        public static EventHandler<MeaningDescriptor> OnMeaningFound;
        public static EventHandler<MeaningfulWord> OnWordSwitch;

        /// <summary>
        /// The current word that the we search for.
        /// </summary>
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

        /// <summary>
        /// List of all the words in the current scene.
        /// </summary>
        public static List<MeaningfulWord> Words => Active ? Instance.words : null;

        /// <summary>
        /// Number of Meanings found for this word.
        /// </summary>
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
                // TODO: refactor to method
                // TODO: work with active?
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

        /// <summary>
        /// Does the current scene have an active word?
        /// </summary>
        public static bool Active { get; private set; }

        /// <summary>
        /// Current Scene's local word manager
        /// </summary>
        public static WordsSceneManager Instance
        {
            get => _instance;
            internal set
            {
                _instance = value;
                Active = value != null;
            }
        }

        /// <summary>
        /// Set of all words completed
        /// </summary>
        public static HashSet<MeaningfulWord> Completed { get; } = new HashSet<MeaningfulWord>();

        #endregion

        #region Private Fields

        private static WordsSceneManager _instance;

        #endregion

        #region Public Methods

        /// <summary>
        /// Find the next word on the list that isn't completed, and switch to it, if there is any.
        /// </summary>
        public static void SwitchToNextAvailableWord()
        {
            if (!Active)
            {
                return;
            }
            // TODO: loop back to original using for loop with index modulo, not just to the end.
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
        
        /// <summary>
        /// switch to the word indicated by newWord index number
        /// </summary>
        /// <param name="newWord"></param>
        public static void SwitchToWord(int newWord)
        {
            if (!Active || newWord < -1 || newWord > Words.Count)
            {
                return;
            }
            //TODO: check if in completed first?
            if (Current != null)
            {
                UnRegisterCurrentMeanings(); // TODO: move both of this to public function of Current
                Current.ToolCanvas.SetActive(false);
            }
            if (Words[newWord].WordComplete || Completed.Contains(Words[newWord]))
            {
                MeaningFoundCount = 0; // TODO: not required?
                OnWordSwitch?.Invoke(Current, null);
                Current = null;
                return;
            }
            OnWordSwitch?.Invoke(Current, Words[newWord]);
            Current = Words[newWord]; // TODO: duplicated by the current index field, merge them.
            Instance.CurrentIndex = newWord;
            // TODO: move both below to public function of Current
            // Current.ToolCanvas.SetActive(true);
            RegisterCurrentMeanings();
            MeaningFoundCount = Current.MeaningFoundCount;
            Debug.Log($"New word: <color=blue>{Current}</color>");
        }

        /// <summary>
        /// Find specific word by name, and switch to it.
        /// </summary>
        /// <param name="word"></param>
        public static void SwitchToWord(string word)
        {
            if (!Active)
            {
                return;
            }

            var newWord = Words.FindIndex(x => x == word);
            SwitchToWord(newWord);
            // TODO: zone based words would use this, meaning we need to be able to have data if the word is finished?
        }


        /// <summary>
        /// Register all Meanings of the current word to their object's interactions.
        /// </summary>
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

        /// <summary>
        /// Unregister the current word's meanings from the interactions.
        /// </summary>
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