using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityGenerator : MonoBehaviour
{
    
    #region Was just a make-shift thing for spawning blocks in

    public int numRows = 5;           // Number of rows of city blocks
    public int numColumns = 5;        // Number of columns of city blocks
    public float blockSize = 10f;     // Size of each city block

    public GameObject blockPrefab;    // Prefab representing a city block
    public Terrain terrain;           // Terrain to place buildings on
    public Transform cityParent;      // Parent transform for instantiated prefabs

    private List<GameObject> spawnedBuildings = new List<GameObject>();

    public void GenerateCity()
    {
        ClearCity();

        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numColumns; col++)
            {
                Vector3 position = new Vector3(col * blockSize, 0f, row * blockSize);

                // Calculate world position from terrain height data
                position.y = terrain.SampleHeight(position);

                GameObject block = Instantiate(blockPrefab, position, Quaternion.identity);
                block.transform.SetParent(cityParent);

                spawnedBuildings.Add(block);
            }
        }
    }

    void ClearCity()
    {
        foreach (GameObject building in spawnedBuildings)
        {
            DestroyImmediate(building);
        }
        spawnedBuildings.Clear();
    }

    #endregion

}
