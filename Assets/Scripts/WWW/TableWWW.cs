using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TableWWW : MonoBehaviour
{
    const string TableKey = @"1ZNmDyrSTJC_R4Fw1-JgEK4W6R7akh8r1vbF0tZ21RoU";
    const string strURLBase = @"http://spreadsheets.google.com/a/google.com/tq?key={0}&gid={1}";

    public void Req<T>(int a_nTableID, Dictionary<int, T> a_refContainer, Action<bool> a_fpCallback)
    {
        StartCoroutine(Request<T>(a_nTableID, a_refContainer, a_fpCallback));
    }

    IEnumerator Request<T>(int a_nTableID, Dictionary<int, T> a_refContainer, Action<bool> a_fpOnResponse)
    {
        bool bResult = true;
        string url = string.Format(strURLBase, TableKey, a_nTableID);

        WWW www = new WWW(url);

        while (www.isDone == false)
        {
            yield return null;
        }

        if (string.IsNullOrEmpty(www.error) == false)
        {
            if (a_fpOnResponse != null)
            {
                Debug.LogError("net error");
                a_fpOnResponse(false);
                yield break;
            }
        }

        string d = www.text;

        try
        {
            // 필요없는 문자열 제거
            int nStart = d.IndexOf("(");
            int nEnd = d.IndexOf(");");
            ++nStart;
            
            string data = d.Substring(nStart, nEnd - nStart);
            // 실제 값 파싱
            List<string> liValueName = new List<string>(); // 변수명
            List<List<string>> liValues = new List<List<string>>(); // 변수값

            var mapParsed = MiniJSON.Json.Deserialize(data) as Dictionary<string, object>;
            //Debug.Log(mapParsed);
            var map = (Dictionary<string, object>)mapParsed["table"];
            var liName = (List<object>)map["cols"];
            var liVal = (List<object>)map["rows"];
            
            // 임시 캐싱 : 테이블명
            for (int i = 0; i < liName.Count; ++i)
            {
                var m1 = (Dictionary<string, object>)liName[i];
                liValueName.Add((string)m1["label"]);
            }

            // 임시 캐싱 : 각 로우의 값들
            for (int i = 0; i < liVal.Count; ++i)
            {
                var m2 = (Dictionary<string, object>)liVal[i];
                var li = (List<object>)m2["c"];

                liValues.Add(new List<string>());

                for (int j = 0; j < li.Count; ++j)
                {
                    var v = (Dictionary<string, object>)li[j];
                    liValues[i].Add(v["v"].ToString());
                }
            }

            // 캐싱한 값으로부터 클래스 생성
            int nValCount = liValues.Count;

            for (int i = 0; i < nValCount; ++i)
            {
                T val = (T)GetInstance(typeof(T).FullName, liValues[i].ToArray());
                a_refContainer.Add(int.Parse(liValues[i][0]), val);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            Debug.LogError("Table Value error");
            bResult = false;
        }

        if (a_fpOnResponse != null)
        {
            a_fpOnResponse(bResult);
        }
    }
    
    // 이름, 인자의 string배열로 클래스 생성
    public object GetInstance(string strFullyQualifiedName, string[] arrArg)
    {
        Type type = Type.GetType(strFullyQualifiedName);
        if (type != null)
            return Activator.CreateInstance(type, arrArg);

        foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
        {
            type = asm.GetType(strFullyQualifiedName);
            if (type != null)
                return Activator.CreateInstance(type, arrArg);
        }

        return null;
    }
}