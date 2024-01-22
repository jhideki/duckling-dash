using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(AbstractGenerator), true)]
public class MapGeneratorEditor : Editor
{
    AbstractGenerator generator;

    private void Awake()
    {
        generator = (AbstractGenerator)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Generate Map"))
        {

            generator.Generate();

        }

        if (GUILayout.Button("Clear map"))
        {
            generator.ClearTiles();


        }
    }

}
