using System;
using Avrahamy;
using Avrahamy.Math;
using Managers;
using UnityEngine;

namespace Interactable_Objects
{
    public class InteractableParticleController : MonoBehaviour
    {
        [SerializeField]
        private int emitCount = 5;
        
        private ParticleSystem _particleSystem;
        private Vector3 _myPos;

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            WordsGameManager.OnMeaningFound += SetParticles;
        }

        private void SetParticles(object sender, MeaningDescriptor meaningDescriptor)
        {
            if (sender is InteractableObject interactable)
            {
                var shape = _particleSystem.shape;
                // var pos = interactable.GetComponent<Rigidbody2D>().position;
                var pos = interactable.transform.position;
                pos = pos - transform.position;
                pos.z = 0;
                shape.position = pos;
                DebugLog.Log(shape.position, interactable);
                _particleSystem.Emit(emitCount);
            }
        }
    }
}