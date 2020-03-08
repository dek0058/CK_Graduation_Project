using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Game.Unit.Character {
    using JToolkit.Utility;
    using JToolkit.Math;
    using Game.Management;
    using Game.Unit;
    using Game.Unit.Type;
    using JToolkit.Testing;

    public class UPaintMan : Unit {

        public const float weak_duration = 5f;
        public const int weak_count = 3;

        public enum AnimatorParameter {
            Walk,
            Weak,
            Dead,
            Attack,
            Hit,

            TWeak,
            TDead,
        }
        private EnumDictionary<AnimatorParameter, int> parameter_hash = new EnumDictionary<AnimatorParameter, int> {
            {AnimatorParameter.Walk, Animator.StringToHash("Walk") },
            {AnimatorParameter.Weak, Animator.StringToHash("Weak") },
            {AnimatorParameter.Dead, Animator.StringToHash("Dead") },
            {AnimatorParameter.Attack, Animator.StringToHash("Attack") },
            {AnimatorParameter.Hit, Animator.StringToHash("Hit") },

            {AnimatorParameter.TWeak, Animator.StringToHash("TWeak") },
            {AnimatorParameter.TDead, Animator.StringToHash("TDead") },
        };


        private PaintManType my_type;

        private bool do_attack = false;
        private bool do_weaked = false;

        private Obj.WaterBottle bottle = null;


        public void attack ( ) {
            if ( get_animator_state ( 0 ).IsTag ( state_tag[AnimatorTag.Attack] ) ) {        // 이미 공격에 성공했으므로
                get_animator ( ).ResetTrigger ( parameter_hash[AnimatorParameter.Attack] );
                set_order ( Order_Id.Attack, false );
                return;
            }

            if ( do_attack ) {
                return;
            }

            get_animator ( ).SetTrigger ( parameter_hash[AnimatorParameter.Attack] );
            StartCoroutine ( Eattack_cooltime ( ) );
        }


        private void on_damage ( float damage, float radius, LayerMask layer) {
            Collider2D[] colliders = Physics2D.OverlapCircleAll ( get_position ( ), radius, layer );
            Debug.Log ( "A" );
            foreach(var col in colliders) {
                Unit unit = col.GetComponent<Unit> ( );
                if (unit == null || my_type.attacked_units.Contains(unit)) {
                    continue;
                }
                if(unit.player.team == player.team) {
                    continue;
                }
                
                my_type.attacked_units.Add ( unit );
                unit.damage ( damage, this );

                float angle = (Angle.target_to_angle ( get_position ( ), unit.get_position ( ) ) * Mathf.Rad2Deg) - 90f;
                Vector2 force = Polar.location ( 3f, angle );
                unit.movement_system.add_force ( force );
            }
        }


        public void weak ( ) {
            StartCoroutine ( Eweaked_condition ( weak_duration ) );
        }


        protected override void active_rotate ( ) {
            unit_status.angle = unit_status.look_at;
        }



        protected override void active_move ( ) {
            if ( get_animator_state ( 0 ).IsTag ( state_tag[AnimatorTag.Movement] ) ||
                get_animator_nextstate ( 0 ).IsTag ( state_tag[AnimatorTag.Movement] ) ) {
                base.active_move ( );
            }

            get_animator ( ).SetBool ( parameter_hash[AnimatorParameter.Walk], unit_order.get_order ( Order_Id.Move ) );
        }


        protected override void active_dead ( ) {
            base.active_dead ( );
            bool dead = get_animator ( ).GetBool ( parameter_hash[AnimatorParameter.Dead] );
            if(!dead) {
                get_animator ( ).SetBool ( parameter_hash[AnimatorParameter.Dead], true );
                get_animator ( ).SetTrigger ( parameter_hash[AnimatorParameter.TDead] );
            }
        }


        protected override void active_alive ( ) {
            bool dead = get_animator ( ).GetBool ( parameter_hash[AnimatorParameter.Dead] );
            if ( dead ) {
                get_animator ( ).SetBool ( parameter_hash[AnimatorParameter.Dead], false );
            }
        }


        protected override void active_hit ( float amount, Unit source = null ) {
            get_animator ( ).SetTrigger ( parameter_hash[AnimatorParameter.Hit] );

            if(source != null) {
                float angle = (Angle.target_to_angle ( source.get_position ( ), get_position ( ) ) * Mathf.Rad2Deg) - 90f;
                Vector2 force = Polar.location ( 5f, angle );
                movement_system.add_force ( force );
            }

            // HACK
            if( bottle != null) {
                bottle.switch_on = true;
                unit_status.is_dead = true;
            }
        }


        protected override void active_update ( ) {
            
        }


        protected override void order ( ) {
            if ( unit_order.get_order ( Order_Id.Attack ) ) {
                attack ( );
            }


            if ( unit_order.get_order ( Order_Id.Weak ) ) {
                weak ( );
            }
        }


        public override void confirm ( ) {
            base.confirm ( );

            my_type = unit_type as PaintManType;

            my_type?.add ( PaintManType.Action_Attack, action_attack );
            my_type?.add ( PaintManType.Action_Attack_Stop, action_attack_stop );

            my_type?.add ( PaintManType.Begin_Hit, begin_hit );
            my_type?.add ( PaintManType.End_Hit, end_hit );


        }


        // Animation Action

        private void action_attack ( ) {
            StartCoroutine ( Eaction_attack ( ) );
        }
        private void action_attack_stop ( ) {
            my_type.do_attack = false;
        }


        private void begin_hit ( ) {
            if ( !do_weaked ) {
                my_type.hit_count++;
            }
        }
        private void end_hit ( ) {
            if( my_type.hit_count >= weak_count ) {
                weak ( );
            }
        }


        // IEnumerator

        private IEnumerator Eaction_attack ( ) {
            if ( my_type.do_attack ) {
                yield break;
            }
            my_type.do_attack = true;
            my_type.attacked_units.Clear ( );

            float damage = unit_status.damage + unit_status.add_damage + unit_status.rate_damage;
            float range = 1f;
            LayerMask layer = 1 << (int)GameLayer.Unit;

            while ( my_type.do_attack ) {
                if ( !get_animator_nextstate ( 0 ).IsTag ( state_tag[AnimatorTag.Attack]) &&
                    !get_animator_state ( 0 ).IsTag ( state_tag[AnimatorTag.Attack] ) ) {
                    action_attack_stop ( );
                    continue;
                }
                on_damage ( damage, range, layer );
                yield return new WaitForEndOfFrame();
            }
        }


        private IEnumerator Eweaked_condition ( float duration ) {
            set_order ( Order_Id.Weak, false );
            if ( do_weaked ) {
                yield break;
            }
            do_weaked = true;
            get_animator ( ).SetBool ( parameter_hash[AnimatorParameter.Weak], true );
            get_animator ( ).SetTrigger ( parameter_hash[AnimatorParameter.TWeak] );

            float t = 0f;
            bool loop = true;
            while ( loop ) {
                if ( !do_weaked ||
                    t >= duration ) {
                    loop = false;
                    continue;
                }
                t += Time.deltaTime;
                yield return new WaitForEndOfFrame ( );
            }

            get_animator ( ).SetBool ( parameter_hash[AnimatorParameter.Weak], false );
            do_weaked = false;
        }


        private IEnumerator Eattack_cooltime ( ) {
            if( do_attack ) {
                yield break;
            }
            do_attack = true;

            float time = 0f;
            float speed = unit_status.aspeed > 0f ? unit_status.aspeed : 0.001f;
            float aspeed = speed + (unit_status.add_aspeed + unit_status.rate_aspeed);
            float result = aspeed > 0f ? aspeed : 0.001f;
            float cooltime = unit_status.attack_cooltime / result;
            while(do_attack) {
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

        private void Awake ( ) {
            confirm ( );
        }


        private void Update ( ) {
            order ( );
        }


        private void FixedUpdate ( ) {
            active ( );
        }


        private void OnTriggerEnter2D ( Collider2D collision ) {
            if(this.bottle != null) {
                return;
            }

            Obj.WaterBottle bottle = collision.GetComponent<Obj.WaterBottle> ( );
            if(bottle == null) {
                return;
            }
            this.bottle = bottle;
        }


        private void OnTriggerExit2D ( Collider2D collision ) {
            if ( this.bottle == null) {
                return;
            }

            Obj.WaterBottle bottle = collision.GetComponent<Obj.WaterBottle> ( );
            if ( bottle == null || this.bottle != bottle) {
                return;
            }
            this.bottle = null;
        }
    }
}
