using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

namespace Game.Unit {
    public abstract class UnitType : MonoBehaviour {

        public Dictionary<string, UnityEvent> schedules = new Dictionary<string, UnityEvent> ( );



        public void add ( string key, UnityAction callback ) {
            if(schedules.ContainsKey(key)) {
                return;
            }
            UnityEvent uevent = new UnityEvent ( );
            uevent.AddListener ( callback );
            
            schedules.Add ( key, uevent );
        }
    }
}
