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
        public const string Begin_Hit = "begin_hit";
        public const string End_Hit = "end_hit";
        public int hit_count;

        public void begin_hit ( ) {
            if ( schedules.ContainsKey ( Begin_Hit ) ) {
                schedules[Begin_Hit]?.Invoke ( );
            }
        }

        public void end_hit ( ) {
            if ( schedules.ContainsKey ( End_Hit ) ) {
                schedules[End_Hit]?.Invoke ( );
            }
        }
        //
    }
}
