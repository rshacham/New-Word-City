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
        [TagSelector]
        private string stopperTag;

        [SerializeField]
        [TagSelector]
        private string grassTag = "Grass";

        [SerializeField]
        private bool interactOnTrigger;

        [SerializeField]
        [ConditionalHide("interactOnTrigger")]
        [Min(1)]
        private int triggerCount = 1;

        #endregion

        #region Private Fields

        private int _counter = 0;

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
                other.GetComponent<CutGrassInteractable>().TurnOff();
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag(grassTag))
            {
                // DebugLog.Log("Grass Cut", col);
                col.GetComponent<CutGrassInteractable>().TurnOff();
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
                DebugLog.Log("Reached Stopper: ", Color.green, col.collider);
                Interact();
                _counter = 0;
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.collider.CompareTag(stopperTag))
            {
                DebugLog.Log("Left Stopper: ", Color.green, other.collider);
            }
        }

        #endregion
    }
}