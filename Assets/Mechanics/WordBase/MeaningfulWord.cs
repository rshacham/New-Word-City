using System;
using System.Collections.Generic;
using System.Linq;
using Mechanics.Object_Interactions.InteractionScripts;
using UnityEngine;

namespace Mechanics.WordBase
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

        [Space]
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
    }

    /// <summary>
    /// Meaning of a word.
    /// </summary>
    [Serializable]
    public class MeaningDescriptor
    {
        #region Inspector

        [SerializeField]
        [Multiline]
        private string meaning;

        [SerializeField]
        private InteractableObject linkedObject;

        #endregion

        #region Register Methods

        public void RegisterMeaning()
        {
            if (linkedObject != null && !Found)
            {
                linkedObject.OnInteractionEnd += FoundMeaning;
            }
        }

        public void UnRegisterMeaning()
        {
            if (linkedObject != null)
            {
                linkedObject.OnInteractionEnd -= FoundMeaning;
            }
        }

        #endregion

        #region Public Properties

        public bool Found { get; set; } = false;
        public string Meaning => meaning;

        public InteractableObject LinkedObject
        {
            get => linkedObject;
            set => linkedObject = value;
        }

        #endregion

        #region Private Methods

        private bool FoundMeaning()
        {
            if (Found)
            {
                return true;
            }

            Found = true;
            Debug.Log($"<color=magenta>Meaning Found: </color> {meaning}", LinkedObject);
            WordsGameManager.MeaningFoundCount++;
            UnRegisterMeaning();
            return Found;
        }

        #endregion
    }
}