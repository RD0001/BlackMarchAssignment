using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject playerPrefab; // Prefab of the player unit
    public Pathfinding pathfinding; // Reference to the pathfinding script
    public ObstacleData obstacleData; // Reference to the obstacle data
    public float moveSpeed = 5f; // Speed at which the player moves

    private GameObject player; // Instance of the player unit
    private bool isMoving = false; // Flag to check if the player is moving

    void Start()
    {
        SpawnPlayer(new Vector2Int(0, 0)); // Initial spawn position
    }

    void Update()
    {
        if (isMoving) return; // Disable input while the player is moving

        if (Input.GetMouseButtonDown(0))
        {
            Vector2Int gridPosition = GetGridPositionUnderMouse();
            if (gridPosition != Vector2Int.zero && !IsObstacle(gridPosition))
            {
                StartCoroutine(MovePlayer(gridPosition)); // Start moving the player to the clicked position
            }
        }
    }

    // Spawns the player at the given grid position
    void SpawnPlayer(Vector2Int gridPosition)
    {
        Vector3 worldPosition = GridToWorld(gridPosition);
        player = Instantiate(playerPrefab, worldPosition, playerPrefab.transform.rotation);
    }

    // Coroutine to move the player along the path
    IEnumerator MovePlayer(Vector2Int targetGridPosition)
    {
        isMoving = true;
        List<Vector2Int> path = pathfinding.FindPath(WorldToGrid(player.transform.position), targetGridPosition);

        foreach (Vector2Int position in path)
        {
            Vector3 targetPosition = GridToWorld(position);
            while ((targetPosition - player.transform.position).sqrMagnitude > 0.01f)
            {
                player.transform.position = Vector3.MoveTowards(player.transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }
        isMoving = false;
    }

    // Gets the grid position under the mouse cursor
    Vector2Int GetGridPositionUnderMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            TileInfo tile = hit.collider.GetComponent<TileInfo>();
            if (tile != null)
            {
                return new Vector2Int(tile.gridX, tile.gridY);
            }
        }
        return Vector2Int.zero;
    }

    // Checks if a position is an obstacle
    bool IsObstacle(Vector2Int position)
    {
        int index = position.y * 10 + position.x;
        return obstacleData.grid[index];
    }

    // Converts a grid position to a world position
    Vector3 GridToWorld(Vector2Int gridPosition)
    {
        return new Vector3(gridPosition.x, 0.0f, gridPosition.y);
    }

    // Converts a world position to a grid position
    Vector2Int WorldToGrid(Vector3 worldPosition)
    {
        return new Vector2Int(Mathf.RoundToInt(worldPosition.x), Mathf.RoundToInt(worldPosition.z));
    }

    public Vector2Int GetCurrentGridPosition()
    {
        return WorldToGrid(player.transform.position);
    }
}
