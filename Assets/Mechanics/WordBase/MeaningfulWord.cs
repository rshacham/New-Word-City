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
        [SerializeField]
        [Multiline]
        private string meaning;

        [SerializeField]
        private InteractableObject linkedObject;

        public bool Found { get; set; } = false;
        public string Meaning => meaning;

        public InteractableObject LinkedObject
        {
            get => linkedObject;
            set => linkedObject = value;
        }
    }
}