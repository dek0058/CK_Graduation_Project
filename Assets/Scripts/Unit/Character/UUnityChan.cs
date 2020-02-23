﻿using UnityEngine;

namespace Game.Unit.Character {
    using JToolkit.Testing;
    using JToolkit.Utility;
    using JToolkit.Math;
    using Game.Unit;
    using Game.Unit.Type;
    
    public class UUnityChan : Unit {

        /// <summary>
        /// UnityChan이 가진 Animator Parameter값 표시용 Enum
        /// </summary>
        public enum AnimatorParameter {
            Dir_X,
            Dir_Y,
            Idle_Type,
            Active,
            Run,
            Back,
            Attack
        }

        private EnumDictionary<AnimatorParameter, int> parameter_hash = new EnumDictionary<AnimatorParameter, int> {
            { AnimatorParameter.Dir_X, Animator.StringToHash("Dir_X") },
            { AnimatorParameter.Dir_Y, Animator.StringToHash("Dir_Y") },
            { AnimatorParameter.Idle_Type, Animator.StringToHash("Idle_Type") },
            { AnimatorParameter.Active, Animator.StringToHash("Active") },
            { AnimatorParameter.Run, Animator.StringToHash("Run") },
            { AnimatorParameter.Back, Animator.StringToHash("Back") },
            { AnimatorParameter.Attack, Animator.StringToHash("Attack") },
        };


        /// <summary>
        /// UnityChan이 가진 Animator State Machine 표시용 Enum
        /// </summary>
        public enum AnimatorState {
            Idle,
            Stand_Clam_To_Stand,
            Stand_Loop,
            Run,
            Run_To_Stand,
            Back,
            AttackA2,
        }

        private EnumDictionary<AnimatorState, int> state_hash = new EnumDictionary<AnimatorState, int> {
            {AnimatorState.Idle, Animator.StringToHash("Base Layer.stand.idle") },
            {AnimatorState.Stand_Clam_To_Stand, Animator.StringToHash("Base Layer.stand.Stand_clamToStand") },
            {AnimatorState.Stand_Loop, Animator.StringToHash("Base Layer.stand.Stand@loop") },
            {AnimatorState.Run, Animator.StringToHash("Base Layer.run.run") },
            {AnimatorState.Run_To_Stand, Animator.StringToHash("Base Layer.run.runToStand") },
            {AnimatorState.Back, Animator.StringToHash("Base Layer.run.back") },
            {AnimatorState.Back, Animator.StringToHash("Base Layer.attackA2") },
        };


        private UnityChanType my_type = null;




        


        public void ready ( ) {
            get_animator ( ).SetFloat ( parameter_hash[AnimatorParameter.Active], 10f );
        }


        public void attack ( ) {
            if(get_animator_state(0).IsTag(state_tag[AnimatorTag.Attack])) {               // 이미 공격시전에 성공을 했으므로
                get_animator ( ).ResetTrigger ( parameter_hash[AnimatorParameter.Attack] );
                unit_order.set_order ( Order_Id.Attack, false );
                return;
            }

            get_animator ( ).SetTrigger ( parameter_hash[AnimatorParameter.Attack] );
        }

        private void attack1_damage_action ( ) {
            float dist = 1.25f;     // 범위 (최대 사정거리)
            float angle = 90f;      // 반경
            LayerMask layer = -1;   // Debug - 우선 전부

            // * TODO : 이부분은 공격 하는 동안에는 계속해서 갱신되어야 함
            Collider[] colliders = Physics.OverlapSphere ( get_position ( ), dist, layer );

            foreach(var col in colliders) {
                Transform target = col.transform;
                float tangle = Angle.target_to_angle ( get_position ( ), target.position ) * Mathf.Rad2Deg;
                float delta = Mathf.Abs ( Mathf.DeltaAngle ( unit_status.angle, tangle ) );
                if(delta > angle) {     //  타겟이 반경을 넘어갔을 경우
                    continue;
                }
                // TODO : 데미지 갱신
                // TODO : 한번만 피해를 입힐 수 있도록 배열에 담아줘야함
            }
            // *


            // Debug
            string path = "Material/Indicator_Mat";
            Material mat = Resources.Load<Material> ( path );
            GameObject indicator = DrawFieldOfView.create ( get_rotation ( ).y, angle, 0.33f, dist, get_position ( ), Quaternion.Euler ( -90f, 0f, 0f ), mat, LayerMask.NameToLayer ( "VFX" ) );
            Destroy ( indicator, 1.5f );
        }

        private void attack_damage_stop ( ) {
            
        }


        /// <summary>
        /// Unit 명령 실행
        /// </summary>
        protected override void order ( ) {
            
            if(unit_order.get_order(Order_Id.Attack)) {
                attack ( );
            }

        }


        public override void confirm ( ) {
            base.confirm ( );

            my_type = unit_type as UnityChanType;

            my_type?.add ( UnityChanType.Attack1, attack1_damage_action );      // 공격1 피해시작
            my_type?.add ( UnityChanType.Attack_Stop, attack_damage_stop );     // 공격 피해중단
        }


        protected override void active_move ( ) {
            float sspeed = 5 + (1 * unit_status.mspeed);
            float mspeed = unit_status.mspeed;
            Vector2 axis = Vector2.zero;
            if ( unit_status.input.magnitude > 0f ) {
                Vector2 polar = Polar.location ( 1f, unit_status.angle );
                Vector2 location = new Vector2(polar.x, polar.y);
                Vector2 direction = unit_status.input;
                Vector2 gap = Vector2.zero;

                location /= Mathf.Abs ( location.x ) + Mathf.Abs ( location.y );
                direction /= Mathf.Abs ( direction.x ) + Mathf.Abs ( direction.y );
                //Debug.Log ( "방향 : " + direction + ", 보는각도 : " + location + ", 갭 : " + gap );

                float dir_angle = Angle.target_to_angle ( Vector2.zero, unit_status.input ) * Mathf.Rad2Deg;
                float delta = Mathf.DeltaAngle ( unit_status.angle, dir_angle );
                float max = Mathf.Max ( Mathf.Abs ( gap.x ), Mathf.Abs ( gap.y ) );

                if ( Mathf.Abs ( delta ) < 90f ) {
                    gap = location - direction;
                    if ( Mathf.Abs ( delta ) < 45f ) {
                        axis.x = max == Mathf.Abs ( gap.x ) ? gap.x : gap.y;
                        axis.y = 1f - max;
                    } else {
                        axis.x = 1f - max;
                        axis.y = max == Mathf.Abs ( gap.y ) ? gap.y : gap.y;
                    }
                    get_animator ( ).SetBool ( parameter_hash[AnimatorParameter.Back], false );

                } else {
                    gap = location + direction;
                    if ( Mathf.Abs ( delta ) > 125f ) {
                        axis.x = max == Mathf.Abs ( gap.x ) ? gap.x : gap.y;
                        axis.y = 1f - max;
                    } else {
                        axis.x = 1f - max;
                        axis.y = max == Mathf.Abs ( gap.y ) ? gap.y : gap.y;
                    }

                    get_animator ( ).SetBool ( parameter_hash[AnimatorParameter.Back], axis.y < 0f ? false : true );
                }

                axis.x = delta > 0f ? axis.x : -axis.x;
                axis.y = Mathf.Abs ( delta ) < 90f ? axis.y  : - axis.y;
            }

            unit_status.direction = Vector2.MoveTowards ( unit_status.direction, unit_status.input, Time.fixedDeltaTime * sspeed );
            unit_status.axis.y = Mathf.MoveTowards ( unit_status.axis.y, axis.y, Time.fixedDeltaTime * sspeed );
            unit_status.axis.x = Mathf.MoveTowards ( unit_status.axis.x, axis.x, Time.fixedDeltaTime * sspeed );

            get_animator ( ).SetFloat ( parameter_hash[AnimatorParameter.Dir_X], unit_status.axis.x );
            get_animator ( ).SetFloat ( parameter_hash[AnimatorParameter.Dir_Y], unit_status.axis.y );

            /* Current Animator State의 Tag가 Movement(이동)일 경우 Unit을 이동시킴
                만약 뒤로 걷고 있다면 이동속도가 50% 감소 */
            bool back = get_animator ( ).GetBool ( parameter_hash[AnimatorParameter.Back] );
            mspeed = back ? mspeed / 2f : mspeed;
            

            if ( get_animator_state(0).IsTag( state_tag[AnimatorTag.Movement] ) ) {
                movement_system.move ( (unit_status.direction / unit_status.direction.magnitude) * mspeed );
            }
            
            // Unit이 이동중인지 확인
            if ( Mathf.Approximately ( unit_status.axis.x, 0f ) && Mathf.Approximately ( unit_status.axis.y, 0f ) &&
                Mathf.Approximately ( unit_status.input.x, 0f ) && Mathf.Approximately ( unit_status.input.y, 0f ) ) {
                get_animator ( ).SetBool ( parameter_hash[AnimatorParameter.Run], false );
            } else {
                get_animator ( ).SetBool ( parameter_hash[AnimatorParameter.Run], true );
            }
        }


        protected override void active_update ( ) {
            //* Idle Animation
            if ( !get_animator_state ( 0 ).IsTag ( state_tag[AnimatorTag.Idle] ) ) {
                ready ( );
            }

            float factive = get_animator ( ).GetFloat ( parameter_hash[AnimatorParameter.Active] );
            if ( factive > 0f ) {
                factive = Mathf.Clamp ( factive - Time.fixedDeltaTime, 0f, float.MaxValue );
                get_animator ( ).SetFloat ( parameter_hash[AnimatorParameter.Active], factive );
            }
            //*

            //* Attack Animation
            unit_order.set_active ( UnitOrder.Active.Rotate, get_animator_state ( 0 ).IsTag ( state_tag[AnimatorTag.Attack] ) );
            if ( get_animator_state ( 0 ).IsTag ( state_tag[AnimatorTag.Attack] ) ) {
                Debug.Log ( "A" );
                unit_status.input = Vector2.zero;
            }
            //*
        }


        ////////////////////////////////////////////////////////////////////////////
        ///                               Unity                                  ///
        ////////////////////////////////////////////////////////////////////////////


        private void Awake ( ) {
            confirm ( );        
        }



        private void FixedUpdate ( ) {
            order ( );
            active ( );
        }
    }
}