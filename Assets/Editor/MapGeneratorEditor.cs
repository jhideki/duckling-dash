using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(AbstractGenerator), true)]
public class MapGeneratorEditor : Editor
{
    AbstractGenerator generator;
    Map map;

    private void Awake()
    {
        generator = (AbstractGenerator)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Generate Map"))
        {

            map = generator.Generate();
            Vector2Int position = new Vector2Int(0, 0);
            generator.RunProceduralGeneration(map, position);
            generator.DrawMapObjects(map);

        }

        if (GUILayout.Button("Clear map"))
        {
            generator.ClearTiles();


        }
    }

}
