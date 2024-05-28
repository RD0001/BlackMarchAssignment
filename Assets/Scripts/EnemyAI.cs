using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IAI
{
    public GameObject enemyPrefab; // Prefab of the enemy unit
    public PlayerController playerController; // Reference to the player's movement script
    public float updateInterval = 1f; // Time interval between pathfinding updates

    private GameObject enemy; // Instance of the enemy unit
    private NavMeshAgent navMeshAgent; // NavMeshAgent component for pathfinding
    private float timeSinceLastUpdate = 0f; // Timer to track time since the last pathfinding update

    void Start()
    {
        SpawnEnemy(new Vector2Int(9, 9)); // Initial spawn position

        navMeshAgent = enemy.GetComponent<NavMeshAgent>();
    }

    void LateUpdate()
    {
        // Update the timer
        timeSinceLastUpdate += Time.deltaTime;

        // Perform pathfinding update at specified intervals
        if (timeSinceLastUpdate >= updateInterval)
        {
            Vector2Int playerPosition = playerController.GetCurrentGridPosition();
            MoveTowards(playerPosition);
            timeSinceLastUpdate = 0f; // Reset the timer
        }
    }

    // Spawns the enemy at the given grid position
    void SpawnEnemy(Vector2Int gridPosition)
    {
        Vector3 worldPosition = GridToWorld(gridPosition);
        enemy = Instantiate(enemyPrefab, worldPosition, Quaternion.identity);
    }

    // Moves the enemy towards the target position using NavMeshAgent
    public void MoveTowards(Vector2Int targetPosition)
    {
        Vector3 targetWorldPosition = GridToWorld(targetPosition);
        // Setting enemy destination
        navMeshAgent.SetDestination(targetWorldPosition);
    }

    // Converts a grid position to a world position
    Vector3 GridToWorld(Vector2Int gridPosition)
    {
        return new Vector3(gridPosition.x , 1f, gridPosition.y);
    }

    // Converts a world position to a grid position
    Vector2Int WorldToGrid(Vector3 worldPosition)
    {
        return new Vector2Int(Mathf.RoundToInt(worldPosition.x), Mathf.RoundToInt(worldPosition.z));
    }
}
