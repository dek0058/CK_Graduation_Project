using UnityEngine;
using UnityEditor;
using System.Collections;

public class MapMaker : Editor
{
    [MenuItem("Tools/NEXTI_MapMaker")]
    public static void CreateMapMaker()
    {
        GameObject GENMaker = Instantiate(Resources.Load("MapMakerV1/MapMaker"), Vector3.zero, Quaternion.identity) as GameObject;
        GENMaker.name = "MapMaker";
    }
}
