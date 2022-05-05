using System;
using System.Collections.Generic;
using System.Linq;
using Mechanics.Object_Interactions.InteractionScripts;
using UnityEngine;

namespace Mechanics.WordBase
{
    [Serializable]
    public class MeaningfulWord
    {
        #region Inspector

        [SerializeField]
        private string word;

        [SerializeField]
        private List<MeaningDescriptor> meanings = new List<MeaningDescriptor>();

        #endregion

        #region Public Properties

        public string Word => word;

        public List<MeaningDescriptor> Meanings => meanings;

        public int MeaningFoundCount
        {
            get { return Meanings.Sum(descriptor => descriptor.Found ? 1 : 0); }
        }

        #endregion
    }

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
            Debug.Log($"<color=magenta>Meaning Found: </color> {meaning}");
            UnRegisterMeaning();
            return Found;
        }

        #endregion
    }
}