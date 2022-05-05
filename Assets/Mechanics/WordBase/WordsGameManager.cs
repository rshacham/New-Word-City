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

        public static MeaningfulWord Current => _instance._current;

        #endregion

        #region Private Fields

        #region Static

        private static WordsGameManager _instance = null;

        #endregion

        #region Instance

        private MeaningfulWord _current;
        
        #endregion

        #endregion



        #region MonoBehaviour

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

        #endregion
    }
}