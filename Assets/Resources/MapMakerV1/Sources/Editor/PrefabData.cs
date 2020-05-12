using UnityEngine;
using System;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

public class PrefabData : EditorWindow
{
    GameObject[] prefabs; 
    int prefabLength;
    Vector2 scrollPos;
    List<GameObject> CHECKGAMEOBJECT = new List<GameObject>();
    List<GameObject> LISTGAMEOBJECT = new List<GameObject>();
    Texture2D buttonTexture;

    void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        AssetPreview.SetPreviewTextureCacheSize(0);
        AssetPreview.SetPreviewTextureCacheSize(5000);
        CHECKGAMEOBJECT = Resources.LoadAll<GameObject>("USER_Prefabs").ToList();
        LISTGAMEOBJECT = CHECKGAMEOBJECT; 
        Repaint();
    }

    void OnGUI()
    {
        int columns = Mathf.FloorToInt(Screen.width / 85);
        GUI.color = new Color(0.3f, 0.3f, 0.3f); 
        GUILayout.BeginHorizontal(GUI.skin.box);
        scrollPos = GUILayout.BeginScrollView(scrollPos);
        GUILayout.BeginHorizontal();
        for (int i = 0; i < LISTGAMEOBJECT.Count; i++) 
        {

            if (LISTGAMEOBJECT[i] == null)
            {
                Refresh();
            }

            try
            {
                if (i % columns == 0)
                {
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                }

                if (AssetPreview.GetAssetPreview(LISTGAMEOBJECT[i]) == null)
                {
                    buttonTexture = AssetPreview.GetMiniThumbnail(LISTGAMEOBJECT[i]);
                }
                else
                {
                    buttonTexture = AssetPreview.GetAssetPreview(LISTGAMEOBJECT[i]);
                }

                GUI.color = new Color(1, 1, 1, 1.0f); 
                if (GUILayout.Button(buttonTexture, GUILayout.Width(80), GUILayout.Height(80)))
                {
                    MapMagEditor.RandomFrefabOn = false;
                    MapMag.selectPrefab = LISTGAMEOBJECT[i];
                    SceneView.RepaintAll();
                }
                GUI.color = Color.white;
                Repaint();
            }
            catch { }
        }

        GUILayout.EndHorizontal();
        GUILayout.EndHorizontal();
        GUI.color = Color.white;
        GUILayout.EndScrollView();
        GUILayout.BeginHorizontal();
               
        if (GUILayout.Button("REFRESH PREFABS DATA",GUILayout.MaxWidth(180))) 
        {
            OnEnable(); 
        }

        GUI.color = LISTGAMEOBJECT.Count >= 50 ? Color.yellow : Color.white;
        EditorGUILayout.HelpBox(" Current Buttons : " + LISTGAMEOBJECT.Count + "", MessageType.None);
        GUILayout.EndHorizontal();
    }
}
