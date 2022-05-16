using System;
using Avrahamy;
using Interactable_Objects;
using UnityEngine;

namespace Interactable_Objects
{
    [RequireComponent(typeof(Collider2D))]
    public class ProximityHighlighter : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("This objects parent")]
        private InteractableObject parent;

        private Collider2D _myCollider2D;

        private void OnValidate()
        {
            if (parent != null)
            {
                ParentCollider = parent.Collider;
            }
        }

        private void Awake()
        {
            if (parent == null)
            {
                parent = GetComponentInParent<InteractableObject>();
                if (parent == null)
                {
                    DebugLog.LogError("Must have InteractableObject as parent!", this);
                }

                ParentCollider = parent.Collider;
            }
            else
            {
                transform.parent = parent.transform;
            }

            _myCollider2D = GetComponent<Collider2D>();
            _myCollider2D.isTrigger = true;
        }

        // private void OnTriggerEnter2D(Collider2D col)
        // {
        //     if (col.CompareTag("Player"))
        //     {
        //         Debug.Log("Should Highlight", parent);
        //     }
        // }
        //
        // private void OnTriggerExit2D(Collider2D other)
        // {
        //     if (other.CompareTag("Player"))
        //     {
        //         Debug.Log("Should UnHighlight", parent);
        //     }
        // }

        public Collider2D ParentCollider { get; private set; }

        public virtual void HighlightEvent(InteractableObject o)
        {
            DebugLog.Log("Highlight", Color.green, o);
        }

        public virtual void UnHighlightEvent(InteractableObject o)
        {
            DebugLog.Log("UnHighlight", Color.green, o);
        }
    }
}