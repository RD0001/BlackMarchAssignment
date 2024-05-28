using Unity.AI.Navigation;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    // Reference to the Scriptable Object containing obstacle data
    public ObstacleData obstacleData;

    // Prefab to be instantiated for obstacles
    public GameObject obstaclePrefab;

    public GridGenerator gridGenerator;

    void Start()
    {
        // Generate obstacles on start
        GenerateObstacles();
    }

    void GenerateObstacles()
    {
        // Loop through the grid
        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                // Convert 2D index to 1D index
                int index = y * 10 + x;

                // Check if the grid cell is blocked by an obstacle
                if (obstacleData.grid[index])
                {
                    // Set position for obstacle instantiation
                    Vector3 position = new Vector3(x, 0.5f, y);

                    // Instantiate obstacle prefab at specified position
                    Instantiate(obstaclePrefab, position, Quaternion.identity);

                    gridGenerator.grid[x,y].gameObject.GetComponent<TileInfo>().isWalkable = false;
                    gridGenerator.grid[x, y].gameObject.GetComponent<NavMeshSurface>().defaultArea = 1;
                    gridGenerator.BakeNavMesh();
                }
            }
        }
    }
}
