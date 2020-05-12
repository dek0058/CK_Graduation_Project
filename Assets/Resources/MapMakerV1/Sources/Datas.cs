using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;


[Serializable]
public class Datas : ScriptableObject 
{
    public string MAPDATA;
    public List<string> PREFABDATA = new List<string>();
    public List<UnityEngine.Object> PrefabSources = new List<UnityEngine.Object>();
    public Dictionary<string, object> PREFABSOURCE = new Dictionary<string, object>();

}
