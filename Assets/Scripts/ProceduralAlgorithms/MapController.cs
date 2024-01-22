using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    private AbstractGenerator abstractGenerator;
    private Map map1;
    private Map map2;

    // Start is called before the first frame update
    void Start()
    {
        Vector2Int firstPosition = new Vector2Int(0, 0);
        abstractGenerator = GetComponent<AbstractGenerator>();
        // Create first map object 
        map1 = abstractGenerator.Generate();
        // Load map data
        abstractGenerator.RunProceduralGeneration(map1, firstPosition);
        // Draw map
        abstractGenerator.DrawMapObjects(map1);

        // Create second map object
        map2 = abstractGenerator.Generate();
        Vector2Int secondPosition = new Vector2Int(map1.boundaries.topRight.x, 0);
        abstractGenerator.RunProceduralGeneration(map2, secondPosition);
        int shiftAmount = map1.boundaries.topRight.x - map2.boundaries.topLeft.x;
        Debug.Log(map1.boundaries.topRight.x);
        Debug.Log(map2.boundaries.topLeft.x);
        map2.ShiftMap(shiftAmount, 0);
        abstractGenerator.DrawMapObjects(map2);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
