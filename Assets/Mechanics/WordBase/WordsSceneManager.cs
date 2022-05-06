using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mechanics.WordBase
{
    /// <summary>
    /// Manager to handle actions related to words in current scene.
    /// </summary>
    public class WordsSceneManager : MonoBehaviour
    {
        //TODO: index? scene object itself?
        // TODO: move to the static Manager?
        private static readonly Dictionary<string, List<MeaningfulWord>> SavedScenesData =
            new Dictionary<string, List<MeaningfulWord>>();

        #region Inspector

        [SerializeField]
        [Tooltip("List of words in this scene")]
        internal List<MeaningfulWord> words = new List<MeaningfulWord>();

        #endregion

        #region Internal Values

        /// <summary>
        /// Number of meanings found for current word.
        /// TODO wtf is internal?
        /// </summary>
        internal int MeaningFoundCount;

        // TODO: setter auto modulo
        /// <summary>
        /// Index of current word in the words array.
        /// </summary>
        internal int CurrentIndex;

        /// <summary>
        /// The current word.
        /// TODO: all the above 3 values are duplicated: with the index we can manage all of them in one.
        /// </summary>
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
            // TODO: check on default getters?
            if (SavedScenesData.ContainsKey(SceneManager.GetActiveScene().name))
            {
                //TODO: move to MeaningfulWord as update method!
                var saved = SavedScenesData[SceneManager.GetActiveScene().name];
                for (int i = 0; i < words.Count; i++)
                {
                    for (int j = 0; j < words[i].Meanings.Count; j++)
                    {
                        words[i].Meanings[j].Found = saved[i].Meanings[j].Found;
                    }
                }
                // TODO: save more information! can be done by creating a state class with all the
                //  serialized data! 
                // TODO: update MeaningCountFound?
            }

            WordsGameManager.Instance = this;
            WordsGameManager.SwitchToNextAvailableWord();
            // WordsGameManager.Current = words[0];
            // TODO if empty, changing based on level, so on and so forth.
            // WordsGameManager.RegisterCurrentMeanings();
        }

        private void OnDisable()
        {
            SavedScenesData[SceneManager.GetActiveScene().name] = words;
            WordsGameManager.Instance = null;
        }

        #endregion
    }
}