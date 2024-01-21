using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundaries
{

    public Vector2Int topLeft;
    public Vector2Int topRight;
    public Vector2Int bottomLeft;
    public Vector2Int bottomRight;

    public Boundaries(Vector2Int topLeft, Vector2Int topRight, Vector2Int bottomLeft, Vector2Int bottomRight)
    {
        this.topLeft = topLeft;
        this.topRight = topRight;
        this.bottomLeft = bottomLeft;
        this.bottomRight = bottomRight;
    }

    public (Vector2Int, Vector2Int, Vector2Int, Vector2Int) GetBoundaries()
    {
        return (this.topLeft, this.topRight, this.bottomLeft, this.bottomRight);

    }
}
