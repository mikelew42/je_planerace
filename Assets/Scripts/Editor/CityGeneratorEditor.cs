using Palmmedia.ReportGenerator.Core;
using UnityEditor;
using UnityEngine;
namespace TopDownRPGDemo
{
    [CustomEditor(typeof(CityGenerator))]
    public class CityGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // this runs the base code that this overrides
            base.OnInspectorGUI();
            if (GUILayout.Button("Generate City"))
            {
                GenerateCity((CityGenerator)target);
            }
        }

        public void GenerateCity(CityGenerator target)
        {
            target.GenerateCity();
        }

    }
}