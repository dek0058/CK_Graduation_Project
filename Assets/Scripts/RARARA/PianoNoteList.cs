using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PianoNodeData
{
    public PianoNodeData(string _time, int _pattern) => (time, pattern) = (_time, _pattern);
    public string time { get; set; }
    public int pattern { get; set; }

    public float tick
    {
        get
        {
            int colon = time.IndexOf(':');
            int sd = time.IndexOf('~');
            string msg = sd == -1 ? time : time.Substring(0, sd);
            if (colon == -1)
            {
                return float.Parse(msg);
            }
            else
            {
                float m = float.Parse(msg.Substring(0, colon));
                float s = float.Parse(msg.Substring(colon + 1));
                return (m * 60) + s;
            }
        }
    }
}

public class PianoNoteList : MonoBehaviour
{
    public List<PianoNodeData> nodes = new List<PianoNodeData>();

    private void Start()
    {
        List<Dictionary<string, object>> note_list = JToolkit.Utility.CSVReader.read("DataTable/PianoNoteTable");

        for (int i = 0; i < note_list.Count; i++)
        {
            nodes.Add(new PianoNodeData(note_list[i]["Time"].ToString(), Convert.ToInt32(note_list[i]["Pattern"])));
        }
    }
}