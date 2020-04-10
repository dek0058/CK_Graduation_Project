using UnityEngine;

namespace Game.Unit.Missile {
    using JToolkit.Math;
    using Game.Unit;
    using Game.Unit.Type;

    public class URedNote3 : UUnit {


        private MelodyMissileType my_type;



        private void reflection ( UUnit source, UUnit target ) {
            float angle = Angle.target_to_angle ( transform.position, owner.get_attech_point ( AttechmentPoint.Origin ).position ) * Mathf.Rad2Deg - 90f;
            float dist = Vector2.Distance ( transform.position, owner.get_attech_point ( AttechmentPoint.Origin ).position );
            float speed = (unit_status.mspeed + unit_status.add_mspeed + unit_status.rate_mspeed) * Time.fixedDeltaTime;
            float tick = speed > 0f ? dist / speed : 0f;

            player = source.player;
            owner = source;
            rotate ( angle );
            unit_status.is_invincible = true;

            movement_system.lerp_flying ( owner.get_attech_point ( AttechmentPoint.Chest ).localPosition.y, tick > 0f ? 1f / tick : 0f );
        }


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

            event_damaged += reflection;

            movement_system.event_collision_enter += my_type.on_collision;
        }


        ////////////////////////////////////////////////////////////////////////////
        ///                               Unity                                  ///
        ////////////////////////////////////////////////////////////////////////////
    }
}
