using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.AI {

    /// <summary>
    /// 임시 List 데이터
    /// </summary>
    [Serializable]
    public class TemporaryData {
        public string time;
        public bool use;
        public int pattern;

        public float tick {
            get {
                int colon = time.IndexOf ( ':' );
                int sd = time.IndexOf ( '~' );
                string msg = sd == -1 ? time : time.Substring ( 0, sd );
                if(colon == -1) {
                    return float.Parse ( msg );
                } else {
                    float m = float.Parse ( msg.Substring ( 0, colon ) );
                    float s = float.Parse ( msg.Substring ( colon + 1 ) );
                    return (m * 60) + s;
                }
            }
        }
    }

    /// <summary>
    /// 임시 테스트용 보스 AI 스크립트
    /// </summary>
    public class TemporaryBossAI : MonoBehaviour {

        public List<TemporaryData> tdata = new List<TemporaryData> ( );

        public Queue<TemporaryData> p_queue = new Queue<TemporaryData> ( );

        public void start ( ) {
            for(int i = 0; i < tdata.Count; ++i ) {
                p_queue.Enqueue ( tdata[i] );
            }
        }

        public void reset ( ) {
            p_queue.Clear ( );
        }

    }
}
