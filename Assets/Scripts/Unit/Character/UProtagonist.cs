using UnityEngine;

namespace Game.Unit.Character {
    using JToolkit.Utility;
    using JToolkit.Math;
    using Game.Unit;
    using Game.Unit.Type;

    public class UProtagonist : Unit {

        /// <summary>
        /// Protagonist가 가진 Animator Paramter
        /// </summary>
        public enum AnimatorParameter {
            Run,
            Attack,
        }

        private EnumDictionary<AnimatorParameter, int> parameter_hash = new EnumDictionary<AnimatorParameter, int> {
            { AnimatorParameter.Run, Animator.StringToHash("Run") },
            { AnimatorParameter.Attack, Animator.StringToHash("Attack") },
        };


        private ProtagonistType my_type;



        public void attack ( ) {
            if(get_animator_state(0).IsTag(state_tag[AnimatorTag.Attack])) {        // 이미 공격에 성공했으므로
                get_animator ( ).ResetTrigger ( parameter_hash[AnimatorParameter.Attack] );
                unit_order.set_order ( Order_Id.Attack, false );
                return;
            }
            get_animator ( ).SetTrigger ( parameter_hash[AnimatorParameter.Attack] );
        }


        protected override void active_rotate ( ) {
            float y = get_rotation ( ).y;
            float gap = Mathf.DeltaAngle ( y, unit_status.look_at );
            unit_model.transform.Rotate ( 0f, gap * unit_status.rspeed * Time.fixedDeltaTime, 0f );
            unit_status.angle = get_rotation ( ).y;
        }


        protected override void active_move ( ) {
            if(get_animator_state(0).IsTag(state_tag[AnimatorTag.Movement])) { 
                base.active_move ( );
            }

            get_animator ( ).SetBool ( parameter_hash[AnimatorParameter.Run], unit_order.get_order ( Order_Id.Move ) );
        }


        /// <summary>
        /// Unit 명령 실행
        /// </summary>
        protected override void order ( ) {
            if ( unit_order.get_order ( Order_Id.Attack ) ) {
                attack ( );
            }
        }


        public override void confirm ( ) {
            base.confirm ( );

            my_type = unit_type as ProtagonistType;
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
