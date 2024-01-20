using UnityEngine;

[CreateAssetMenu(fileName = "BoundaryData", menuName = "ScriptableObjects/BoundaryData", order = 1)]
public class BoundaryData : ScriptableObject
{
    public Vector2Int topLeft;
    public Vector2Int topRight;
    public Vector2Int bottomLeft;
    public Vector2Int bottomRight;
}
