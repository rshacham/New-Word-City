using System;
using Avrahamy.Math;
using UnityEngine;

namespace Interactable_Objects.Utilities
{
    /// <summary>
    /// Utility class to hold animation clip that has random chance 
    /// </summary>
    [Serializable]
    public class AnimationWithChance : RandomUtils.ClassWithChance
    {
        #region Inspector

        [SerializeField]
        public AnimationClip clip;

        [SerializeField]
        public bool connectedToWord = true;

        #endregion

        #region Implicit Convertions

        public static implicit operator AnimationClip(AnimationWithChance t)
        {
            return t.clip;
        }

        #endregion
    }
}