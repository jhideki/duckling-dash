using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBackground : MonoBehaviour
{
    public GameObject parent;
    public void drawBackground(GameObject background, Boundaries boundaries)
    {

        parent = new GameObject();
        parent.name = "Background";
        if (background == null)
        {
            Debug.LogWarning("Background object is not assigned.");
            return;
        }

        SpriteRenderer bgRenderer = background.GetComponent<SpriteRenderer>();

        if (bgRenderer == null)
        {
            Debug.LogWarning("Background object does not have a SpriteRenderer component.");
            return;
        }

        // Calculate the size of the background sprite
        float bgWidth = bgRenderer.bounds.size.x;
        float bgHeight = bgRenderer.bounds.size.y;

        // Calculate the number of repetitions needed in both x and y directions
        int xRepetitions = Mathf.CeilToInt((float)(boundaries.topRight.x - boundaries.topLeft.x) / bgWidth) + 1;
        int yRepetitions = Mathf.CeilToInt((float)(boundaries.topLeft.y - boundaries.bottomLeft.y) / bgHeight) + 1;

        // Instantiate background objects in a grid pattern
        for (int i = 0; i < xRepetitions; i++)
        {
            for (int j = 0; j < yRepetitions; j++)
            {
                float xPos = boundaries.topLeft.x + i * bgWidth;
                float yPos = boundaries.topLeft.y - j * bgHeight;

                Vector3 spawnPosition = new Vector3(xPos, yPos, 0);
                GameObject obj = Instantiate(background, spawnPosition, Quaternion.identity);

                obj.transform.SetParent(parent.transform);
            }
        }
    }

    public void clearBackground()
    {
        if (parent != null)
        {
            DestroyImmediate(parent);
        }
        Destroy(this);
    }


}
