using UnityEditor;

[CustomEditor(typeof(TestingAround)), CustomPreview(typeof(TestingAround))]
public class TestingAroundEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
    }
}
