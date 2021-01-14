namespace RogueBall
{
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(LimitsManager))]
    public class LimitsManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(10);

            if (GUILayout.Button("Regen"))
            {
                ((LimitsManager)target).RegenLimits();

                Undo.RegisterFullObjectHierarchyUndo(target, "Regen Limits");
            }
        }
    }
}