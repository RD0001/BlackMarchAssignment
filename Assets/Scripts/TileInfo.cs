using UnityEngine;

public class TileInfo : MonoBehaviour
{
    // Variables to store the grid position
    public int gridX;
    public int gridY;

    public bool isWalkable = true;

    // Method to set the grid position
    public void SetPosition(int x, int y)
    {
        gridX = x;
        gridY = y;
    }
}
