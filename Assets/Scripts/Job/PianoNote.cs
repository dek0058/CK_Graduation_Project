using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Job {
    [Serializable]
    public class PianoNoteData {
        public PianoNoteData ( string _time, int _pattern ) => (time, pattern) = (_time, _pattern);
        public string time {
            get; set;
        }
        public int pattern {
            get; set;
        }

        public float tick {
            get {
                int colon = time.IndexOf ( ':' );
                int sd = time.IndexOf ( '~' );
                string msg = sd == -1 ? time : time.Substring ( 0, sd );
                if ( colon == -1 ) {
                    return float.Parse ( msg );
                } else {
                    float m = float.Parse ( msg.Substring ( 0, colon ) );
                    float s = float.Parse ( msg.Substring ( colon + 1 ) );
                    return (m * 60) + s;
                }
            }
        }
    }

    [SerializeField]
    public class PianoNote {
        [SerializeField]
        private List<PianoNoteData> save_notes = new List<PianoNoteData> ( );
        public Queue<PianoNoteData> note_datas = new Queue<PianoNoteData> ( );

        public int count {
            get => note_datas.Count;
        }

        public PianoNoteData peek {
            get => note_datas.Peek ( );
        }

        public PianoNoteData dequeue {
            get => note_datas.Dequeue ( );
        }


        public void load ( string path ) {
            List<Dictionary<string, object>> note_list = JToolkit.Utility.CSVReader.read ( path );
            for ( int i = 0; i < note_list.Count; ++i ) {
                save_notes.Add ( new PianoNoteData ( note_list[i]["Time"].ToString ( ), Convert.ToInt32 ( note_list[i]["Pattern"] ) ) );
            }
        }


        public void initialize ( ) {
            clear ( );
            for ( int i = 0; i < save_notes.Count; ++i ) {
                note_datas.Enqueue ( save_notes[i] );
            }
        }


        public void clear ( ) {
            note_datas.Clear ( );
        }
    }
}