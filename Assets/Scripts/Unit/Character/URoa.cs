using UnityEngine;

namespace Game.Unit.Character {
    using Game.Unit.Type;
    using JToolkit.Math;

    public class URoa : UUnit {

        
        private RoaType my_type;

    

        public override void confirm() {
            base.confirm();

            if(unit_data == null) {
                // TODO : 유닛 데이타 경로
                // unit_data = Resources.Load<UnitData>("PATH");
            }


            
            my_type = unit_type as RoaType;
        }


        protected override void active_rotate() {
            base.active_rotate();
            float angle = Angle.trim ( unit_status.angle ) / 360f;
            get_animator ( ).SetFloat ( Animation_Angle_Hash, angle );
        }


        protected override void active_move() {
            if(get_animator_state(0).IsTag(state_tag[AnimatorTag.Movement]) ||
                get_animator_nextstate ( 0 ).IsTag ( state_tag[AnimatorTag.Movement] ) ) { 
                base.active_move ( );
            }

            // HACK : 수정예정

            get_animator ( ).SetBool ( "isMovement", unit_order.execute ( ) == OrderId.Move );
        }


        protected override void active_update() {
            base.active_update();

        }

        protected override void active_fixedupdate() {
            base.active_fixedupdate();
        }

        protected override void active_lateupdate() {
            base.active_lateupdate();
        }
    }
}