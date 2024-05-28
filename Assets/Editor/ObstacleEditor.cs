using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ObstacleEditor : Editor
{
    // Reference to the scriptable object containing obstacle data
    private ObstacleData obstacleData;

    private void OnGUI()
    {
        GUILayout.Label("Obstacle Grid Editor", EditorStyles.boldLabel);

        obstacleData = (ObstacleData)target;

        if (obstacleData != null)
        {
            for (int y = 0; y < 10; y++)
            {
                GUILayout.BeginHorizontal();
                for (int x = 0; x < 10; x++)
                {
                    // Convert 2D index to 1D index
                    int index = y * 10 + x;

                    // Toggle button for each grid cell
                    obstacleData.grid[index] = GUILayout.Toggle(obstacleData.grid[index], "");
                }
                GUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Save"))
            {
                // Mark the scriptable object as dirty to save changes
                EditorUtility.SetDirty(obstacleData);

                // Save changes to the scriptable object
                AssetDatabase.SaveAssets();
            }
        }
    }
}
