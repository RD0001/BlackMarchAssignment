using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleData", menuName = "ScriptableObjects/ObstacleData", order = 1)]
public class ObstacleData : ScriptableObject
{
    // 10x10 grid represented as a single array
    public bool[] grid = new bool[100]; 
}
