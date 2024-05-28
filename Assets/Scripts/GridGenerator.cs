using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class GridGenerator : MonoBehaviour
{
    // Reference to the cube prefab
    public GameObject cubePrefab;

    // Two dimensional array to store the grid of cubes
    public GameObject[,] grid = new GameObject[10, 10];

    // The NavMeshSurface component
    public NavMeshSurface navMeshSurface;

    void Start()
    {
        // Generate the grid when the game starts
        GenerateGrid();
        BakeNavMesh();
    }

    void GenerateGrid()
    {
        // Loop through each position in a 10x10 grid
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                // Instantiate a cube at the specified grid position
                GameObject cube = Instantiate(cubePrefab, new Vector3(x, 0, y), Quaternion.identity);

                // Get TileInfo component to store grid position
                cube.GetComponent<TileInfo>().SetPosition(x, y);

                // Store the cube in the grid array
                grid[x, y] = cube;
            }
        }
    }

    public void BakeNavMesh()
    {
        // Bake navmesh at runtime
        navMeshSurface.BuildNavMesh();
    }
}
