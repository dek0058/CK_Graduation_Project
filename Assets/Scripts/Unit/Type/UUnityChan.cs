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
            Walk,
        }

        private EnumDictionary<Animator_Parameter, int> animator_hash = new EnumDictionary<Animator_Parameter, int> {
            { Animator_Parameter.Dir_X, Animator.StringToHash("Dir_X") },
            { Animator_Parameter.Dir_Y, Animator.StringToHash("Dir_Y") },
            { Animator_Parameter.Idle_Type, Animator.StringToHash("Idle_Type") },
            { Animator_Parameter.Active, Animator.StringToHash("Active") },
            { Animator_Parameter.Run, Animator.StringToHash("Run") },
            { Animator_Parameter.Walk, Animator.StringToHash("Walk") },
        };


        private UnityChanType my_type;


        public override void confirm ( ) {
            base.confirm ( );

            if(unit_type == null) {
                unit_type = new UnityChanType ( );
            }
        }


        protected override void active_move ( ) {
            if(unit_status.direction == Vector2.zero && unit_status.next_direction == Vector2.zero ) {
                return;
            }
            unit_status.direction = Vector2.MoveTowards ( unit_status.direction, unit_status.next_direction, Time.fixedDeltaTime );
            //movement_system.move ( unit_status.direction * unit_status.mspeed );

            Vector2 temp = Polar.location ( 1f, unit_status.angle );
            Vector2 location = new Vector2 ( temp.y, temp.x );
            Vector2 gap = unit_status.direction - location;

            Debug.Log ( gap );
            //unit_status.direction 와 location을 비교해야함

            unit_model.animator.SetFloat ( animator_hash[Animator_Parameter.Dir_X], 0f );
            unit_model.animator.SetFloat ( animator_hash[Animator_Parameter.Dir_Y], 0f );
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
