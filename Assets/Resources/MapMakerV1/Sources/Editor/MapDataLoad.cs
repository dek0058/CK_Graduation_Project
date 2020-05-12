using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO; 
using System.Linq;
using System;

public class MapDataLoad : EditorWindow
{
    ScriptableObject[] Datas;
    List<string> DATANAMES = new List<string>();
    List<ScriptableObject> ddd = new List<ScriptableObject>();
    Vector2 scrollVector;
    int dataLength;

    void OnEnable()
    {
        Datas = Resources.LoadAll<ScriptableObject>("MapMakerV1/Data/Mapdata");
        foreach (var t in Datas)
        {
            DATANAMES.Add(t.name);
        }
        dataLength = Datas.Length;
    }

    public void OnRefresh()
    {
        OnEnable();
    }

    void OnGUI()
    {
                
        GUILayout.BeginHorizontal(GUI.skin.box);
        scrollVector = GUILayout.BeginScrollView(scrollVector);
            
        for (int i = 0; i < dataLength; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.HelpBox(Datas[i].name, MessageType.None);
            if (GUILayout.Button("Load", GUILayout.Width(40)))
            {
                MapMagEditor.mapDataName = Datas[i].name;
                MapMagEditor.mapDataSwitch = true;
                SceneView.RepaintAll();
            }
            if (GUILayout.Button("-", GUILayout.Width(25)))
            {
                DeleteData(Datas[i].name);
                break;
            }
            EditorGUILayout.EndHorizontal();
        }

        GUILayout.EndScrollView();
        GUILayout.EndHorizontal();

        if (GUILayout.Button("REFRESH MAP DATA"))
        {
            OnRefresh();
        }
    }

    void DeleteData(string name)
    {
        AssetDatabase.DeleteAsset("Assets/Resources/MapMakerV1/Data/Mapdata/"+name+".asset");
        OnEnable();
    }
}
