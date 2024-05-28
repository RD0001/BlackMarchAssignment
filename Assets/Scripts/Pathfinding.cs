using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public ObstacleData obstacleData; // Reference to the ScriptableObject containing obstacle data

    private int gridSize = 10; // Size of the grid

    // Finds a path from start to end using the A* algorithm
    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int end)
    {
        List<Node> openList = new List<Node>(); // Nodes to be evaluated
        HashSet<Node> closedList = new HashSet<Node>(); // Nodes already evaluated

        Node startNode = new Node(start); // Start node
        Node endNode = new Node(end); // End node

        openList.Add(startNode); // Add start node to the open list

        while (openList.Count > 0)
        {
            Node currentNode = openList[0];

            // Find the node with the lowest F cost
            foreach (var node in openList)
            {
                if (node.FCost < currentNode.FCost || node.FCost == currentNode.FCost && node.HCost < currentNode.HCost)
                {
                    currentNode = node;
                }
            }

            openList.Remove(currentNode); // Move current node from open to closed list
            closedList.Add(currentNode);

            // Path found
            if (currentNode.Position == endNode.Position)
            {
                return RetracePath(startNode, currentNode);
            }

            // Evaluate neighbors
            foreach (var neighbor in GetNeighbors(currentNode))
            {
                if (closedList.Contains(neighbor) || IsObstacle(neighbor.Position))
                {
                    continue; // Ignore neighbors that are obstacles or already evaluated
                }

                int newMovementCostToNeighbor = currentNode.GCost + GetDistance(currentNode, neighbor);
                if (newMovementCostToNeighbor < neighbor.GCost || !openList.Contains(neighbor))
                {
                    neighbor.GCost = newMovementCostToNeighbor;
                    neighbor.HCost = GetDistance(neighbor, endNode);
                    neighbor.Parent = currentNode;

                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                }
            }
        }

        return new List<Vector2Int>(); // No path found
    }

    // Returns the neighbors of a given node
    private List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;
                if (Mathf.Abs(x) == 1 && Mathf.Abs(y) == 1) continue; // No diagonal movement

                Vector2Int neighborPos = new Vector2Int(node.Position.x + x, node.Position.y + y);
                if (neighborPos.x >= 0 && neighborPos.x < gridSize && neighborPos.y >= 0 && neighborPos.y < gridSize)
                {
                    neighbors.Add(new Node(neighborPos));
                }
            }
        }

        return neighbors;
    }

    // Checks if a position is an obstacle
    private bool IsObstacle(Vector2Int position)
    {
        int index = position.y * gridSize + position.x;
        return obstacleData.grid[index];
    }

    // Returns the distance between two nodes
    private int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.Position.x - nodeB.Position.x);
        int dstY = Mathf.Abs(nodeA.Position.y - nodeB.Position.y);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }

    // Retraces the path from end node to start node
    private List<Vector2Int> RetracePath(Node startNode, Node endNode)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode.Position);
            currentNode = currentNode.Parent;
        }
        path.Reverse();
        return path;
    }

    // Node class used in the A* algorithm
    private class Node
    {
        public Vector2Int Position;
        public int GCost; // Cost from start to this node
        public int HCost; // Heuristic cost from this node to end
        public int FCost => GCost + HCost; // Total cost
        public Node Parent; // Parent node

        public Node(Vector2Int position)
        {
            Position = position;
        }
    }
}
