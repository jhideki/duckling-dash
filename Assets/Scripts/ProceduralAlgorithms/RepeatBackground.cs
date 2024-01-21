using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatBackground : MonoBehaviour
{

    public GameObject backgroundPrefab; // Reference to the background prefab
    public BoundaryData boundaryData; // Reference to the boundary data
    public int offsetX = 12;
    public int offsetY = 5;

    void Start()
    {
        RepeatBackgroundObjects();
        transform.position = new Vector3(boundaryData.bottomLeft.x + offsetX, boundaryData.bottomLeft.y + offsetY, 0);
    }

    void RepeatBackgroundObjects()
    {
        // Get the size of the background object
        float backgroundWidth = backgroundPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        float backgroundHeight = backgroundPrefab.GetComponent<SpriteRenderer>().bounds.size.y;

        // Calculate the boundaries
        Vector2Int bottomLeft = boundaryData.bottomLeft;
        Vector2Int topRight = boundaryData.topRight;
        int numRight = (topRight.x - bottomLeft.x) / (int)backgroundWidth;
        int numUp = (topRight.y - bottomLeft.y) / (int)backgroundHeight;

        // Create the background objects within the boundaries
        for (int i = 0; i < numRight; i++)
        {
            for (int j = 0; j < numUp; j++)
            {
                // Instantiate the background prefab
                GameObject backgroundObject = Instantiate(backgroundPrefab, new Vector3(i * backgroundWidth, j * backgroundHeight, 0), Quaternion.identity);
                Debug.Log("test");

                // Set the parent to keep the hierarchy clean
                backgroundObject.transform.parent = transform;
            }
        }
    }
}
