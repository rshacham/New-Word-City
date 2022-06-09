using System;
using Avrahamy;
using Avrahamy.EditorGadgets;
using BitStrap;
using Player_Control;
using UnityEngine;

namespace Interactable_Objects
{
    class LawnMowerInteractable : EventInteractable
    {
        #region Inspector

        [SerializeField]
        [Tooltip("The tag the stoppers are marked with")]
        [TagSelector]
        private string stopperTag;

        [SerializeField]
        [Tooltip("The tag the grass is tagged with")]
        [TagSelector]
        private string grassTag = "Grass";

        [SerializeField]
        [Tooltip("Should cutting an amount of grass count as interactions, or stoppers only.")]
        private bool interactOnTrigger;

        [SerializeField]
        [ConditionalHide("interactOnTrigger")]
        [Tooltip("The number of triggers to cut before interaction (note cut that the mower starts above should count!)")]
        [Min(1)]
        private int triggerCount = 1;

        #endregion

        #region Private Fields

        private int _counter;

        #endregion

        #region EventInteractable

        public override bool SetInteraction(PlayerInteract other)
        {
            base.SetInteraction(other);
            Player = other;
            return false;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag(grassTag))
            {
                other.GetComponent<CutGrassUtility>().TurnOff();
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag(grassTag))
            {
                // DebugLog.Log("Grass Cut", col);
                col.GetComponent<CutGrassUtility>().TurnOff();
                if (interactOnTrigger)
                {
                    _counter++;
                    if (_counter == triggerCount)
                    {
                        Interact();
                        _counter = 0;
                    }
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.collider.CompareTag(stopperTag))
            {
                DebugLog.Log(LogTag.Gameplay, "Reached Stopper: ", col.collider);
                Interact();
                _counter = 0;
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.collider.CompareTag(stopperTag))
            {
                DebugLog.Log(LogTag.Gameplay,"Left Stopper: ",other.collider);
            }
        }

        #endregion
    }
}