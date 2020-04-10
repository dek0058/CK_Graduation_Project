using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game.Unit.Type {
    using Game.Unit;
    using Management;

    public class MelodyMissileType : UnitType {

        private Dictionary<UUnit, Coroutine> collisions_info = new Dictionary<UUnit, Coroutine> ( );


        public void on_collision ( Collider2D collision ) {
            UUnit target = collision.GetComponentInParent<UUnit> ( );
            if( target == null) {
                if ( collision.gameObject.layer == (int)GameLayer.Map_Border ||
                    collision.gameObject.layer == (int)GameLayer.Door) {
                    unit.destroy ( );
                }
            }
        }


        private IEnumerator Ecollision_check ( UUnit target ) {
            bool loop = true;

            while(loop) {
                if ( unit.player.team != target.player.team ) {
                    if ( unit.movement_system.collisions.Contains ( target ) ) {
                        float damage = unit.unit_status.damage + unit.unit_status.add_damage + unit.unit_status.rate_damage;
                        UUnit source = unit.owner == null ? unit : unit.owner;
                        if ( target.damage ( DamageInfo.Type.Melee, damage, source ) ) {
                            loop = false;
                            unit.destroy ( );
                        }
                    }
                }
                yield return new WaitForEndOfFrame ( );
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        ///                               Unity                                  ///
        ////////////////////////////////////////////////////////////////////////////

        private void OnTriggerEnter2D ( Collider2D collision ) {
            UUnit target = collision.GetComponentInParent<UUnit> ( );
            if(target != null) {
                
                if ( !collisions_info.ContainsKey ( target ) ) {
                    collisions_info.Add ( target, StartCoroutine ( Ecollision_check ( target ) ) );
                }
            }
        }

        private void OnTriggerExit2D ( Collider2D collision ) {
            UUnit target = collision.GetComponentInParent<UUnit> ( );
            if(target != null) {
                if ( collisions_info.ContainsKey ( target ) ) {
                    StopCoroutine ( collisions_info[target] );
                    collisions_info.Remove ( target );
                }
            }
        }

    }
}