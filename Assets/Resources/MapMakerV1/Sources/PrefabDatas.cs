using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

[Serializable]
public class PrefabDatas : ScriptableObject
{

    public Dictionary<string, string> savePrefabDATAInfoDic = new Dictionary<string, string>();
    public List<string> savePrefabDATAInfoListName = new List<string>();
    public List<string> savePrefabDATAInfoListValue = new List<string>();
    public List<Texture2D> saveTextureData = new List<Texture2D>();

}



