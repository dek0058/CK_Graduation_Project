using UnityEngine;
using System.Collections;

namespace Game.Unit.Character {
    using JToolkit.Utility;
    using JToolkit.Math;
    using Game.Unit;
    using Game.Unit.Type;
    using Game.Management;
    using JToolkit.Testing;

    public class UProtagonist : Unit {

        /// <summary>
        /// Protagonist가 가진 Animator Paramter
        /// </summary>
        public enum AnimatorParameter {
            Angle,
            Run,
            Attack,
        }

        readonly private EnumDictionary<AnimatorParameter, int> parameter_hash = new EnumDictionary<AnimatorParameter, int> {
            { AnimatorParameter.Angle, Animator.StringToHash("Angle") },
            { AnimatorParameter.Run, Animator.StringToHash("Run") },
            { AnimatorParameter.Attack, Animator.StringToHash("Attack") },
        };


        private ProtagonistType my_type;

        public Transform weapon_pivot;

        private bool do_attack = false;


        public void attack ( ) {
            if(get_animator_state(0).IsTag(state_tag[AnimatorTag.Attack])) {        // 이미 공격에 성공했으므로
                active_attack ( );
                get_animator ( ).ResetTrigger ( parameter_hash[AnimatorParameter.Attack] );
                unit_order.set_order ( Order_Id.Attack, false );
                return;
            }

            if ( do_attack ) {
                return;
            }

            get_animator ( ).SetTrigger ( parameter_hash[AnimatorParameter.Attack] );
            StartCoroutine ( Eattack_cooltime ( ) );
        }


        public void on_damage ( float damage, float angle, float radius, LayerMask layer) {
            Collider2D[] colliders = Physics2D.OverlapCircleAll ( get_position ( ), radius, layer );

            Vector2 unit_target = new Vector2 ( -get_position ( ).x, get_position ( ).y );

            foreach ( var col in colliders ) {

                Unit unit = col.GetComponent<Unit> ( );
                if ( unit == null || my_type.attacked_units.Contains ( unit ) ) {
                    continue;
                }
                if ( unit.player.team == player.team ) {
                    continue;
                }

                Vector2 position = unit.get_position ( );
                Vector2 ang_target = new Vector2 ( -position.x, position.y );

                float tangle = (Angle.target_to_angle ( unit_target, ang_target ) * Mathf.Rad2Deg) - 90f;
                float delta = Mathf.Abs ( Mathf.DeltaAngle ( unit_status.angle, tangle ) );
                if ( delta > angle ) {     //  타겟이 반경을 넘어갔을 경우
                    continue;
                }

                my_type.attacked_units.Add ( unit );
                unit.damage ( DamageInfo.Type.Melee, damage, this );
            }
        }


        protected override void active_rotate ( ) {
            unit_status.angle = unit_status.look_at;
        }


        protected override void active_move ( ) {
            if(get_animator_state(0).IsTag(state_tag[AnimatorTag.Movement]) ||
                get_animator_nextstate ( 0 ).IsTag ( state_tag[AnimatorTag.Movement] ) ) { 
                base.active_move ( );
            }

            get_animator ( ).SetBool ( parameter_hash[AnimatorParameter.Run], unit_order.get_order ( Order_Id.Move ) );
        }


        public override void confirm ( ) {
            base.confirm ( );

            my_type = unit_type as ProtagonistType;
            my_type.add ( ProtagonistType.Action_Attack, action_attack );
            my_type.add ( ProtagonistType.Action_Attack_Stop, action_attack_stop );
        }


        protected override void active_update ( ) {
            base.active_update ( );

            if ( unit_order.get_order ( Order_Id.Attack ) ) {
                attack ( );
            }
        }


        protected override void active_fixedupdate ( ) {
            base.active_fixedupdate ( );
        }


        protected override void active_lateupdate ( ) {
            base.active_lateupdate ( );

            float angle = Angle.trim ( unit_status.angle ) / 360f;
            get_animator ( ).SetFloat ( parameter_hash[AnimatorParameter.Angle], angle );
        }


        // Animation Action
        private void action_attack ( ) {
            StartCoroutine ( Eaction_attack ( ) );
        }
        private void action_attack_stop ( ) {
            my_type.do_attack = false;
        }

        // IEnumerator

        private IEnumerator Eaction_attack ( ) {
            if ( my_type.do_attack ) {
                yield break;
            }
            my_type.do_attack = true;
            my_type.attacked_units.Clear ( );
            unit_order.set_active ( UnitOrder.Active.Rotate, true );

            

            // - HACK
           //weapon_pivot.gameObject.SetActive ( true );
            // - 

            float damage = unit_status.damage + unit_status.add_damage + unit_status.rate_damage;
            float angle = 90f;
            float range = 1f;
            LayerMask layer = 1 << (int)GameLayer.Unit_Collider;

            while ( my_type.do_attack ) {
                if ( !get_animator_nextstate ( 0 ).IsTag ( state_tag[AnimatorTag.Attack] ) &&
                    !get_animator_state ( 0 ).IsTag ( state_tag[AnimatorTag.Attack] ) ) {
                    action_attack_stop ( );
                    continue;
                }
                on_damage ( damage, angle, range, layer );
                yield return new WaitForEndOfFrame ( );
            }

            while( get_animator_state ( 0 ).IsTag ( state_tag[AnimatorTag.Attack] ) ) {
                if( get_animator ( ).IsInTransition ( 0 ) ) {
                    break;
                }
                yield return new WaitForEndOfFrame ( );
            }

            unit_order.set_active ( UnitOrder.Active.Rotate, false );

            // - HACK
            //weapon_pivot.gameObject.SetActive ( false );
            // - 

        }


        private IEnumerator Eattack_cooltime ( ) {
            if ( do_attack ) {
                yield break;
            }
            do_attack = true;

            float time = 0f;
            float speed = unit_status.aspeed > 0f ? unit_status.aspeed : 0.001f;
            float aspeed = speed + (unit_status.add_aspeed + unit_status.rate_aspeed);
            float result = aspeed > 0f ? aspeed : 0.001f;
            float cooltime = unit_status.attack_cooltime / result;
            while ( do_attack ) {
                if ( time >= cooltime ) {
                    do_attack = false;
                    continue;
                }
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame ( );
            }
        }


        ////////////////////////////////////////////////////////////////////////////
        ///                               Unity                                  ///
        ////////////////////////////////////////////////////////////////////////////
    }
}
