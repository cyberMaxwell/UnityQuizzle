using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(QuestionDatabase))]
public class QuestionDBEditor : Editor
{
    private QuestionDatabase database;

    private void Awake()
    {
        database = (QuestionDatabase)target;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Remove"))
        {
            database.RemoveCurrentElement();
        }

        if (GUILayout.Button("Add"))
        {
            database.AddElement();
        }

        if (GUILayout.Button("<="))
        {
            database.GetPrev();
        }

        if (GUILayout.Button("=>"))
        {
            database.GetNext();
        }

        GUILayout.EndHorizontal();

        base.OnInspectorGUI();
    }
}

