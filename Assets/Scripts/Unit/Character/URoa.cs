using UnityEngine;

namespace Game.Unit.Character {
    using Type;
    using Ability;
    using Ability.Spell;

    public class URoa : UUnit {

        private enum ActionId {
            Idle = 0,
            Movement = 1,
            Attack = 2,
            Death = 3,
        }


        private RoaType my_type;



        public override void confirm ( ) {
            base.confirm ( );

            AbilityCaster.add ( this, APurgatoryArea.ID );
            my_type = unit_type as RoaType;
        }


        protected override void active_rotate ( ) {
            if( action_doing ) {
                return;
            }

            base.active_rotate ( );
        }


        protected override void active_move ( ) {
            if ( unit_status.input == Vector2.zero || action_doing) {
                return;
            }

            base.active_move ( );
            if ( get_animator ( ).GetInteger ( state_para[AnimatorParameter.OrderId] ) != (int)ActionId.Movement ) {
                action_animation ( (int)ActionId.Movement );
            }
        }


        protected override void active_attack ( ) {
            if (doing) {
                return;
            }
            base.active_attack ( );
            action_animation ( (int)ActionId.Attack );
            action_doing = true;
        }

        protected override void active_dead ( ) {
            base.active_dead ( );

            if ( get_animator ( ).GetInteger ( state_para[AnimatorParameter.OrderId] ) != (int)ActionId.Death ) {
                action_animation ( (int)ActionId.Death );
            }
        }
    }
}