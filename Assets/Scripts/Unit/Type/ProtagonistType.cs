using System.Collections.Generic;

namespace Game.Unit.Type {
    using Game.Unit;

    public class ProtagonistType : UnitType {

        // Attack
        public const string Action_Attack = "action_attack";
        public const string Action_Attack_Stop = "action_attack_stop";

        public bool do_attack = false;
        public List<UUnit> attacked_units = new List<UUnit> ( );
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
    }
}
