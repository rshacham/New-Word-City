using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Avrahamy;
using UnityEngine;
using Managers;
using UI;

namespace Managers
{
    /// <summary>
    /// Data structure to represent a word with meanings
    /// </summary>
    [Serializable]
    public class MeaningfulWord
    {
        #region Inspector

        [SerializeField]
        [Tooltip("The word itself. dont change in runtime - readonly, without the proper attribute")]
        private string word;

        [SerializeField]
        [Tooltip("The Canvas for this words computer")]
        private Pokedex toolCanvas;

        [Space(2)]
        [SerializeField]
        [Tooltip("List of meanings of this word.")]
        private List<MeaningDescriptor> meanings = new List<MeaningDescriptor>();

        #endregion

        #region Public Properties

        /// <summary>
        /// All meanings found.
        /// </summary>
        public bool WordComplete { get; set; }

        /// <summary>
        /// Readonly for the word
        /// TODO: remove, auto conversion should be good enough?
        /// </summary>
        public string Word => word;

        /// <summary>
        /// Getter for the Meanings of this word
        /// </summary>
        public List<MeaningDescriptor> Meanings => meanings;

        /// <summary>
        /// How many meanings were found?
        /// </summary>
        public int MeaningFoundCount
        {
            get { return Meanings.Sum(descriptor => descriptor.Found ? 1 : 0); }
        }

        public Pokedex ToolCanvas => toolCanvas;

        #endregion

        #region Public Methods

        public void SetActiveCanvas(bool active)
        {
            // TODO: switch to nex with some delay:
            // basically if false: close then set active false.
            // if true: wait for false of previous then active true 
            if (toolCanvas != null)
            {
                if (!active && toolCanvas.IsOpen)
                {
                    toolCanvas.EndingCanvas = true;
                    PureCoroutines.StartRoutine(WaitClose());
                }
                else if (!active)
                {
                    toolCanvas.gameObject.SetActive(false);
                }
                else if (CanvasManager.CanvasManagerInstance != null &&
                         CanvasManager.ActiveCanvas is {isActiveAndEnabled: true, IsOpen: true})
                {
                    PureCoroutines.StartRoutine(WaitOpen());
                }
                else
                {
                    toolCanvas.gameObject.SetActive(true);
                }
                // toolCanvas.gameObject.SetActive(active);
            }
        }

        #endregion

        #region Type Conversions

        // Allow converting the word to string implicitly

        public static implicit operator string(MeaningfulWord t)
        {
            return t.ToString();
        }

        public override string ToString()
        {
            return word;
        }

        #endregion

        #region Object

        // Allow using the word in hashed sets/dictionaries
        public override bool Equals(object obj)
        {
            return obj != null && obj.ToString().Equals(word);
        }

        public override int GetHashCode()
        {
            return Word.GetHashCode();
        }

        #endregion

        #region Coroutines

        private IEnumerator WaitClose()
        {
            toolCanvas.OpenClose();
            yield return new WaitWhile(() => toolCanvas.Angle < 0);
            toolCanvas.gameObject.SetActive(false);
        }

        private IEnumerator WaitOpen()
        {
            yield return new WaitWhile(() => CanvasManager.ActiveCanvas.isActiveAndEnabled);
            toolCanvas.gameObject.SetActive(true);
        }

        #endregion
    }
}