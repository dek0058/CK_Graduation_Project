﻿using UnityEngine;

namespace Game.Unit.Missile {
    using JToolkit.Math;
    using Game.Unit;
    using Game.Unit.Type;

    public class UBlueNote2 : UUnit {

        private MelodyMissileType my_type;


        protected override void active_rotate ( ) {
            unit_status.angle = unit_status.look_at;
            my_type.transform.localEulerAngles = new Vector3 ( 0f, 0f, unit_status.angle - 180f );
        }


        protected override void active_move ( ) {
            Vector2 location = Polar.location ( 1f, unit_status.angle );
            unit_status.direction = location;    // 미사일이 정면을 향해서만 움직인다.
            movement_system.move ( (unit_status.direction / unit_status.direction.magnitude) * unit_status.mspeed );
        }


        public override void confirm ( ) {
            base.confirm ( );

            my_type = unit_type as MelodyMissileType;

            movement_system.event_collision_enter += my_type.on_collision;
        }


        ////////////////////////////////////////////////////////////////////////////
        ///                               Unity                                  ///
        ////////////////////////////////////////////////////////////////////////////
    }
}