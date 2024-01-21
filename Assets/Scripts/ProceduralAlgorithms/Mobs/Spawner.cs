using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> mapObjects;

    public Spawner()
    {
        mapObjects = new List<GameObject>();
    }
    public void SpawnObjects(List<Vector2Int> spawnLocation, GameObject gameObj)
    {
        string objName = gameObj.name;
        GameObject parent = new GameObject(objName);
        foreach (var location in spawnLocation)
        {
            Vector3 spawnPosition = new Vector3(location.x, location.y, 0);
            GameObject newObj = Instantiate(gameObj, spawnPosition, Quaternion.identity);
            newObj.transform.SetParent(parent.transform);
        }
        mapObjects.Add(parent);
    }

    public void ClearObjects()
    {
        foreach (var obj in mapObjects)
        {
            DestroyImmediate(obj);
        }
        mapObjects.Clear();
    }

}
