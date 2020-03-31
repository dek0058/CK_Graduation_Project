using UnityEngine;

namespace Game.Unit.Type {
    using Game.Unit;
    using Management;

    public class MelodyMissileType : UnitType {


        public void on_collision ( Collider2D collision ) {
            Unit target = collision.GetComponentInParent<Unit> ( );
            if( target == null) {
                if ( collision.gameObject.layer == (int)GameLayer.Map_Border ||
                    collision.gameObject.layer == (int)GameLayer.Door) {
                    unit.destroy ( );
                }

            } else {

            }
        }

    }
}