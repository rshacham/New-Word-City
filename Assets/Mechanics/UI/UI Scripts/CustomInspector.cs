using Managers;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(QuestionMarksMaker))]
public class CustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        QuestionMarksMaker script = (QuestionMarksMaker) target;
        if (GUILayout.Button("Create Object"))
        {
            // script.CreateAchievement(this, WordsGameManager.Current);
        }
    }
}
