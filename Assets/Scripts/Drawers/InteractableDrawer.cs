using Interactable_Objects;
using UnityEditor;
using UnityEngine;

namespace Drawers
{
    [CustomEditor(typeof(InteractableObject), true)]
    public class InteractableDrawer : Editor
    {
        public override void OnInspectorGUI()
        {
            var io = target as InteractableObject;
            if (GUILayout.Button("Interact") && io != null)
            {
                io.Interact();
            }
            DrawDefaultInspector();
        }
    }
}
