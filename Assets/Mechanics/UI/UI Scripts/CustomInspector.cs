using Mechanics.WordBase;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(QuestionMarksMaker))]
public class CustomInspector : Editor
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
