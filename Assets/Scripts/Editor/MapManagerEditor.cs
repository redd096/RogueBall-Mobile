namespace RogueBall
{
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(MapManager))]
    public class MapManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(10);

            if(GUILayout.Button("Refresh"))
            {
                ((MapManager)target).RefreshWaypoints();
            }
        }
    }
}