using UnityEngine;

namespace Game.Unit.Character {
    using Type;

    public class UPomp : UUnit {

        private enum ActionId {
            Idle = 0,
            Movement = 1,
        }

        private PompType my_type;
        private float angle;



        public override void confirm ( ) {
            base.confirm ( );

            my_type = unit_type as PompType;
        }


        protected override void active_rotate ( ) {
            angle = JToolkit.Math.Angle.trim ( unit_status.look_at );
        }


        protected override void active_move ( ) {
            if ( unit_status.input == Vector2.zero ) {
                return;
            }

            base.active_move ( );
            if ( get_animator ( ).GetInteger ( state_para[AnimatorParameter.OrderId] ) != (int)ActionId.Movement ) {
                action_animation ( (int)ActionId.Movement );
            }
        }


        protected override void active_update ( ) {
            base.active_update ( );
            get_animator ( ).SetFloat ( "Angle", angle );
        }
    }
}
