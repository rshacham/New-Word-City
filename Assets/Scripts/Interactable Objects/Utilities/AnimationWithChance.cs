using System;
using Avrahamy.Math;
using UnityEngine;

namespace Interactable_Objects
{
    /// <summary>
    /// Utility class to hold animation clip that has random chance 
    /// </summary>
    [Serializable]
    public class AnimationWithChance : RandomUtils.ClassWithChance
    {
        [SerializeField]
        public AnimationClip clip;

        [SerializeField]
        public bool connectedToWord = true;

        public static implicit operator AnimationClip(AnimationWithChance t)
        {
            return t.clip;
        }
    }
}