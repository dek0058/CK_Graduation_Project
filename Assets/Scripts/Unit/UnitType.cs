using UnityEngine;
using System;
using System.Collections.Generic;

namespace Game.Unit {
    public abstract class UnitType : MonoBehaviour {

        public const string Begin = "begin";
        public const string End = "end";


        public Dictionary<string, Action> schedules = new Dictionary<string, Action> ( );



        public void add ( string key, Action callback ) {
            if(schedules.ContainsKey(key)) {
                return;
            }
            schedules.Add ( key, callback );
        }


        public virtual void action_begin ( ) {
            if ( schedules.ContainsKey ( Begin ) ) {
                schedules[Begin]?.Invoke ( );
            }
        }


        public virtual void action_end ( ) {
            if ( schedules.ContainsKey ( End ) ) {
                schedules[End]?.Invoke ( );
            }
        }
    }
}
