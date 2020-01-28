using UnityEngine;

namespace Game.Unit.Type {
    using JToolkit.Utility;
    using JToolkit.Math;
    using Game.Unit;

    public class UUnityChan : Unit {

        /// <summary>
        /// UnityChan이 가진 Animator Parameter값 표시용 Enum
        /// </summary>
        public enum Animator_Parameter {
            Dir_X,
            Dir_Y,
            Idle_Type,
            Active,
            Run,
        }

        private EnumDictionary<Animator_Parameter, int> parameter_hash = new EnumDictionary<Animator_Parameter, int> {
            { Animator_Parameter.Dir_X, Animator.StringToHash("Dir_X") },
            { Animator_Parameter.Dir_Y, Animator.StringToHash("Dir_Y") },
            { Animator_Parameter.Idle_Type, Animator.StringToHash("Idle_Type") },
            { Animator_Parameter.Active, Animator.StringToHash("Active") },
            { Animator_Parameter.Run, Animator.StringToHash("Run") },
        };


        /// <summary>
        /// UnityChan이 가진 Animator State Machine 표시용 Enum
        /// </summary>
        public enum Animator_State {
            Idle,
            Stand_Clam_To_Stand,
            Stand_Loop,
            Run,
            Run_To_Stand,
        }

        private EnumDictionary<Animator_State, int> state_hash = new EnumDictionary<Animator_State, int> {
            {Animator_State.Idle, Animator.StringToHash("Base Layer.stand.idle") },
            {Animator_State.Stand_Clam_To_Stand, Animator.StringToHash("Base Layer.stand.Stand_clamToStand") },
            {Animator_State.Stand_Loop, Animator.StringToHash("Base Layer.stand.Stand@loop") },
            {Animator_State.Run, Animator.StringToHash("Base Layer.run.run") },
            {Animator_State.Run_To_Stand, Animator.StringToHash("Base Layer.run.runToStand") },
        };


        private UnityChanType my_type;



        public void ready ( ) {
            get_animator ( ).SetFloat ( parameter_hash[Animator_Parameter.Active], 10f );
        }



        public override void confirm ( ) {
            base.confirm ( );

            if(unit_type == null) {
                unit_type = new UnityChanType ( );
            }
        }


        protected override void active_move ( ) {
            float sspeed = 5 + (1 * unit_status.mspeed);
            Vector2 axis = Vector2.zero;
            if ( unit_status.input.magnitude > 0f ) {
                Vector2 polar = Polar.location ( 1f, unit_status.angle );
                Vector2 location = new Vector2(polar.y, polar.x);
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
                } else {
                    gap = location + direction;
                    if ( Mathf.Abs ( delta ) > 125f ) {
                        axis.x = max == Mathf.Abs ( gap.x ) ? gap.x : gap.y;
                        axis.y = 1f - max;
                    } else {
                        axis.x = 1f - max;
                        axis.y = max == Mathf.Abs ( gap.y ) ? gap.y : gap.y;
                    }
                }

                axis.x = delta > 0f ? axis.x : -axis.x;
                axis.y = Mathf.Abs ( delta ) < 90f ? axis.y  : - axis.y;
            }

            unit_status.direction = Vector2.MoveTowards ( unit_status.direction, unit_status.input, Time.fixedDeltaTime * sspeed );
            unit_status.axis.y = Mathf.MoveTowards ( unit_status.axis.y, axis.y, Time.fixedDeltaTime * sspeed );
            unit_status.axis.x = Mathf.MoveTowards ( unit_status.axis.x, axis.x, Time.fixedDeltaTime * sspeed );

            get_animator ( ).SetFloat ( parameter_hash[Animator_Parameter.Dir_X], unit_status.axis.x );
            get_animator().SetFloat ( parameter_hash[Animator_Parameter.Dir_Y], unit_status.axis.y );

            
            if ( equals_animator_hash ( 0, state_hash[Animator_State.Run] ) ) {     // Unit이 달리는 애니메이션 중일 때만 이동을 시킴
                movement_system.move ( (unit_status.direction / unit_status.direction.magnitude) * unit_status.mspeed * Time.fixedDeltaTime );
            }
            
            // Unit이 이동중인지 확인
            if ( Mathf.Approximately ( unit_status.axis.x, 0f ) && Mathf.Approximately ( unit_status.axis.y, 0f ) &&
                Mathf.Approximately ( unit_status.input.x, 0f ) && Mathf.Approximately ( unit_status.input.y, 0f ) ) {
                get_animator ( ).SetBool ( parameter_hash[Animator_Parameter.Run], false );
            } else {
                get_animator ( ).SetBool ( parameter_hash[Animator_Parameter.Run], true );
            }
        }


        protected override void active_update ( ) {
            if ( !equals_animator_hash ( 0, state_hash[Animator_State.Stand_Clam_To_Stand] ) &&
                !equals_animator_hash ( 0, state_hash[Animator_State.Idle] ) &&
                !equals_animator_hash ( 0, state_hash[Animator_State.Stand_Loop] ) ) {
                ready ( );
            }

            float factive = get_animator ( ).GetFloat ( parameter_hash[Animator_Parameter.Active] );
            if(factive > 0f) {
                factive = Mathf.Clamp ( factive - Time.fixedDeltaTime, 0f, float.MaxValue );
                unit_model.animator.SetFloat ( parameter_hash[Animator_Parameter.Active], factive );
            }

        }


        /// <summary>
        /// 현재 Animator State Machine을 가져옵니다.
        /// </summary>
        /// <returns>Current Animator State Machine</returns>
        public AnimatorStateInfo get_animator_state ( int layer ) {
            return get_animator ( ).GetCurrentAnimatorStateInfo ( layer );
        }


        /// <summary>
        /// 현재 Animator State Machine의 해시값과 일치하는지 검증합니다.
        /// </summary>
        public bool equals_animator_hash ( int layer, int hash ) {
            return get_animator_state ( layer ).fullPathHash == hash;
        }


        ////////////////////////////////////////////////////////////////////////////
        ///                               Unity                                  ///
        ////////////////////////////////////////////////////////////////////////////


        private void Awake ( ) {
            confirm ( );

            
        }



        private void FixedUpdate ( ) {

            active ( );

        }
    }


    [System.Serializable]
    public class UnityChanType : UnitType {

    }
}
