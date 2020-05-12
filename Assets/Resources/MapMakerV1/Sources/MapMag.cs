using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MapMag : MonoBehaviour {
    
    public int currentWidth = 10; 
    public int currentHeight = 10;

    public float tileSize; 
    public float mapSizeX;
    public float mapSizeZ;
    public float tilePosY; 
    public float prefabPosX;
    public float prefabPosY;
    public float prefabPosZ;
    public float prefabRotX;
    public float prefabRotY;
    public float prefabRotZ;
    public float mapPosX;
    public float mapPosZ;
    public float randomprefabRotXMinValue;
    public float randomprefabRotXMaxValue;
    public float randomprefabRotYMinValue;
    public float randomprefabRotYMaxValue;
    public float randomprefabRotZMinValue;
    public float randomprefabRotZMaxValue;

    public Dictionary<string, GameObject> CreateObjectDic = new Dictionary<string, GameObject>();
    public Dictionary<string, string> prefabInfo = new Dictionary<string, string>();
    public Dictionary<string, string> savePrefabDATAInfo = new Dictionary<string, string>();

    public Texture2D textureTest;
    public Texture2D titleTexture;
    public Texture2D earserTexture;
    public Texture2D randomTexture;
    
    public GameObject tile = null;
    public GameObject eraserObj;
    public static GameObject selectPrefab;

    public GameObject[] PREFABS;
    public Texture2D[] texture = new Texture2D[5];

    public Dictionary<int, Texture2D> dicTexture = new Dictionary<int, Texture2D>();
    public List<GameObject> RANDOMPrefabs = new List<GameObject>();
    public List<Texture2D> TEXTURES = new List<Texture2D>();
    public Transform myTR;

    public static string teeeeee;
    public GameObject[,] tileList = null; 
  
    public void ArrResrt()
    {
        if (selectPrefab != null && selectPrefab.gameObject.name != "ERASEROBJECT")
        {
          //  selectPrefab = null;
        }
        Array.Clear(PREFABS, 0, PREFABS.Length);
        dicTexture.Clear();
    }
}

