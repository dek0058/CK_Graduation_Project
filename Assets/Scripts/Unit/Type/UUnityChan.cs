using UnityEngine;

namespace Game.Unit.Type {
    using JToolkit.Utility;
    using Game.Unit;

    public class UUnityChan : Unit {

        /// <summary>
        /// UnityChan이 가진 Animator Parameter값 표시용 Enum
        /// </summary>
        public enum Animator_Parameter {
            
        }

        private EnumDictionary<Animator_Parameter, int> animator_hash = new EnumDictionary<Animator_Parameter, int> {

        };


        private UnityChanType my_type;


        public override void rotate ( float angle ) {
            unit_model.transform.Rotate ( 0f, angle * unit_status.rotation_speed, 0f );
        }


        public override void move ( Vector3 direction ) {
            movement_system.move ( direction * unit_status.movement_speed );
        }



        public override void confirm ( ) {
            base.confirm ( );

            if(unit_type == null) {
                unit_type = new UnityChanType ( );
            }

            nickname = "Unity-Chan";
        }


        


        ////////////////////////////////////////////////////////////////////////////
        ///                               Unity                                  ///
        ////////////////////////////////////////////////////////////////////////////


        private void Awake ( ) {
            confirm ( );
        }
    }


    [System.Serializable]
    public class UnityChanType : UnitType {

    }
}
