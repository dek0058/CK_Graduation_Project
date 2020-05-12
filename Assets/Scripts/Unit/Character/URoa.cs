using UnityEngine;

namespace Game.Unit.Character {
    using Game.Unit.Type;
    using JToolkit.Math;

    public class URoa : UUnit {

        private enum ActionId {
            Idle = 0,
            Movement = 1,
            Attack = 2,
        }

        
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
        }


        protected override void active_move() {
            if(unit_status.input == Vector2.zero) {
                return;
            }

            base.active_move ( );
            if ( get_animator ( ).GetInteger ( state_para[AnimatorParameter.OrderId] ) != (int)ActionId.Movement ) {
                action_animation ( (int)ActionId.Movement );
            }
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