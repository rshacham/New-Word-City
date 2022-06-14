using System;
using System.Collections.Generic;
using System.Linq;
using Avrahamy;
using Interactable_Objects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Managers
{
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
        [HideInInspector]
        public Sprite image; // TODO: remove

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
        public bool Found { get; set; }

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

            DebugLog.Log($"<color=magenta>Meaning Found: </color> {meaning}", e);
            // meaningGameObject.FoundMeaning(this, e); // TODO: remove this call
            UnRegisterMeaning();
            Found = true;
            WordsGameManager.OnMeaningFound?.Invoke(e, this);
            WordsGameManager.MeaningFoundCount++;
        }

        #endregion

        #region Type Conversions

        public static implicit operator string(MeaningDescriptor t)
        {
            return t.ToString();
        }

        public override string ToString()
        {
            return Meaning;
        }

        #endregion
    }
}