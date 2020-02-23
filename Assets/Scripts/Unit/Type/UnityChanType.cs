
namespace Game.Unit.Type {
    using Game.Unit;    

    public class UnityChanType : UnitType {

        public const string Attack1 = "attack1";

        public const string Attack_Stop = "attack_stop";

        public void attack1 ( ) {
            if ( schedules.ContainsKey ( Attack1 ) ) {
                schedules[Attack1]?.Invoke ( );
            }
        }

        public void attack_stop ( ) {
            if ( schedules.ContainsKey ( Attack_Stop ) ) {
                schedules[Attack_Stop]?.Invoke ( );
            }
        }
    }
}
