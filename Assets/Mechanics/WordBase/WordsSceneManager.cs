using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mechanics.WordBase
{
    public class WordsSceneManager : MonoBehaviour
    {
        //TODO: index? scene object itself?
        private static Dictionary<string, List<MeaningfulWord>> _savedScenesData =
            new Dictionary<string, List<MeaningfulWord>>();

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
            // TODO: check on default getters?
            if (_savedScenesData.ContainsKey(SceneManager.GetActiveScene().name))
            {
                //TODO: move to MeaningfulWord as update method!
                var saved = _savedScenesData[SceneManager.GetActiveScene().name];
                for (int i = 0; i < words.Count; i++)
                {
                    for (int j = 0; j < words[i].Meanings.Count; j++)
                    {
                        words[i].Meanings[j].Found = saved[i].Meanings[j].Found;
                    }
                }
            }
            else
            {
                _savedScenesData[SceneManager.GetActiveScene().name] = words;
            }

            WordsGameManager.Instance = this;
            WordsGameManager.SwitchToNextAvailableWord();
            // WordsGameManager.Current = words[0];
            // TODO if empty, changing based on level, so on and so forth.
            // WordsGameManager.RegisterCurrentMeanings();
        }

        private void OnDisable()
        {
            _savedScenesData[SceneManager.GetActiveScene().name] = words;
            WordsGameManager.Instance = null;
        }

        #endregion
    }
}