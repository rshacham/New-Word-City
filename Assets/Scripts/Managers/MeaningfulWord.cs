using System;
using System.Collections.Generic;
using System.Linq;
using Interactable_Objects;
using Managers.UI;
using Mechanics.WordBase;
using UnityEngine;
using UnityEngine.Serialization;

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
        private GameObject toolCanvas;

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

        public GameObject ToolCanvas => toolCanvas;

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
        [Tooltip("A string to represent this meaning. should be readonly?")]
        [Multiline]
        public string meaning;
        
        [SerializeField] 
        public Sprite image;

        [SerializeField]
        public MeaningCanvasHolder meaningGameObject;

        [FormerlySerializedAs("linkedObject")]
        [SerializeField]
        [Tooltip("The interactable object that hides this meaning")]
        private List<InteractableObject> linkedObjects = new List<InteractableObject>();

        #endregion

        #region Register Methods

        /// <summary>
        /// Link this meaning to the objects interaction.
        /// </summary>
        public void RegisterMeaning()
        {
            if (Found)
            {
                return; // TODO: inform canvas that was already found?
            }

            foreach (var interactableObject in linkedObjects.Where(interactableObject => interactableObject != null))
            {
                // TODO: if single use interaction, have a way to re-enable it!
                interactableObject.OnInteractionEnd += FoundMeaning;
                interactableObject.OnInformChain();
            }
        }

        /// <summary>
        /// Unlink this meaning from the object.
        /// </summary>
        public void UnRegisterMeaning()
        {
            if (Found)
            {
                return;
            }

            foreach (var interactableObject in linkedObjects.Where(interactableObject => interactableObject != null))
            {
                // TODO: if single use interaction, have a way to re-enable it!
                interactableObject.OnInteractionEnd -= FoundMeaning;
                interactableObject.OnInformChain();
            }
        }

        #endregion

        #region Public Properties

        // TODO: add getter/setter for the word itself if we require this? and update in the manager?
        // public MeaningfulWord Word { get; set; }

        /// <summary>
        /// Did we find this meaning?
        /// </summary>
        public bool Found { get; set; } = false;

        /// <summary>
        /// Getter for the meaning itself.
        /// </summary>
        public string Meaning => meaning;

        /// <summary>
        /// Getter/Setter for the linked object - this allows us to still serialize the object if required.
        /// TODO: Active in scene check.
        /// </summary>
        public List<InteractableObject> LinkedObjects
        {
            get => linkedObjects;
            set => linkedObjects = value;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Function to be called when the interaction to find it has ended.
        /// </summary>
        /// <returns></returns>
        private void FoundMeaning(object sender, InteractableObject e)
        {
            if (Found)
            {
                return;
            }

            Debug.Log($"<color=magenta>Meaning Found: </color> {meaning}", e);
            // meaningGameObject.FoundMeaning(this, e);
            UnRegisterMeaning();
            Found = true;
            WordsGameManager.OnMeaningFound?.Invoke(e, this);
            WordsGameManager.MeaningFoundCount++;
        }
        #endregion
    }
}