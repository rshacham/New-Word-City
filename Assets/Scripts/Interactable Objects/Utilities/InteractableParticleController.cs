using System;
using Avrahamy;
using Avrahamy.Math;
using BitStrap;
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
            // TODO: roi should use this for trampoline particles >>>
            StaticEventsGameManager.EmitParticles += (sender, vector2) => EmitAtPosition(vector2);
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
                // var pos = interactable.GetComponent<Rigidbody2D>().position;
                var pos = interactable.transform.position;
                pos -= transform.position;
                pos.z = 0;
                EmitAtPosition(pos);
            }
        }

        private void EmitAtPosition(Vector3 pos)
        {
            var shape = _particleSystem.shape;
            shape.position = pos;
            // DebugLog.Log(shape.position, interactable);
            _particleSystem.Emit(emitCount);
        }

        [Button]
        private void EmitButton()
        {
            if (_particleSystem == null)
            {
                _particleSystem = GetComponent<ParticleSystem>();
            }

            EmitAtPosition(Vector3.zero);
        }

        #endregion
    }
}