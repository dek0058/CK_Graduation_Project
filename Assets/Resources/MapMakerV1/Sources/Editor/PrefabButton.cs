using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class PrefabButton : EditorWindow
{

    PrefabDatas prefabDatas; 
    Object FOLDER;
    
    Vector2 MAINSCROLLVIEW;
    Vector2 SCROLLVIEWPREVIEW;
    Vector3 ADDPOS;
    Vector3 POS;

    FileAttributes CHECKDIR;
    Texture2D[] PREFABTEXTURE;
    Texture2D buttonTexture;
    Texture2D[] thumnailArray;

    Rect rect;

    GameObject PREFAB;
    GameObject create;
    GameObject[] SELECTObject;
    GameObject OBJJ = null;
    DirectoryInfo DIRINFO;
    FileInfo[] PREFABINFO;

    Dictionary<string, FileInfo> DICPREFABSDATA = new Dictionary<string, FileInfo>();

    List<Texture2D> PREFABTEXTURELIST = new List<Texture2D>();
    List<GameObject> PREFABS = new List<GameObject>();
    List<GameObject> PREFABSCHECK = new List<GameObject>();// 중복체크
    List<GameObject> GAMEOBJECTNameCheck = new List<GameObject>();
    

    bool shouldLoadPreview = false;
    bool tmpBreakVar = false;
    int loadingIndex = -1;
    byte[] bytes;
    string prefabName;
    string folderPath;
    string[] DirpathName;
    string returnText;
    string tagName;
    string GameObjectName = "GameObject Name";

    void OnEnable()
    {
        PREFABS.Clear();
        PREFABTEXTURELIST.Clear();
        DICPREFABSDATA.Clear();
    }

    void OnGUI()
    {
        GUI.color = Color.gray;
        EditorGUILayout.HelpBox("Customized Prefab Palette", MessageType.None);
        GUI.color = Color.white;
        EditorGUILayout.Space();
        FOLDER = (Object)EditorGUILayout.ObjectField("Input Folder", FOLDER, typeof(Object), false); 
        if (GUI.changed && FOLDER != null)
        {
            folderPath = AssetDatabase.GetAssetPath(FOLDER);
            CHECKDIR = File.GetAttributes(folderPath);

            if (CHECKDIR == FileAttributes.Directory)
            {
                DIRINFO = new DirectoryInfo(folderPath);
                DICPREFABSDATA.Clear();

                foreach (var t in DIRINFO.GetFiles())
                {
                    try
                    {
                        if (DICPREFABSDATA.Count > 49) break;
                    }
                    catch { }

                    if (t.Name.Contains(".meta") && t.Name.Contains(".prefab")) continue;
                    else if (t.Name.Contains(".prefab")) DICPREFABSDATA[t.ToString()] = t;
                }

                PREFABSCHECK.Clear();
                PREFABSCHECK = DICPREFABSDATA.Values.Select(s => (GameObject)AssetDatabase.LoadAssetAtPath(DirPathNameFactory(s.ToString()), typeof(GameObject))).ToList();
                foreach (var t in PREFABSCHECK)
                {
                    if (!PREFABS.Contains(t)) PREFABS.Add(t);
                }

                PREFABTEXTURELIST.Clear();
                AssetPreview.SetPreviewTextureCacheSize(0);
                AssetPreview.SetPreviewTextureCacheSize(5000);
            }
            else
            {
                string title1 = "Error Meassage";
                string msg1 = "폴더가 아닙니다. 폴더를 선택해 주세요.\n Must Imported Folder!";
                EditorUtility.DisplayDialog(title1, msg1, "OK");
            }

            FOLDER = null;
            Repaint();
        }

        PREFAB = (GameObject)EditorGUILayout.ObjectField("Input Prefab", PREFAB, typeof(GameObject), false);
        if (GUI.changed)
        {
            if (AssetDatabase.GetAssetPath(PREFAB).Contains(".prefab") && !PREFABS.Contains((GameObject)PREFAB))
            {
                PREFABS.Add((GameObject)PREFAB);
            }
            PREFAB = null;
            Repaint();
        }

        if (GUILayout.Button(" LOAD SELECTED PREFABS" , GUILayout.Height(22)) && Selection.objects.Length > 0)
        {
            foreach (var t in Selection.objects)
            {
                if (AssetDatabase.GetAssetPath(t).Contains(".prefab") && !PREFABS.Contains((GameObject)t)) PREFABS.Add((GameObject)t);
            }
        }

        int columns = Mathf.FloorToInt(Screen.width / 85);
        GUI.color = new Color(0.3f, 0.3f, 0.3f);
        GUILayout.BeginHorizontal(GUI.skin.box);
        SCROLLVIEWPREVIEW = GUILayout.BeginScrollView(SCROLLVIEWPREVIEW);
        GUILayout.BeginHorizontal();

        for (int i = 0; i < PREFABS.Count; i++)
        {
            if (PREFABS[i] == null)
            {
                MapMagEditor.RandomFrefabOn = false;
                MapMag.selectPrefab = null;
                SceneView.RepaintAll();
                PREFABS.RemoveAt(i);
                Repaint();
            }
            try 
            {
                if (i % columns == 0)
                {
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                }

                GUI.color = Color.white;
                if (AssetPreview.GetAssetPreview(PREFABS[i]) == null)
                {
                    buttonTexture = AssetPreview.GetMiniThumbnail(PREFABS[i]);
                }
                else
                {
                    buttonTexture = AssetPreview.GetAssetPreview(PREFABS[i]);
                }

                GUILayout.BeginVertical(GUILayout.Width(80));
                if (GUILayout.Button(buttonTexture, GUILayout.Width(80), GUILayout.Height(80)))
                {
                    MapMagEditor.RandomFrefabOn = false;
                    MapMag.selectPrefab = PREFABS[i];
                    SceneView.RepaintAll();
                }
                GUI.color = new Color(0.3f, 0.3f, 0.3f);
                prefabName = PREFABS[i].name;
                if (prefabName.Length > 11) prefabName = prefabName.Substring(0, 10);
                EditorGUILayout.HelpBox(prefabName, MessageType.None);
                GUI.color = new Color(0.4f, 0.4f, 0.4f);

                if (GUILayout.Button("Delete", GUILayout.Width(80), GUILayout.Height(18)))
                {
                    MapMagEditor.RandomFrefabOn = false;
                    MapMag.selectPrefab = null;
                    SceneView.RepaintAll();
                    PREFABS.RemoveAt(i);
                }
                GUILayout.EndVertical();
                Repaint();
                GUI.color = Color.white;

            }
            catch { }
        }

        GUILayout.EndHorizontal();
        GUILayout.EndScrollView();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("REMOVE ALL PREFABS DATA", GUILayout.MaxWidth(220)))
        {
            PREFABS.Clear();
            PREFABTEXTURELIST.Clear();
            DICPREFABSDATA.Clear();
            FOLDER = null;
        }

        GUI.color = PREFABS.Count >= 50 ? Color.yellow : Color.white;
        EditorGUILayout.HelpBox(" Current Buttons : " + PREFABS.Count + "", MessageType.None);
        GUILayout.EndHorizontal();
    }

    string DirPathNameFactory(string pathName)
    {
        DirpathName = pathName.Split(new string[] { "Assets" }, System.StringSplitOptions.None);
        returnText = "Assets" + DirpathName[1];
        return returnText;
    }

}

