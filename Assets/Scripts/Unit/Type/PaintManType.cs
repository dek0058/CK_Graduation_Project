using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Game.Unit.Type {
    public class PaintManType : UnitType {
        

        // Attack
        public const string Action_Attack = "action_attack";
        public const string Action_Attack_Stop = "action_attack_stop";

        public bool do_attack = false;
        public List<Unit> attacked_units = new List<Unit> ( );
        public void action_attack ( ) {
            if ( schedules.ContainsKey ( Action_Attack ) ) {
                schedules[Action_Attack]?.Invoke ( );
            }
        }

        public void action_attack_stop ( ) {
            if ( schedules.ContainsKey ( Action_Attack_Stop ) ) {
                schedules[Action_Attack_Stop]?.Invoke ( );
            }
        }
        //

        // Hit
        public const float Weak_Duration = 5f;
        public const int Weak_Count = 3;
        //
    }
}
