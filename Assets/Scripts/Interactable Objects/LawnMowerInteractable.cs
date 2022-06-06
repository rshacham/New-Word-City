using System;
using Avrahamy;
using BitStrap;
using Player_Control;
using UnityEngine;

namespace Interactable_Objects
{
    class LawnMowerInteractable : EventInteractable
    {
        [SerializeField]
        [TagSelector]
        private string stopperTag;

        [SerializeField]
        [TagSelector]
        private string grassTag = "Grass";

        public override bool SetInteraction(PlayerInteract other)
        {
            base.SetInteraction(other);
            Player = other;
            return false;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag(grassTag))
            {
                DebugLog.Log("Grass Cut", col);
                col.GetComponent<CutGrassInteractable>().TurnOff();
            }
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.collider.CompareTag(stopperTag))
            {
                DebugLog.Log("Reached Stopper: ", Color.green, col.collider);
                Interact();
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.collider.CompareTag(stopperTag))
            {
                DebugLog.Log("Left Stopper: ", Color.green, other.collider);
            }
        }
    }
}