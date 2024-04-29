using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TestStruct)), CustomPreview(typeof(TestingAround))]
public class TestingAroundEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if(GUILayout.Button("Put collider to every Mesh Renderer"))
        {
            
        }
    }
}
