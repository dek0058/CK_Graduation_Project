using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.Text;
using System.IO;

[CustomEditor(typeof(MapMag))] 
public class MapMagEditor : Editor
{
    GameObject makePreFabGroup;
    GameObject ReMoveGroup;
    GameObject[] pref;

    Transform[] parentPrefab;
    Transform[] basicPos;

    Vector2 scrollVector;
    Vector3 tilePos;
    Vector3 preFabPos;
    Vector3 firstPos;

    Quaternion firstRot;

    Datas allSaveDatas;

    PrefabDatas prefabDatas;

    string posName = " [Y-DEPTH] = ";
    string saveMapDataName = "Map Data Name";

    string nameLastChr;
    string completeName;
    string stateText;
    string infoText;
    string getTransformText;
    string savePrefabDataName;
    string prefabSourceName;
    string refTrasnformInfo;
    string insfactorInfoView;

    string[] mapDepthInfo;
    string[] transformhInfo;
    string[] nameSplit;
    string[] createMapData;
    string[] createPrefabData;
    string[] lastIndexName;
    string[] completPrefabName;
    string[] datas;
    string[] replaceName;

    bool leftAltOn;
    bool genMap;
    bool ranDomObjCheck = false;
    bool prefabPosRot;
    bool tilesizecheck = false;
    bool checkTileSizeMeassge;
    public static bool RandomFrefabOn;
    bool loadPrefabState;
    bool mapPosition;
    bool refRotation;
    bool getInfo;
    bool getTransformInfo;
    bool SeparationRatio;
    bool aspectRatio = true;
    bool rotX;
    bool rotY;
    bool rotZ;
    bool infoview;
    bool SavePrefabData;
    bool saveMapData;

    public static bool mapDataSwitch;
    public static string mapDataName;

    GameObject TileData;

    float rectWidth = 70;
    float buttonPosX = 10;
    float randRotNumX;
    float randRotNumY;
    float randRotNumZ;

    Rect rect;
    Rect MouseArea;
    Texture2D selectedPreFab;
    List<GameObject> deletePREbox = new List<GameObject>(); //중복된 오브젝트 삭제할때 오브젝트를 리스트에 담고 따로 지워 준다.
    List<GameObject> deleteFullDrawObj = new List<GameObject>(); //중복된 오브젝트 삭제할때 오브젝트를 리스트에 담고 따로 지워 준다.
    List<GameObject> childObj = new List<GameObject>();
    Dictionary<string, Quaternion> DICRandomRot = new Dictionary<string, Quaternion>();

    void InputUpdate(MapMag mapMag)
    {
        TextureInput(mapMag);
        TileSize(mapMag);
        TilePositon(mapMag);
        FirstTileSize(mapMag);
    }

    void MapLoad(MapMag mapMag)
    {
        if (RandomFrefabOn == true)
        {
            selectedPreFab = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/MapMakerV1/Texture/RANDOM.jpg");
        }
        else if (MapMag.selectPrefab == null)
        {
            selectedPreFab = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/MapMakerV1/Texture/SELECT.jpg");
        }
        else if (MapMag.selectPrefab.name == "ERASEROBJECT")
        {
            selectedPreFab = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/MapMakerV1/Texture/EARSER.jpg");
        }
        else if (MapMag.selectPrefab != null && RandomFrefabOn == false)
        {
            if (AssetPreview.GetAssetPreview(MapMag.selectPrefab) == null)
            {
                selectedPreFab = AssetPreview.GetMiniThumbnail(MapMag.selectPrefab);
            }
            else
            {
                selectedPreFab = AssetPreview.GetAssetPreview(MapMag.selectPrefab);
            }
        }
    }

    void MapDataLoad(MapMag mapMag)
    {
        if (mapDataSwitch)
        {
            try
            {
                LoadMapData(mapMag, mapDataName);
                
            }
            catch
            {
                //mapDataSwitch = false;
            }
        }
    }

    void FirstTileSize(MapMag mapMag)
    {

        if (mapMag.tileList == null)
        {

            mapMag.tileSize = 1;
            mapMag.mapSizeX = 1;
            mapMag.mapSizeZ = 1;
            mapMag.tilePosY = 0;
        }

        if (parentPrefab != null && parentPrefab.Length > 12 && checkTileSizeMeassge == false)
        {
            tilesizecheck = false;
            checkTileSizeMeassge = true;
        }
        if (parentPrefab != null && parentPrefab.Length > 12 && tilesizecheck == true)
        {
            string title1 = "Error Meassage";
            string msg1 = "맵 사이즈 조정은 프리팹 10개 이하에서 해주세요\nYou can adjust map size when you have 10 or less prefab";
            EditorUtility.DisplayDialog(title1, msg1, "OK");
            tilesizecheck = false;
        }
    }

    void TilePositon(MapMag mapMag)
    {

        mapMag.transform.position = new Vector3(mapMag.mapPosX, mapMag.tilePosY, mapMag.mapPosZ);
    }
    void TileSize(MapMag mapMag)
    {
        if (mapMag.mapSizeX > 1 || mapMag.mapSizeX < 1 || mapMag.mapSizeZ > 1 || mapMag.mapSizeZ < 1) aspectRatio = false;


        if (aspectRatio)
        {
            mapMag.transform.localScale = new Vector3(mapMag.tileSize, mapMag.tileSize, mapMag.tileSize);
        }
        else
        {
            mapMag.transform.localScale = new Vector3(mapMag.mapSizeX, mapMag.tileSize, mapMag.mapSizeZ);
        }

        if (makePreFabGroup != null && tilesizecheck == true)
        {
            parentPrefab = makePreFabGroup.GetComponentsInChildren<Transform>();
        }

        if (makePreFabGroup != null && tilesizecheck == true && parentPrefab.Length < 12) 
        {
            basicPos = mapMag.transform.GetComponentsInChildren<Transform>();
            for (int i = 0; i < basicPos.Length; i++)
            {
                for (int j = 0; j < parentPrefab.Length; j++)
                {
                    if (parentPrefab[j].name.Contains(basicPos[i].name))
                    {
                        preFabPos = new Vector3(basicPos[i].position.x + mapMag.prefabPosX, parentPrefab[j].position.y, basicPos[i].position.z + mapMag.prefabPosZ);
                        parentPrefab[j].position = preFabPos;
                    }
                }
            }
        }
    }

    int tileNumber = 1;
    void CreateGroup()
    {
        makePreFabGroup = new GameObject();
        makePreFabGroup.name = "PrefabGroup";
    }

    void CreateMap(MapMag mapMag)
    {
        if (mapMag.tileList != null)
        {
            string title1 = "Error Meassage";
            string msg1 = "모든 맵을 삭제후 실행 하세요.\nDelete all map tile first. you cannot create new map without delete all map tiles.";
            EditorUtility.DisplayDialog(title1, msg1, "OK");
            return;
        }
        if (makePreFabGroup == null) CreateGroup();

        int width = mapMag.currentWidth;
        int height = mapMag.currentHeight;
        mapMag.tileList = new GameObject[width, height];
        tileNumber = 1;
        for (int j = 0; j < mapMag.currentHeight; j++)
        {
            for (int i = 0; i < mapMag.currentWidth; i++)
            {
                GameObject obj = Instantiate(mapMag.tile) as GameObject;
                obj.transform.SetParent(mapMag.transform); 
                obj.transform.position = new Vector3(i, 0, j); 
                obj.name = j + "_" + i + " " + tileNumber; 
                obj.hideFlags = HideFlags.HideInHierarchy; 
                mapMag.tileList[i, j] = obj; 
                tileNumber++;
            }
        }
    }

    void RemoveMap(MapMag mapMag)
    {
        if (mapMag.tileList == null)
        {
            string title1 = "Error Meassage";
            string msg1 = "지울 맵이 없습니다.\nThere are no map tile to clear.";
            EditorUtility.DisplayDialog(title1, msg1, "OK");
            return;
        }

        if (mapMag.tileList != null)
        {
            makePreFabGroup = GameObject.Find("PrefabGroup");
            parentPrefab = makePreFabGroup.GetComponentsInChildren<Transform>();
            if (parentPrefab != null && parentPrefab.Length > 1)
            {
                string title1 = "Error Meassage";
                string msg1 = "맵을 완성 하거나 모든 프리팹을 삭제후 실행 하세요.\nDelete all prefab or complete map FIRST, then you can delete map tiles.";
                EditorUtility.DisplayDialog(title1, msg1, "OK");
                return;
            }

            mapMag.tileSize = 1; 

            int width = mapMag.currentWidth;
            int height = mapMag.currentHeight;

            for (int j = 0; j < height; ++j)
            {
                for (int i = 0; i < width; ++i)
                {
                    DestroyImmediate(mapMag.tileList[i, j]);
                }
            }

            RemoveTileMap(mapMag);
            mapMag.tileList = null;
        }
    }

    
    void CompletMapf(MapMag mapMag)
    {
        makePreFabGroup = GameObject.Find("PrefabGroup");
        parentPrefab = new Transform[makePreFabGroup.transform.childCount];
        for (int i = 0; i < makePreFabGroup.transform.childCount; i++)
            parentPrefab[i] = makePreFabGroup.transform.GetChild(i); 

        GameObject CompletMap = new GameObject();

        CompletMap.name = "CompletMapGroup";
        for (int i = 0; i < parentPrefab.Length; i++)
        {
            parentPrefab[i].transform.SetParent(CompletMap.transform);
            completPrefabName = parentPrefab[i].name.Split(new string[] { " @ " }, StringSplitOptions.None);
            parentPrefab[i].name = completPrefabName[0];
        }
        Array.Clear(parentPrefab, 0, parentPrefab.Length);
        parentPrefab = null;
        DestroyImmediate(makePreFabGroup);
    }

    void RemoveTileMap(MapMag mapMag)
    {
        Transform[] t = mapMag.gameObject.GetComponentsInChildren<Transform>();
        for (int i = 0; i < t.Length; i++)
        {
            if (t[i].CompareTag("EditorTile"))
            {
                DestroyImmediate(t[i].gameObject);
            }
        }
        ReMoveGroup = GameObject.Find("PrefabGroup");
        DestroyImmediate(ReMoveGroup);
    }

    void TextureInput(MapMag mapMag)
    {
        if (mapMag.PREFABS.Length > 5)
        {
            string title1 = "Error Meassage";
            string msg1 = "프리팹 버튼은 5개 까지 생성 할 수 있습니다.\nYou can create up to five Prefab button.";
            EditorUtility.DisplayDialog(title1, msg1, "OK");
            Array.Resize<GameObject>(ref mapMag.PREFABS, 5); 
            return;
        }

        if (mapMag.PREFABS.Length == 0)
        {
            mapMag.ArrResrt();
            return;
        }
        if (mapMag.PREFABS.Length < mapMag.dicTexture.Count)
        {
            for (int i = mapMag.PREFABS.Length; i < mapMag.dicTexture.Count; ++i)
            {
                mapMag.dicTexture.Remove(i);
            }
        }

        for (int i = 0; i < mapMag.PREFABS.Length; i++)
        {
            if (mapMag.PREFABS[i] == null)
            {
                return;
            }
            mapMag.dicTexture[i] = AssetPreview.GetAssetPreview(mapMag.PREFABS[i]);
        }
    }

    void LenghCase(MapMag mapMag, int length)
    {
        for (int i = (length + 1); i < mapMag.dicTexture.Count; i++)
        {
            mapMag.dicTexture.Remove(i);
        }
    }

    void CreativeBTN(float posX, float posY, int textureNum, MapMag mapMag)
    {
        rect = new Rect(posX, posY, Screen.height / 8, Screen.height / 8);
        for (int i = 0; i < mapMag.PREFABS.Length; i++)
        {
            if (mapMag.PREFABS[i] == null)
            {
                return;
            }
        }

        if (GUI.Button(rect, mapMag.dicTexture[textureNum]))
        {
            RandomFrefabOn = false;
            MapMag.selectPrefab = mapMag.PREFABS[textureNum];
        }
    }

    public void SelectPref(MapMag mapMag, GameObject prefab)
    {
        MapMag.selectPrefab = prefab;
    }

    string SelectObjectNameSplit(string name)
    {
        nameSplit = name.Split(new string[] { " @ " }, StringSplitOptions.None);
        return nameSplit[1];
    }


    void OnSceneGUI()
    {
        if (Application.isPlaying == true)
        {
            return;
        }

        MapMag mapMag = target as MapMag;
        Handles.BeginGUI(); 

        MapLoad(mapMag);
        MapDataLoad(mapMag);

        rect = new Rect(10, 10, Screen.height / 8, Screen.height / 8);
        if (GUI.Button(rect, mapMag.titleTexture))
        {
            string title1 = "Meassage";
            string msg1 = "2015 NEXTI GAMES \nWEB SITE : https://www.behance.net/ankWorks \nE-MAIL : adepter@naver.com ";
            EditorUtility.DisplayDialog(title1, msg1, "OK");

        }
        rect = new Rect(10, (Screen.height / 8) + 15, Screen.height / 8, Screen.height / 8);
        if (GUI.Button(rect, mapMag.earserTexture))
        {
            RandomFrefabOn = false;
            getTransformInfo = false;
            MapMag.selectPrefab = mapMag.eraserObj;

        }
        rect = new Rect(10, (Screen.height / 8) * 2 + 20, Screen.height / 8, Screen.height / 8);
        if (GUI.Button(rect, selectedPreFab))
        {

            string title1 = "Meassage";
            string msg1 = "Current Selected Prefab";
            EditorUtility.DisplayDialog(title1, msg1, "OK");

        }

        switch (mapMag.dicTexture.Count)
        {

            case 0:

                break;
            case 1:
                CreativeBTN(buttonPosX, ((Screen.height / 8) * 2) + 15, 0, mapMag);
                break;
            case 2:
                CreativeBTN(buttonPosX, ((Screen.height / 8) * 2) + 15, 0, mapMag);
                CreativeBTN(buttonPosX, ((Screen.height / 8) * 3) + 15, 1, mapMag);
                break;
            case 3:
                CreativeBTN(buttonPosX, ((Screen.height / 8) * 2) + 15, 0, mapMag);
                CreativeBTN(buttonPosX, ((Screen.height / 8) * 3) + 15, 1, mapMag);
                CreativeBTN(buttonPosX, ((Screen.height / 8) * 4) + 15, 2, mapMag);
                break;
            case 4:
                CreativeBTN(buttonPosX, ((Screen.height / 8) * 2) + 15, 0, mapMag);
                CreativeBTN(buttonPosX, ((Screen.height / 8) * 3) + 15, 1, mapMag);
                CreativeBTN(buttonPosX, ((Screen.height / 8) * 4) + 15, 2, mapMag);
                CreativeBTN(buttonPosX, ((Screen.height / 8) * 5) + 15, 3, mapMag);
                break;
            case 5:
                CreativeBTN(buttonPosX, ((Screen.height / 8) * 2) + 15, 0, mapMag);
                CreativeBTN(buttonPosX, ((Screen.height / 8) * 3) + 15, 1, mapMag);
                CreativeBTN(buttonPosX, ((Screen.height / 8) * 4) + 15, 2, mapMag);
                CreativeBTN(buttonPosX, ((Screen.height / 8) * 5) + 15, 3, mapMag);
                CreativeBTN(buttonPosX, ((Screen.height / 8) * 6) + 15, 4, mapMag);
                break;
        }

        if (mapMag.RANDOMPrefabs.Count > 1)
        {
            rect = new Rect((Screen.height / 8) + 15, (Screen.height / 8) + 15, Screen.height / 8, Screen.height / 8);
            if (GUI.Button(rect, mapMag.randomTexture))
            {
                if (mapMag.RANDOMPrefabs[1] == null)
                {
                    string title1 = "Error Meassage";
                    string msg1 = "2개 이상의 프리팹을 등록 해주세요.\nApply two more prefabs";
                    EditorUtility.DisplayDialog(title1, msg1, "OK");
                    return;
                }
                if (mapMag.tileList == null)
                {
                    string title1 = "Error Meassage";
                    string msg1 = "맵이 생성되지 않았습니다.\nPlease generate the map";
                    EditorUtility.DisplayDialog(title1, msg1, "OK");
                    return;
                }
                if (mapMag.RANDOMPrefabs.Count > 49)
                {
                    string title1 = "Meassage";
                    string msg1 = "랜덤 프리팹은 49개 이하로 등록 해주세요.\nThe number of random prefab registration is limited up to forty-nine.";
                    EditorUtility.DisplayDialog(title1, msg1, "OK");

                    int count = mapMag.RANDOMPrefabs.Count-1;
                    for (int i = 49; i < mapMag.RANDOMPrefabs.Count; count--)
                    {
                        mapMag.RANDOMPrefabs.RemoveAt(count);
                    }
                }
                RandomFrefabOn = true;
            }
        }

        if (getTransformInfo)
        {
            rect = new Rect(0, 0, Screen.width, Screen.height);
            EditorGUIUtility.AddCursorRect(rect, MouseCursor.ArrowPlus);
        }
        if (getInfo)
        {
            rect = new Rect(0, 0, Screen.width, Screen.height);
            EditorGUIUtility.AddCursorRect(rect, MouseCursor.ArrowPlus);
        }

        GUIStyle style = new GUIStyle(); 
        style.normal.textColor = Color.green; 

        style.fontSize = 12; 
        rect = new Rect((Screen.height / 8) + 17, 15, 500, 30);
        GUI.Label(rect, "[ EDIT MODE ]", style);
        rect = new Rect((Screen.height / 8) + 17, 35, 500, 30);
        if (RandomFrefabOn == true)
        {
            stateText = "- Selected prefab : Random Prefab";
            GUI.Label(rect, stateText, style);
        }
        else if (MapMag.selectPrefab == null)
        {
            stateText = "- No selected prefab.";
            GUI.Label(rect, stateText, style);
        }
        else if (MapMag.selectPrefab.gameObject.name == "ERASEROBJECT")
        {
            stateText = "- Earser mode.";
            GUI.Label(rect, stateText, style);
        }
        else if (MapMag.selectPrefab != null && RandomFrefabOn == false)
        {
            stateText = "- Selected prefab : ";
            GUI.Label(rect, stateText + MapMag.selectPrefab.name, style);
        }
        rect = new Rect((Screen.height / 8) + 17, 55, 500, 30);
        GUI.Label(rect, infoText, style); 
        Handles.EndGUI();

        int id = GUIUtility.GetControlID(FocusType.Passive);
        HandleUtility.AddDefaultControl(id);

        Event key = Event.current;
        if (key.keyCode == KeyCode.LeftAlt)
        {
            leftAltOn = true;
        }
        if (key.type == EventType.KeyUp)
        {
            leftAltOn = false;
        }
        if (key.type == EventType.MouseUp)
        {

            DubbleObjCheck(mapMag);
        }

        Event e = Event.current;
        if ((e.type == EventType.MouseDown || e.type == EventType.MouseDrag) && Event.current.button == 0 && leftAltOn == false)
        {
            if (stateText == "- No selected prefab.")
            {
                return;
            }

            Vector2 mousePosition = Event.current.mousePosition; 
            RaycastHit hit;
            Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition); 
            if (Physics.Raycast(ray, out hit)) 
            {
                Transform go = hit.transform.gameObject.GetComponent<Transform>();
                makePreFabGroup = GameObject.Find("PrefabGroup");
                if ((MapMag.selectPrefab == null && mapMag.RANDOMPrefabs[1] == null) || mapMag.tileList == null || go == null)
                {
                    return;
                }
                else if (makePreFabGroup == null)
                {
                    CreateGroup();
                }

                if (go != null && go.transform.gameObject.tag == "EditorTile")
                {  
                    try  
                    {
                        if (mapMag.CreateObjectDic[go.name + posName + mapMag.tilePosY] != null)
                        {
                            DestroyImmediate(mapMag.CreateObjectDic[go.name + posName + mapMag.tilePosY]);
                            mapMag.CreateObjectDic.Remove(go.name + posName + mapMag.tilePosY); // 딕셔너리를 키값으로 지울때는 remove!!
                        }
                    }
                    catch
                    {
                        // 
                    }

                    if (mapMag.RANDOMPrefabs.Count > 1 && RandomFrefabOn == true)
                    {
                        MapMag.selectPrefab = mapMag.RANDOMPrefabs[RandNum(mapMag)];
                    }

                    if (rotX || rotY || rotZ) RandomRotation(mapMag);
                    firstRot = Quaternion.Euler(mapMag.prefabRotX, mapMag.prefabRotY, mapMag.prefabRotZ);
                    firstPos = new Vector3(go.position.x + mapMag.prefabPosX, go.position.y + mapMag.prefabPosY, go.position.z + mapMag.prefabPosZ);

                    GameObject pre = PrefabUtility.InstantiatePrefab(MapMag.selectPrefab) as GameObject;
                    pre.transform.position = firstPos;
                    pre.transform.rotation = firstRot;
                    pre.transform.parent = makePreFabGroup.transform;
                    pre.name = pre.name + " @ " + go.name + posName + mapMag.tilePosY;
                    pre.name = pre.name.Replace("(Clone)", null);
                    mapMag.CreateObjectDic[SelectObjectNameSplit(pre.name)] = pre;
                    RandomPrefInfo(pre, mapMag);
                    mapMag.prefabInfo[pre.name] = mapMag.prefabPosX + " / " + mapMag.prefabPosY + " / " + mapMag.prefabPosZ + " / " + mapMag.prefabRotX + " / " + mapMag.prefabRotY + " / " + mapMag.prefabRotZ;

                    if (pre.tag == "eraser")
                    {
                        mapMag.CreateObjectDic.Remove(SelectObjectNameSplit(pre.name));
                        mapMag.prefabInfo.Remove(pre.name);
                        DestroyImmediate(pre);
                        return;
                    }

                }
                else if (go != null && go.transform.gameObject.tag != "EditorTile" && go.name.Contains(posName))
                {
                    if (getInfo && getTransformInfo)
                    {
                        GetPrefabInfo(mapMag, go);
                        GetTrasnformInfoF(mapMag, mapMag.prefabInfo[go.name]);
                        getInfo = false;
                        return;
                    }
                    if (getInfo)
                    {
                        GetPrefabInfo(mapMag, go);
                        getInfo = false;
                        return;
                    }
                    if (getTransformInfo)
                    {
                        GetTrasnformInfoF(mapMag, mapMag.prefabInfo[go.name]);
                        return;
                    }
                    if (go.name.Contains(" = " + mapMag.tilePosY.ToString()))
                    {
                        if (mapMag.RANDOMPrefabs.Count > 1 && RandomFrefabOn == true)
                        {
                            MapMag.selectPrefab = mapMag.RANDOMPrefabs[RandNum(mapMag)];
                        }

                        if (rotX || rotY || rotZ) RandomRotation(mapMag);
                        firstRot = Quaternion.Euler(mapMag.prefabRotX, mapMag.prefabRotY, mapMag.prefabRotZ);
                        GameObject pre;

                        if (refRotation)
                        {
                            pre = PrefabUtility.InstantiatePrefab(MapMag.selectPrefab) as GameObject;
                            pre.transform.position = go.position;
                            pre.transform.rotation = go.transform.rotation;

                        }
                        else
                        {
                            pre = PrefabUtility.InstantiatePrefab(MapMag.selectPrefab) as GameObject;
                            pre.transform.position = go.position;
                            pre.transform.rotation = firstRot;
                        }
                        pre.transform.SetParent(makePreFabGroup.transform);
                        rePlaceNameFactory(go.name);
                        pre.name = pre.name + " @ " + replaceName[1];
                        pre.name = pre.name.Replace("(Clone)", null);
                        mapMag.CreateObjectDic[SelectObjectNameSplit(pre.name)] = pre;

                        if (refRotation)
                        {
                            refTrasnformInfo = mapMag.prefabInfo[go.name];
                            mapMag.prefabInfo[pre.name] = refTrasnformInfo;
                        }
                        else
                        {
                            mapMag.prefabInfo[pre.name] = mapMag.prefabPosX + " / " + mapMag.prefabPosY + " / " + mapMag.prefabPosZ + " / " + mapMag.prefabRotX + " / " + mapMag.prefabRotY + " / " + mapMag.prefabRotZ;
                        }

                        DestroyImmediate(go.gameObject);
                        if (pre.tag == "eraser")
                        {

                            mapMag.CreateObjectDic.Remove(SelectObjectNameSplit(pre.name)); 
                            mapMag.prefabInfo.Remove(pre.name);
                            DestroyImmediate(pre);
                            return;
                        }
                    }
                }
            }
        }
        Tools.current = Tool.None; 
    }

    void rePlaceNameFactory(string name)
    {
        replaceName = name.Split(new string[] { " @ " }, StringSplitOptions.None);
    }

    void RandomPrefInfo(GameObject pre, MapMag mapMag)
    {
        if (rotX && rotY && rotZ) mapMag.prefabInfo[pre.name] = mapMag.prefabPosX + " / " + mapMag.prefabPosY + " / " + mapMag.prefabPosZ + " / " + firstRot.x + " / " + firstRot.y + " / " + firstRot.z;
        else if (rotY && rotZ) mapMag.prefabInfo[pre.name] = mapMag.prefabPosX + " / " + mapMag.prefabPosY + " / " + mapMag.prefabPosZ + " / " + mapMag.prefabRotX + " / " + firstRot.y + " / " + firstRot.z;
        else if (rotX && rotZ) mapMag.prefabInfo[pre.name] = mapMag.prefabPosX + " / " + mapMag.prefabPosY + " / " + mapMag.prefabPosZ + " / " + firstRot.x + " / " + mapMag.prefabRotY + " / " + firstRot.z;
        else if (rotX && rotY) mapMag.prefabInfo[pre.name] = mapMag.prefabPosX + " / " + mapMag.prefabPosY + " / " + mapMag.prefabPosZ + " / " + firstRot.x + " / " + firstRot.y + " / " + mapMag.prefabRotZ;
        else if (rotZ) mapMag.prefabInfo[pre.name] = mapMag.prefabPosX + " / " + mapMag.prefabPosY + " / " + mapMag.prefabPosZ + " / " + mapMag.prefabRotX + " / " + mapMag.prefabRotY + " / " + firstRot.z;
        else if (rotY) mapMag.prefabInfo[pre.name] = mapMag.prefabPosX + " / " + mapMag.prefabPosY + " / " + mapMag.prefabPosZ + " / " + mapMag.prefabRotX + " / " + firstRot.y + " / " + mapMag.prefabRotZ;
        else if (rotX) mapMag.prefabInfo[pre.name] = mapMag.prefabPosX + " / " + mapMag.prefabPosY + " / " + mapMag.prefabPosZ + " / " + firstRot.x + " / " + mapMag.prefabRotY + " / " + mapMag.prefabRotZ;
    }

    void GetPrefabInfo(MapMag mapMag, Transform obj)
    {

        mapDepthInfo = obj.name.Split(new string[] { " = " }, StringSplitOptions.None);

        mapMag.tilePosY = float.Parse(mapDepthInfo[1]);
    }

    void GetTrasnformInfoF(MapMag mapMag, string prefabtrinfo)
    {
        transformhInfo = prefabtrinfo.Split(new string[] { " / " }, StringSplitOptions.None);


        mapMag.prefabPosX = float.Parse(transformhInfo[0]);
        mapMag.prefabPosY = float.Parse(transformhInfo[1]);
        mapMag.prefabPosZ = float.Parse(transformhInfo[2]);

        mapMag.prefabRotX = float.Parse(transformhInfo[3]);
        mapMag.prefabRotY = float.Parse(transformhInfo[4]);
        mapMag.prefabRotZ = float.Parse(transformhInfo[5]);

        infoview = true;
        insfactorInfoView = " - POSTION : X = " + transformhInfo[0] + " | Y = " + transformhInfo[1] + " | Z = " + transformhInfo[2] + "\n"
                          + " - ROTATION : X = " + transformhInfo[3] + " | Y = " + transformhInfo[4] + " | Z = " + transformhInfo[5] + "\n";
    }

    void INFOVIEW(string[] info)
    {
        //
    }

    void RandomRotation(MapMag mapMag)
    {
        randRotNumX = RandomFloat(mapMag.randomprefabRotXMinValue, mapMag.randomprefabRotXMaxValue);
        randRotNumY = RandomFloat(mapMag.randomprefabRotYMinValue, mapMag.randomprefabRotYMaxValue);
        randRotNumZ = RandomFloat(mapMag.randomprefabRotZMinValue, mapMag.randomprefabRotZMaxValue);

        if (rotX) mapMag.prefabRotX = randRotNumX;
        if (rotY) mapMag.prefabRotY = randRotNumY;
        if (rotZ) mapMag.prefabRotZ = randRotNumZ;
        if (rotX && rotY) { mapMag.prefabRotX = randRotNumX; mapMag.prefabRotY = randRotNumY; }
        if (rotX && rotZ) { mapMag.prefabRotX = randRotNumX; mapMag.prefabRotZ = randRotNumZ; }
        if (rotY && rotZ) { mapMag.prefabRotY = randRotNumY; mapMag.prefabRotZ = randRotNumZ; }
        if (rotX && rotY && rotZ) { mapMag.prefabRotX = randRotNumX; mapMag.prefabRotY = randRotNumY; mapMag.prefabRotZ = randRotNumZ; }
    }

    void RanDomDic(MapMag mapMag)
    {
        DICRandomRot["X"] = Quaternion.Euler(randRotNumX, mapMag.prefabRotY, mapMag.prefabRotZ);
        DICRandomRot["Y"] = Quaternion.Euler(mapMag.prefabRotX, randRotNumY, mapMag.prefabRotZ);
        DICRandomRot["Z"] = Quaternion.Euler(mapMag.prefabRotX, mapMag.prefabRotY, randRotNumZ);
        DICRandomRot["XY"] = Quaternion.Euler(randRotNumX, randRotNumY, mapMag.prefabRotZ);
        DICRandomRot["XZ"] = Quaternion.Euler(randRotNumX, mapMag.prefabRotY, randRotNumZ);
        DICRandomRot["YZ"] = Quaternion.Euler(mapMag.prefabRotX, randRotNumY, randRotNumZ);
        DICRandomRot["XYZ"] = Quaternion.Euler(randRotNumX, randRotNumY, randRotNumZ);
    }

    float RandomFloat(float min, float max)
    {
        return UnityEngine.Mathf.Round(UnityEngine.Random.Range(min, max));
    }

    int randFrefabNum;
    int RandNum(MapMag mapMag)
    {
        randFrefabNum = UnityEngine.Random.Range(0, mapMag.RANDOMPrefabs.Count);
        return randFrefabNum;
    }

    string[] currentObjName;
    void EmptyFullDrawPrefab(MapMag mapMag)
    {
        if (MapMag.selectPrefab == null && mapMag.RANDOMPrefabs[1] == null)
        {
            return;
        }
        if (mapMag.tileList != null)
        {
            makePreFabGroup = GameObject.Find("PrefabGroup");
            parentPrefab = makePreFabGroup.GetComponentsInChildren<Transform>();

            Transform go;
            for (int i = 0; i < mapMag.tileList.GetLength(0); i++)
            {
                for (int j = 0; j < mapMag.tileList.GetLength(1); j++)
                {
                    go = mapMag.tileList[i, j].GetComponent<Transform>();

                    foreach (Transform t in parentPrefab)
                    {
                        if (t.gameObject.name.Contains(go.name + posName + mapMag.tilePosY))
                        {
                            goto Escape; 
                        }
                    }
                    if (go != null && go.transform.gameObject.tag == "EditorTile")
                    {
                        if (mapMag.RANDOMPrefabs.Count > 1 && RandomFrefabOn == true)
                        {
                            MapMag.selectPrefab = mapMag.RANDOMPrefabs[RandNum(mapMag)];
                        }

                        if (rotX || rotY || rotZ) RandomRotation(mapMag);
                        firstRot = Quaternion.Euler(mapMag.prefabRotX, mapMag.prefabRotY, mapMag.prefabRotZ);
                        firstPos = new Vector3(go.position.x + mapMag.prefabPosX, go.position.y + mapMag.prefabPosY, go.position.z + mapMag.prefabPosZ);
                        GameObject pre = PrefabUtility.InstantiatePrefab(MapMag.selectPrefab) as GameObject;
                        pre.transform.position = firstPos;
                        pre.transform.rotation = firstRot;
                        pre.transform.SetParent(makePreFabGroup.transform);
                        pre.name = pre.name + " @ " + go.name + posName + mapMag.tilePosY;
                        pre.name = pre.name.Replace("(Clone)", null);
                        mapMag.CreateObjectDic[SelectObjectNameSplit(pre.name)] = pre;
                        mapMag.prefabInfo[pre.name] = mapMag.prefabPosX + " / " + mapMag.prefabPosY + " / " + mapMag.prefabPosZ + " / " + mapMag.prefabRotX + " / " + mapMag.prefabRotY + " / " + mapMag.prefabRotZ;
                        if (pre.tag == "eraser")
                        {
                            mapMag.CreateObjectDic.Remove(go.name + posName + mapMag.tilePosY);
                            mapMag.prefabInfo.Remove(go.name + posName + mapMag.tilePosY);
                            DestroyImmediate(pre);

                            return;
                        }
                    }
                Escape: 
                    continue;
                }
            }
        }
    }



    void makeFullPrefab(MapMag mapMag)
    {

        if (MapMag.selectPrefab == null && mapMag.RANDOMPrefabs[1] == null)
        {
            return;
        }
        if (mapMag.tileList != null)
        {
            makePreFabGroup = GameObject.Find("PrefabGroup");
            FullDrawCheck(mapMag);
            Transform go;
            for (int i = 0; i < mapMag.tileList.GetLength(0); i++)
            {
                for (int j = 0; j < mapMag.tileList.GetLength(1); j++)
                {
                    go = mapMag.tileList[i, j].GetComponent<Transform>();

                    if (go != null && go.transform.gameObject.tag == "EditorTile")
                    {
                        if (mapMag.RANDOMPrefabs.Count > 1 && RandomFrefabOn == true)
                        {
                            MapMag.selectPrefab = mapMag.RANDOMPrefabs[RandNum(mapMag)];
                        }

                        if (rotX || rotY || rotZ) RandomRotation(mapMag);
                        firstRot = Quaternion.Euler(mapMag.prefabRotX, mapMag.prefabRotY, mapMag.prefabRotZ);
                        firstPos = new Vector3(go.position.x + mapMag.prefabPosX, go.position.y + mapMag.prefabPosY, go.position.z + mapMag.prefabPosZ);
                        GameObject pre = PrefabUtility.InstantiatePrefab(MapMag.selectPrefab) as GameObject;
                        pre.transform.position = firstPos;
                        pre.transform.rotation = firstRot;
                        pre.transform.SetParent(makePreFabGroup.transform);
                        pre.name = pre.name + " @ " + go.name + posName + mapMag.tilePosY;
                        pre.name = pre.name.Replace("(Clone)", null);
                        mapMag.CreateObjectDic[SelectObjectNameSplit(pre.name)] = pre;
                        mapMag.prefabInfo[pre.name] = mapMag.prefabPosX + " / " + mapMag.prefabPosY + " / " + mapMag.prefabPosZ + " / " + mapMag.prefabRotX + " / " + mapMag.prefabRotY + " / " + mapMag.prefabRotZ;
                        if (pre.tag == "eraser")
                        {
                            mapMag.CreateObjectDic.Remove(SelectObjectNameSplit(pre.name));
                            mapMag.prefabInfo.Remove(pre.name);
                            DestroyImmediate(pre);

                            return;
                        }
                    }
                }
            }
        }
    }

    void FullDrawCheck(MapMag mapMag)
    {
        makePreFabGroup = GameObject.Find("PrefabGroup");
        parentPrefab = makePreFabGroup.GetComponentsInChildren<Transform>();

        for (int i = 0; i < parentPrefab.Length; ++i)
        {
            if (parentPrefab[i].gameObject.name.Contains(posName + mapMag.tilePosY.ToString()))
            {
                deleteFullDrawObj.Add(parentPrefab[i].gameObject);
            }
        }
        defull(mapMag);
    }

    void defull(MapMag mapMag)
    {
        if (deleteFullDrawObj == null) return;
        foreach (GameObject t in deleteFullDrawObj)
        {
            mapMag.CreateObjectDic.Remove(SelectObjectNameSplit(t.name));
            mapMag.prefabInfo.Remove(t.name);
            DestroyImmediate(t);
        }
        deleteFullDrawObj.Clear();
    }

    void DubbleObjCheck(MapMag mapMag)
    {
        if (mapMag.tileList == null)
        {
            return;
        }
        else
        {
            makePreFabGroup = GameObject.Find("PrefabGroup");
            parentPrefab = new Transform[makePreFabGroup.transform.childCount];
            for (int i = 0; i < makePreFabGroup.transform.childCount; i++)
                parentPrefab[i] = makePreFabGroup.transform.GetChild(i); 
        }

        for (int i = 0; i < parentPrefab.Length; ++i)
        {
            for (int j = 0; j < i; j++)
            {

                if (parentPrefab[i].gameObject.name == parentPrefab[j].gameObject.name)
                {
                    deletePREbox.Add(parentPrefab[j].gameObject);
                }
            }
        }
        de();
    }
    void de()
    {
        if (deletePREbox == null) return;
        foreach (GameObject t in deletePREbox)
        {
            DestroyImmediate(t);
        }
        deletePREbox.Clear();
    }


    void ShowPrefabs()
    {
        PrefabData fabDataWindow = (PrefabData)EditorWindow.GetWindow(typeof(PrefabData));
        fabDataWindow.Show();
    }

    void ShowMapData()
    {
        //
    }
    string[] mapDataInputName;
    string[] mapDataInputValue;
    string mapDataInputALL;
    string prefabDataInputAll;

    void SaveMapDATAS(MapMag mapMag, string name)
    {

        try
        {
            makePreFabGroup = GameObject.Find("PrefabGroup");
            parentPrefab = makePreFabGroup.GetComponentsInChildren<Transform>();
        }
        catch
        {
            return;
        }
        allSaveDatas = ScriptableObject.CreateInstance<Datas>();
        AssetDatabase.CreateAsset(allSaveDatas, "Assets/Resources/MapMakerV1/Data/Mapdata/" + name + ".asset");

        foreach (Transform t in parentPrefab)
        {
            if (t.name == "PrefabGroup") continue;
            NameFactory(mapMag, t.name, mapMag.prefabInfo[t.name]);
            allSaveDatas.MAPDATA = mapDataInputALL;
            allSaveDatas.PREFABDATA.Add(prefabDataInputAll);
            allSaveDatas.PREFABSOURCE[mapDataInputName[0]] = PrefabUtility.GetPrefabParent(t.gameObject);
            mapDataInputALL = "";
            prefabDataInputAll = "";
            Array.Clear(mapDataInputName, 0, mapDataInputName.Length);
            Array.Clear(mapDataInputValue, 0, mapDataInputValue.Length);
        }

        foreach (var t in allSaveDatas.PREFABSOURCE)
        {
            allSaveDatas.PrefabSources.Add((UnityEngine.Object)t.Value);
        }

        EditorUtility.SetDirty(allSaveDatas); 
        AssetDatabase.SaveAssets();
        saveMapDataName = "Map Data Name";
    }

    void NameFactory(MapMag mapMag, string name, string values)
    {
        mapDataInputName = name.Split(new string[] { " " }, StringSplitOptions.None);
        mapDataInputValue = values.Split(new string[] { " / " }, StringSplitOptions.None);
        if (mapMag.mapSizeX != 1 || mapMag.mapSizeZ != 1)
        {
            mapDataInputALL = "XY/" + mapMag.currentWidth.ToString() + "/" + mapMag.currentHeight.ToString() + "/" + mapMag.mapSizeX + "/" + mapMag.mapSizeZ;
        }
        else
        {
            mapDataInputALL = mapMag.currentWidth.ToString() + "/" + mapMag.currentHeight.ToString() + "/" + mapMag.tileSize;
        }
        for (int i = 0; i < mapDataInputValue.Length; i++)
        {
            prefabDataInputAll = prefabDataInputAll + mapDataInputValue[i] + "/";
        }
        prefabDataInputAll = prefabDataInputAll + mapDataInputName[6] + "/" + mapDataInputName[2] + " " + mapDataInputName[3] + "/" + mapDataInputName[0];
    }

    void LoadScripterbleObject()
    {
        MapDataLoad fabDataWindow = (MapDataLoad)EditorWindow.GetWindow(typeof(MapDataLoad));
        fabDataWindow.Show();
    }
    public void LoadMapData(MapMag mapMag, string name)
    {
        mapDataSwitch = false;
        makePreFabGroup = GameObject.Find("PrefabGroup");
        if (makePreFabGroup != null)
        {
            string title1 = "Error Meassage";
            string msg1 = "dyd모든 맵을 삭제후 실행 하세요.\nDelete all map tile first. you cannot create new map without delete all map tiles.";
            EditorUtility.DisplayDialog(title1, msg1, "OK");
            return;
        }

        Datas loadData = new Datas();
        loadData = (Datas)AssetDatabase.LoadAssetAtPath("Assets/Resources/MapMakerV1/Data/Mapdata/" + name + ".asset", typeof(Datas));
        CreateDATAMap(mapMag, loadData.MAPDATA);

        for (int i = 0; i < loadData.PREFABDATA.Count; i++)
        {
            CreateDATAPrefab(mapMag, loadData.PREFABDATA[i], loadData);
        }
        loadPrefabState = false;
        SceneView.RepaintAll();
    }

    void CreateDATAMap(MapMag mapMag, string mapData)
    {

        createMapData = mapData.Split(new string[] { "/" }, StringSplitOptions.None);
        if (createMapData[0] == "XY")
        {
            mapMag.currentWidth = int.Parse(createMapData[1]);
            mapMag.currentHeight = int.Parse(createMapData[2]);

            CreateMap(mapMag);

            aspectRatio = false;
            mapMag.mapSizeX = float.Parse(createMapData[3]);
            mapMag.mapSizeZ = float.Parse(createMapData[4]);
            mapMag.transform.localScale = new Vector3(mapMag.mapSizeX, mapMag.transform.localScale.y, mapMag.mapSizeZ);
            SceneView.RepaintAll();
        }
        else
        {
            mapMag.currentWidth = int.Parse(createMapData[0]);
            mapMag.currentHeight = int.Parse(createMapData[1]);
            CreateMap(mapMag);
            mapMag.tileSize = float.Parse(createMapData[2]);
            mapMag.transform.localScale = new Vector3(mapMag.tileSize, mapMag.transform.localScale.y, mapMag.tileSize);
            SceneView.RepaintAll();
        }
    }

    void CreateDATAPrefab(MapMag mapMag, string prefabData, Datas loadData)
    {
        createPrefabData = prefabData.Split(new string[] { "/" }, StringSplitOptions.None);
        mapMag.prefabPosX = float.Parse(createPrefabData[0]);
        mapMag.prefabPosY = float.Parse(createPrefabData[1]);
        mapMag.prefabPosZ = float.Parse(createPrefabData[2]);
        mapMag.prefabRotX = float.Parse(createPrefabData[3]);
        mapMag.prefabRotY = float.Parse(createPrefabData[4]);
        mapMag.prefabRotZ = float.Parse(createPrefabData[5]);
        mapMag.tilePosY = float.Parse(createPrefabData[6]);
        TileData = GameObject.Find(createPrefabData[7]);
        prefabSourceName = createPrefabData[8];

        for (int i = 0; i < loadData.PrefabSources.Count; i++)
        {
            if (loadData.PrefabSources[i].name == prefabSourceName)
            {
                MapMag.selectPrefab = (GameObject)loadData.PrefabSources[i];
                break;
            }
        }

        if (MapMag.selectPrefab == null)
        {
            string title1 = "Error Meassage";
            string msg1 = "프리팹이 없습니다.\nThere is no prefab";
            EditorUtility.DisplayDialog(title1, msg1, "OK");
            loadPrefabState = true;
            return;
        }

        firstRot = Quaternion.Euler(mapMag.prefabRotX, mapMag.prefabRotY, mapMag.prefabRotZ);
        firstPos = new Vector3(TileData.transform.position.x + mapMag.prefabPosX, mapMag.tilePosY + mapMag.prefabPosY, TileData.transform.position.z + mapMag.prefabPosZ);
        GameObject pre = PrefabUtility.InstantiatePrefab(MapMag.selectPrefab) as GameObject;
        pre.transform.position = firstPos;
        pre.transform.rotation = firstRot;
        pre.transform.SetParent(makePreFabGroup.transform);
        pre.name = pre.name + " @ " + TileData.name + posName + mapMag.tilePosY;
        pre.name = pre.name.Replace("(Clone)", null);
        mapMag.CreateObjectDic[SelectObjectNameSplit(pre.name)] = pre;
        RandomPrefInfo(pre, mapMag);
        mapMag.prefabInfo[pre.name] = mapMag.prefabPosX + " / " + mapMag.prefabPosY + " / " + mapMag.prefabPosZ + " / " + mapMag.prefabRotX + " / " + mapMag.prefabRotY + " / " + mapMag.prefabRotZ;
    }

     
    void OnEnable()
    {
        LoadDatabase();
    }

    void LoadDatabase()
    {
        prefabDatas = (PrefabDatas)AssetDatabase.LoadAssetAtPath("Assets/Resources/MapMakerV1/Data/prefabInfoData.asset", typeof(PrefabDatas));
        if (prefabDatas == null)
        {
            CreatePrefabsDataBase();
        }
    }

    void CreatePrefabsDataBase()
    {
        prefabDatas = ScriptableObject.CreateInstance<PrefabDatas>();
        AssetDatabase.CreateAsset(prefabDatas, "Assets/Resources/MapMakerV1/Data/prefabInfoData.asset");
    }

    void DataLoad(MapMag mapMag, string value)
    {
        datas = value.Split(new string[] { " / " }, StringSplitOptions.None);
        mapMag.prefabPosX = float.Parse(datas[0]);
        mapMag.prefabPosY = float.Parse(datas[1]);
        mapMag.prefabPosZ = float.Parse(datas[2]);
        mapMag.prefabRotX = float.Parse(datas[3]);
        mapMag.prefabRotY = float.Parse(datas[4]);
        mapMag.prefabRotZ = float.Parse(datas[5]);
    }

    void DataDelete(int num)
    {
        prefabDatas.savePrefabDATAInfoListName.Remove(prefabDatas.savePrefabDATAInfoListName[num]);
        prefabDatas.savePrefabDATAInfoListValue.Remove(prefabDatas.savePrefabDATAInfoListValue[num]);
        EditorUtility.SetDirty(prefabDatas); 
        AssetDatabase.SaveAssets();
    }

    bool HIDEINSPECTOR = false;
    string Notice = "\n  ░▒░ Map Maker V1.0\n  ▒█▒ 2015 NEXTI GAMES\n  ░▒░ MANUAL SITE : http://goo.gl/31fRWl";
    public override void OnInspectorGUI() 
    {
        MapMag mapMag = target as MapMag; 
        InputUpdate(mapMag);
        MapDataLoad(mapMag);

        mapMag.transform.hideFlags = HideFlags.HideInInspector;
        EditorGUILayout.Separator();
        CommonEditorUi.DrawLine(new Color(0f, 0f, 0f), 5);
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        Notice = EditorGUILayout.TextArea(Notice, GUILayout.Height(66.0f));
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        if (mapMag.tileList == null)
        {
            GUI.color = mapMag.currentWidth >= 1 ? Color.white : Color.red;
            mapMag.currentWidth = EditorGUILayout.IntField("ㆍMAP WIDTH ", mapMag.currentWidth);
            GUI.color = mapMag.currentHeight >= 1 ? Color.white : Color.red;
            mapMag.currentHeight = EditorGUILayout.IntField("ㆍMAP HEIGHT ", mapMag.currentHeight);
            GUI.color = Color.white;
        }

        GUI.color = mapMag.tile != null ? Color.white : Color.red;

        if (HIDEINSPECTOR)
        {
            mapMag.tile = EditorGUILayout.ObjectField("TILE", mapMag.tile, typeof(GameObject)) as GameObject;
            mapMag.eraserObj = EditorGUILayout.ObjectField("ERASER", mapMag.eraserObj, typeof(GameObject)) as GameObject;

            MapMag.selectPrefab = EditorGUILayout.ObjectField("selectFrefab", MapMag.selectPrefab, typeof(GameObject)) as GameObject;

            mapMag.titleTexture = (Texture2D)EditorGUILayout.ObjectField(mapMag.titleTexture, typeof(Texture2D));
            mapMag.earserTexture = (Texture2D)EditorGUILayout.ObjectField(mapMag.earserTexture, typeof(Texture2D));
            mapMag.randomTexture = (Texture2D)EditorGUILayout.ObjectField(mapMag.randomTexture, typeof(Texture2D));
        }


        tilesizecheck = EditorGUILayout.Toggle("ㆍMAP SIZE ", tilesizecheck);
        if (tilesizecheck)
        {
            aspectRatio = EditorGUILayout.Toggle("ㆍㆍASPECT RATIO", aspectRatio);
            if (aspectRatio)
            {
                mapMag.mapSizeX = 1;
                mapMag.mapSizeZ = 1;
                mapMag.tileSize = EditorGUILayout.Slider("ㆍㆍSIZE", mapMag.tileSize, 0.1f, 10.0f);
            }
            else
            {
                mapMag.mapSizeX = EditorGUILayout.Slider("ㆍㆍSIZE X", mapMag.mapSizeX, 0.1f, 10.0f);
                mapMag.mapSizeZ = EditorGUILayout.Slider("ㆍㆍSIZE Z", mapMag.mapSizeZ, 0.1f, 10.0f);
            }

        }

        mapPosition = EditorGUILayout.Toggle("ㆍMAP POSITION", mapPosition);
        if (mapPosition)
        {
            mapMag.mapPosX = EditorGUILayout.Slider(" - POS-X ", mapMag.mapPosX, -25.0f, 25.0f);
            mapMag.mapPosZ = EditorGUILayout.Slider(" - POS-Z ", mapMag.mapPosZ, -25.0f, 25.0f);
        }

        getInfo = EditorGUILayout.Toggle("ㆍGET MAP DEPTH", getInfo);
        if (getInfo)
        {
            rect = new Rect(0, 0, Screen.width, Screen.height);
            EditorGUIUtility.AddCursorRect(rect, MouseCursor.ArrowPlus);
            SceneView.RepaintAll();
        }

        mapMag.tilePosY = EditorGUILayout.Slider("ㆍMAP DEPTH ", mapMag.tilePosY, 0f, 25.0f);

        EditorGUILayout.Separator();
        CommonEditorUi.DrawSeparator(Color.gray);
        EditorGUILayout.Separator();
        GUI.color = Color.gray;
        EditorGUILayout.HelpBox("Prefab Position & Rotation", MessageType.Info, true);
        GUI.color = Color.white;

        mapMag.prefabPosX = EditorGUILayout.Slider("ㆍPREFAB X-POS", mapMag.prefabPosX, -10f, 10.0f);
        mapMag.prefabPosY = EditorGUILayout.Slider("ㆍPREFAB Y-POS", mapMag.prefabPosY, -10f, 10.0f);
        mapMag.prefabPosZ = EditorGUILayout.Slider("ㆍPREFAB Z-POS", mapMag.prefabPosZ, -10f, 10.0f);

        GUI.color = rotX == false ? Color.white : new Color(0.2f, 0.2f, 0.2f);
        mapMag.prefabRotX = EditorGUILayout.FloatField("ㆍPREFAB X-ROT", mapMag.prefabRotX);

        GUI.color = rotY == false ? Color.white : new Color(0.2f, 0.2f, 0.2f);
        mapMag.prefabRotY = EditorGUILayout.FloatField("ㆍPREFAB Y-ROT", mapMag.prefabRotY);

        GUI.color = rotZ == false ? Color.white : new Color(0.2f, 0.2f, 0.2f);
        mapMag.prefabRotZ = EditorGUILayout.FloatField("ㆍPREFAB Z-ROT", mapMag.prefabRotZ);
        GUI.color = Color.white;

        rotX = EditorGUILayout.Toggle("ㆍRANDOM X-ROT", rotX);
        if (rotX)
        {
            EditorGUILayout.MinMaxSlider(new GUIContent(" " + " - 0 ~ 360"), ref mapMag.randomprefabRotXMinValue, ref mapMag.randomprefabRotXMaxValue, 0, 360);
        }
        rotY = EditorGUILayout.Toggle("ㆍRANDOM Y-ROT", rotY);
        if (rotY)
        {
            EditorGUILayout.MinMaxSlider(new GUIContent(" " + " - 0 ~ 360"), ref mapMag.randomprefabRotYMinValue, ref mapMag.randomprefabRotYMaxValue, 0, 360);
        }
        rotZ = EditorGUILayout.Toggle("ㆍRANDOM Z-ROT", rotZ);
        if (rotZ)
        {
            EditorGUILayout.MinMaxSlider(new GUIContent(" " + " - 0 ~ 360"), ref mapMag.randomprefabRotZMinValue, ref mapMag.randomprefabRotZMaxValue, 0, 360);
        }

        refRotation = EditorGUILayout.Toggle("ㆍREFERENCE ROT", refRotation);
        getTransformInfo = EditorGUILayout.Toggle("ㆍGET TRANSFORM", getTransformInfo);
        if (getTransformInfo)
        {
            rect = new Rect(0, 0, Screen.width, Screen.height);
            EditorGUIUtility.AddCursorRect(rect, MouseCursor.ArrowPlus);
            SceneView.RepaintAll();
            infoText = "- Enable Get Transform";
            GUI.color = new Color(0.6f, 0.6f, 0.6f);
            EditorGUILayout.TextArea("ㆍTRASNFORM INFOMATION\n" + insfactorInfoView, GUILayout.Height(42.0f));
            GUI.color = Color.white;
        }
        else
        {
            infoText = "";
            insfactorInfoView = "";
            infoview = false;
        }


        SavePrefabData = EditorGUILayout.Toggle("ㆍPREFAB DATA", SavePrefabData);
        if (SavePrefabData)
        {

            GUI.color = new Color(0.8f, 0.8f, 0.8f);
            savePrefabDataName = EditorGUILayout.TextField(" - DATA NAME", savePrefabDataName);
            if (GUILayout.Button("SAVE PREFAB DATA"))
            {
                if (savePrefabDataName == "")
                {
                    return;
                }
                prefabDatas.savePrefabDATAInfoListName.Add(savePrefabDataName);
                prefabDatas.savePrefabDATAInfoListValue.Add(mapMag.prefabPosX + " / " + mapMag.prefabPosY + " / " + mapMag.prefabPosZ + " / " + mapMag.prefabRotX + " / " + mapMag.prefabRotY + " / " + mapMag.prefabRotZ);
                EditorUtility.SetDirty(prefabDatas); 
                AssetDatabase.SaveAssets();
                savePrefabDataName = "";
            }

            GUILayout.BeginHorizontal(GUI.skin.box, GUILayout.MinHeight(80), GUILayout.MaxHeight(400.0f));
            scrollVector = GUILayout.BeginScrollView(scrollVector);

            for (int i = 0; i < prefabDatas.savePrefabDATAInfoListName.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.HelpBox(prefabDatas.savePrefabDATAInfoListName[i], MessageType.None);
                if (GUILayout.Button("Load", GUILayout.Width(40)))
                {
                    DataLoad(mapMag, prefabDatas.savePrefabDATAInfoListValue[i]);
                }
                if (GUILayout.Button("-", GUILayout.Width(25)))
                {
                    DataDelete(i);
                }
                EditorGUILayout.EndHorizontal();

            }

            GUI.color = Color.white;
            GUILayout.EndScrollView();
            GUILayout.EndHorizontal();
        }

        EditorGUILayout.Separator();
        CommonEditorUi.DrawSeparator(new Color(0.7f, 0.5f, 0.5f));//줄 넣기
        EditorGUILayout.Separator();

        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("RANDOMPrefabs"), true);
        serializedObject.ApplyModifiedProperties();


        EditorGUILayout.Separator();
        if (GUILayout.Button("PREFAB PALETTE"))
        {
            ShowPrefabs();
        }
        if (GUILayout.Button("CUSTOMIZED PREFAB PALETTE"))
        {
            PrefabButton fabDataWindow = (PrefabButton)EditorWindow.GetWindow(typeof(PrefabButton));
            fabDataWindow.Show();
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Draw Full Tiles", GUILayout.MaxWidth(Screen.width / 2)))
        {
            if (MapMag.selectPrefab == null && mapMag.RANDOMPrefabs[1] == null)
            {
                string title1 = "Error Meassage";
                string msg1 = "프리팹을 선택 하세요.\nPlease select a prefab.";
                EditorUtility.DisplayDialog(title1, msg1, "OK");
                return;
            }
            if (mapMag.tileList == null)
            {
                string title1 = "Error Meassage";
                string msg1 = "맵 생성후 실행 하세요.\nCreate map first, then you can use this function(FULL DRAW).";
                EditorUtility.DisplayDialog(title1, msg1, "OK");
                return;
            }
            makeFullPrefab(mapMag);
        }

        if (GUILayout.Button("Draw Unused Tiles", GUILayout.MaxWidth(Screen.width / 2)))
        {
            EmptyFullDrawPrefab(mapMag);
        }
        GUILayout.EndHorizontal();
        GUI.color = Color.white;
        EditorGUILayout.Separator();
        CommonEditorUi.DrawSeparator(Color.gray);
        EditorGUILayout.Separator();
        GUI.color = Color.yellow;
        EditorGUILayout.BeginHorizontal(); 
        if (GUILayout.Button("Generate Map", GUILayout.MaxWidth(Screen.width / 2)))
        {
            CreateMap(mapMag);
        }
        if (GUILayout.Button("Remove Map", GUILayout.MaxWidth(Screen.width / 2)))
        {
            RemoveMap(mapMag);
        }
        EditorGUILayout.EndHorizontal(); 
        GUI.color = Color.white;
        EditorGUILayout.Separator();
        GUI.color = new Color(0.6f, 0.6f, 0.6f);
        saveMapDataName = EditorGUILayout.TextField(saveMapDataName);
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("SAVE MAP DATA", GUILayout.MaxWidth(Screen.width / 2)))
        {
            if (saveMapDataName == "Map Data Name" || saveMapDataName == "" || saveMapDataName == null)
            {
                string title1 = "Error Meassage";
                string msg1 = "데이터 이름을 입력 하세요.\nPlease enter the data name.";
                EditorUtility.DisplayDialog(title1, msg1, "OK");
                return;
            }

            saveMapDataName = System.Text.RegularExpressions.Regex.Replace(saveMapDataName, @"[^0-9a-zA-Z가-힣]", "");
            SaveMapDATAS(mapMag, saveMapDataName);
        }

        if (GUILayout.Button("LOAD MAP DATA", GUILayout.MaxWidth(Screen.width / 2)))
        {
            LoadScripterbleObject();
        }
        GUILayout.EndHorizontal();
        GUI.color = Color.white;
        EditorGUILayout.Separator();
        GUI.color = new Color(0.4f, 0.7f, 1);
        if (GUILayout.Button("= COMPLETE = "))
        {
            if (mapMag.tileList == null)
            {
                string title1 = "Error Meassage";
                string msg1 = "완성시킬 맵이 없습니다.\nThere are no Map Tiles to Complete";
                EditorUtility.DisplayDialog(title1, msg1, "OK");
                return;
            }

            makePreFabGroup = GameObject.Find("PrefabGroup");
            parentPrefab = makePreFabGroup.GetComponentsInChildren<Transform>();

            if (parentPrefab.Length <= 1)
            {
                string title1 = "Error Meassage";
                string msg1 = "완성시킬 맵이 없습니다.\nThere are no Map Tiles to Complete";
                EditorUtility.DisplayDialog(title1, msg1, "OK");
                return;
            }

            if (parentPrefab.Length > 1)
            {
                if (parentPrefab != null && parentPrefab.Length > 1)
                {
                    string title = "Save Map Prefab Group";
                    string msg = "현재 맵 작업을 완성 하시겠습니까?\nDo you want to finish current map work?";
                    if (EditorUtility.DisplayDialog(title, msg, "yse", "no") == false)
                    {
                        return; 
                    }
                    CompletMapf(mapMag);
                    RemoveTileMap(mapMag);

                }
                mapMag.CreateObjectDic.Clear();
                mapMag.prefabInfo.Clear();
                mapMag.mapPosX = 0;
                mapMag.mapPosZ = 0;
                mapMag.tileList = null;
            }

        }
        GUI.color = Color.white;
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
    }
}
