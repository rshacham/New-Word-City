using Interactable_Objects;
using UnityEditor;
using UnityEngine;

namespace NewWordCity.Editor.Drawers
{
    [CustomEditor(typeof(InteractableObject), true)]
    public class InteractableDrawer : UnityEditor.Editor
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
