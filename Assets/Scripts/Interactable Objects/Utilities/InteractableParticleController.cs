using System;
using Avrahamy;
using Avrahamy.Math;
using Managers;
using UnityEngine;

namespace Interactable_Objects
{
    /// <summary>
    /// Create Particle Effects From interacted objects
    /// </summary>
    public class InteractableParticleController : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        [Tooltip("Number of particles to emit")]
        private int emitCount = 5;

        #endregion

        #region Private Fields

        private ParticleSystem _particleSystem;

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            WordsGameManager.OnMeaningFound += SetParticles;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Create particle effect from sender's position.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="meaningDescriptor"></param>
        private void SetParticles(object sender, MeaningDescriptor meaningDescriptor)
        {
            if (sender is InteractableObject interactable) // TODO: is GameObject?
            {
                var shape = _particleSystem.shape;
                // var pos = interactable.GetComponent<Rigidbody2D>().position;
                var pos = interactable.transform.position;
                pos -= transform.position;
                pos.z = 0;
                shape.position = pos;
                // DebugLog.Log(shape.position, interactable);
                _particleSystem.Emit(emitCount);
            }
        }

        #endregion
    }
}