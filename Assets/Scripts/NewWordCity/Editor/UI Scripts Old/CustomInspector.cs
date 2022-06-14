using Managers;
using UnityEditor;
using UnityEngine;

namespace NewWordCity.Editor.UI_Scripts_Old
{
    [CustomEditor(typeof(QuestionMarksMaker))]
    public class CustomInspector : UnityEditor.Editor
    {
        [SerializeField] private Sprite sprite;
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            QuestionMarksMaker script = (QuestionMarksMaker) target;
            MeaningDescriptor hey = new MeaningDescriptor();
            if (GUILayout.Button("Create Object"))
            {
                script.CreateAchievement(null, hey);
            }
        }
    }
}
