using UnityEditor;
using UnityEngine;
namespace TopDownRPGDemo
{
    [CustomEditor(typeof(TerrainPlacer))]
    public class TerrainPlacerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // this runs the base code that this overrides
            base.OnInspectorGUI();
            if (GUILayout.Button("Place Terrains"))
            {
                PlaceTerrains((TerrainPlacer)target);
            }
        }

        public void PlaceTerrains(TerrainPlacer target)
        {
            int index = 0;
            for (int r = 0; r < target.rows; r++)
            {
                for (int c = 0; c < target.columns; c++)
                {
                    target.terrainPieces[index].position = new Vector3(
                        target.terrainSize * r,
                        0,
                        target.terrainSize * c);
                    index++;
                }
            }
        }

    }
}