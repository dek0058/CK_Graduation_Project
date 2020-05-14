using UnityEngine;

namespace Game.Unit.Character
{
    using Type;

    public class UBejjangi : UUnit
    {
        private enum ActionId
        {
            Idle = 0,
            Movement = 1,
        }
        
        private BejjangiType my_type;
        
        public override void confirm()
        {
            base.confirm();
            
            my_type = unit_type as BejjangiType;
        }

        protected override void active_rotate()
        {
            base.active_rotate();
        }

        protected override void active_move()
        {
            if (unit_status.input == Vector2.zero)
            {
                return;
            }

            base.active_move();
            if (get_animator().GetInteger(state_para[AnimatorParameter.OrderId]) != (int)ActionId.Movement)
            {
                action_animation((int)ActionId.Movement);
            }
        }
        
        protected override void active_update()
        {
            base.active_update();
        }

        protected override void active_fixedupdate()
        {
            base.active_fixedupdate();
        }

        protected override void active_lateupdate()
        {
            base.active_lateupdate();
        }
    }
}