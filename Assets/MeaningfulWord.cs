using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "Word Meanings", menuName = "Word Meaning", order = 0)]
    public class MeaningfulWord : ScriptableObject
    {
        [SerializeField]
        private string word;

        [SerializeField]
        private List<MeaningDescriptor> meanings = new List<MeaningDescriptor>();

        public string Word => word;

        public List<MeaningDescriptor> Meanings => meanings;

        public int MeaningFoundCount
        {
            get { return Meanings.Sum(descriptor => descriptor.Found ? 1 : 0); }
        }
    }

    [Serializable]
    public class MeaningDescriptor
    {
        [SerializeField]
        [Multiline]
        private string meaning;

        public bool Found { get; set; } = false;
        public string Meaning => meaning;
    }
}