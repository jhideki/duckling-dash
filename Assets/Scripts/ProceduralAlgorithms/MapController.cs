using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class MapController : MonoBehaviour
{
    private AbstractGenerator abstractGenerator;
    private Map map1;
    private Map map2;

    public Transform player;
    public float cutoff = 10f;
    private BoxCollider2D boxCollider2D;
    private Vector3 positionCrossed;
    private bool inMap2;
    private int mapSide;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        Vector2Int firstPosition = new Vector2Int(0, 0);
        abstractGenerator = GetComponent<AbstractGenerator>();
        // Create first map object 
        map1 = abstractGenerator.Generate();
        // Load map data
        abstractGenerator.RunProceduralGeneration(map1, firstPosition);

        // Create second map object
        mapSide = UnityEngine.Random.Range(1, 5);
        Vector2Int secondPosition;
        int shiftAmountX;
        int shiftAmountY;
        switch (mapSide)
        {
            case 1:
                secondPosition = new Vector2Int(map1.boundaries.topRight.x, 0);
                break;
            case 2:
                secondPosition = new Vector2Int(map1.boundaries.topLeft.x, 0);
                break;
            case 3:
                secondPosition = new Vector2Int(0, map1.boundaries.topLeft.y);
                break;
            default:
                secondPosition = new Vector2Int(0, map1.boundaries.bottomLeft.y);
                break;
        }

        map2 = abstractGenerator.Generate();
        abstractGenerator.RunProceduralGeneration(map2, secondPosition);

        switch (mapSide)
        {
            case 1:
                shiftAmountX = map1.boundaries.topRight.x - map2.boundaries.topLeft.x;
                shiftAmountY = 0;
                break;
            case 2:
                shiftAmountX = map1.boundaries.topLeft.x - map2.boundaries.topRight.x;
                shiftAmountY = 0;
                break;
            case 3:
                shiftAmountX = 0;
                shiftAmountY = map1.boundaries.topLeft.y - map2.boundaries.bottomLeft.y;
                break;
            default:
                shiftAmountX = 0;
                shiftAmountY = map1.boundaries.bottomLeft.y - map2.boundaries.topLeft.y;
                break;
        }
        map2.ShiftMap(shiftAmountX, shiftAmountY);

        abstractGenerator.CreateCorridor(map1, mapSide, map2);
        // Draw map
        StartCoroutine(abstractGenerator.DrawMapObjects(map1));
        StartCoroutine(abstractGenerator.DrawMapObjects(map2));
        SetBoxColliderPerimeter(map2.boundaries);
    }

    void Update()
    {
        if (inMap2)
        {
            switch (mapSide)
            {
                case 1:
                    if (player.transform.position.x > positionCrossed.x + cutoff + map2.corridorLength)
                    {
                        GenerateNewMap();
                    }
                    break;
                case 2:
                    if (player.transform.position.x < positionCrossed.x - cutoff - map2.corridorLength)
                    {
                        GenerateNewMap();
                    }
                    break;
                case 3:
                    if (player.transform.position.y > positionCrossed.y + cutoff + map2.corridorLength)
                    {
                        GenerateNewMap();
                    }
                    break;
                default:
                    if (player.transform.position.y < positionCrossed.y - cutoff - map2.corridorLength)
                    {
                        GenerateNewMap();
                    }
                    break;
            }
        }
    }
    public void GenerateNewMap()
    {
        map1.ClearMap();
        StartCoroutine(abstractGenerator.FillCorridor(map2));
        map1 = map2;

        switch (mapSide)
        {
            // East
            case 1:
                mapSide = 2;
                break;
            // West
            case 2:
                mapSide = 1;
                break;
            // North
            case 3:
                mapSide = 4;
                break;
            // South
            default:
                mapSide = 3;
                break;
        }
        // Create second map object
        int previousMapSide = mapSide;

        mapSide = Random.Range(1, 5);
        if (mapSide == previousMapSide)
        {

            mapSide = (mapSide + 1) % 4;
        }

        Vector2Int secondPosition;
        int shiftAmountX;
        int shiftAmountY;
        switch (mapSide)
        {
            // East
            case 1:
                secondPosition = new Vector2Int(map1.boundaries.topRight.x, (map1.boundaries.topRight.y + map1.boundaries.bottomRight.y) / 2);
                break;
            // West
            case 2:
                secondPosition = new Vector2Int(map1.boundaries.topLeft.x, (map1.boundaries.topLeft.y + map1.boundaries.bottomLeft.y) / 2);
                break;
            // North
            case 3:
                secondPosition = new Vector2Int((map1.boundaries.topRight.x + map1.boundaries.topLeft.x) / 2, map1.boundaries.topLeft.y);
                break;
            // South
            default:
                secondPosition = new Vector2Int((map1.boundaries.bottomRight.x + map1.boundaries.bottomLeft.x) / 2, map1.boundaries.bottomLeft.y);
                break;
        }

        map2 = abstractGenerator.Generate();
        abstractGenerator.RunProceduralGeneration(map2, secondPosition);

        switch (mapSide)
        {
            case 1:
                shiftAmountX = map1.boundaries.topRight.x - map2.boundaries.topLeft.x;
                shiftAmountY = 0;
                break;
            case 2:
                shiftAmountX = map1.boundaries.topLeft.x - map2.boundaries.topRight.x;
                shiftAmountY = 0;
                break;
            case 3:
                shiftAmountX = 0;
                shiftAmountY = map1.boundaries.topLeft.y - map2.boundaries.bottomLeft.y;
                break;
            default:
                shiftAmountX = 0;
                shiftAmountY = map1.boundaries.bottomLeft.y - map2.boundaries.topLeft.y;
                break;
        }

        map2.ShiftMap(shiftAmountX, shiftAmountY);
        abstractGenerator.CreateCorridor(map1, mapSide, map2);
        StartCoroutine(abstractGenerator.DrawMapObjects(map2, map1));
        SetBoxColliderPerimeter(map2.boundaries);
    }
    private void SetBoxColliderPerimeter(Boundaries boundaries)
    {
        // Extract boundary values
        Vector2Int topLeft = boundaries.topLeft;
        Vector2Int topRight = boundaries.topRight;
        Vector2Int bottomLeft = boundaries.bottomLeft;
        Vector2Int bottomRight = boundaries.bottomRight;

        // Calculate size and position of the collider
        float width = Mathf.Abs(topRight.x - topLeft.x);
        float height = Mathf.Abs(topLeft.y - bottomLeft.y);

        float xPosition = (topLeft.x + topRight.x) / 2f;
        float yPosition = (topLeft.y + bottomLeft.y) / 2f;

        // Set BoxCollider2D properties
        boxCollider2D.size = new Vector2(width, height);
        boxCollider2D.offset = new Vector2(xPosition, yPosition);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inMap2 = true;
            positionCrossed = other.transform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inMap2 = false;
        }
    }
}
