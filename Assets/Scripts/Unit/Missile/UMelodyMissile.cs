using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Game.Unit.Missile {
    using JToolkit.Utility;
    using JToolkit.Math;
    using Game.Unit;
    using Game.Unit.Type;

    public class UMelodyMissile : Unit {


        private MelodyMissileType my_type;


        protected override void active_rotate ( ) {
            float z = get_rotation().z;
            float gap = Mathf.DeltaAngle ( z, unit_status.look_at );
            unit_model.transform.Rotate ( 0f, 0f, gap * unit_status.rspeed * Time.fixedDeltaTime );
            unit_status.angle = get_rotation ( ).z;
        }


        protected override void active_move ( ) {
            Vector2 location = Polar.location ( 1f, get_rotation ( ).z );
            unit_status.direction = location;    // 미사일이 정면을 향해서만 움직인다.
            movement_system.move ( (unit_status.direction / unit_status.direction.magnitude) * unit_status.mspeed );
        }


        public override void confirm ( ) {
            base.confirm ( );

            my_type = unit_type as MelodyMissileType;
        }


        ////////////////////////////////////////////////////////////////////////////
        ///                               Unity                                  ///
        ////////////////////////////////////////////////////////////////////////////

        private void Awake ( ) {
            confirm ( );
        }


        private void Update ( ) {
            order ( );
        }


        private void FixedUpdate ( ) {
            active ( );
        }
    }
}
