using UnityEngine;
using System.Collections;

namespace Game.Item.Equipment {
    using JToolkit.Utility;
    using JToolkit.Math;

    public class WCypressItem : Item, IEquipmentItem {

        public EquipmentType type {
            get; set;
        }

        public EquipmentStatus status {
            get; set;
        }


        private enum AnimatorParamter {
            Aspeed,
            Angle,
            Type,
            Attack,
            Cancel,
        }
        private EnumDictionary<AnimatorParamter, int> parameter_hash = new EnumDictionary<AnimatorParamter, int> {
            {AnimatorParamter.Aspeed, Animator.StringToHash("Aspeed") },
            {AnimatorParamter.Angle, Animator.StringToHash("Angle") },
            {AnimatorParamter.Type, Animator.StringToHash("Type") },
            {AnimatorParamter.Attack, Animator.StringToHash("Attack") },
            {AnimatorParamter.Cancel, Animator.StringToHash("Cancel") },
        };


        public Animator animator;

        [HideInInspector]
        public bool smb_attack;
        


        public void action ( EquipmentAction action ) {
            if(action == EquipmentAction.Attack) {
                StartCoroutine ( Eattack ( ) );
            }
        }


        public void cancel ( ) {
            animator.SetTrigger ( parameter_hash[AnimatorParamter.Cancel] );
        }
        


        public override void confirm ( ) {
            base.confirm ( );
            
            if(animator == null) {
                animator = GetComponent<Animator> ( );
            }

            type = EquipmentType.Weapon;
        }


        private void set_animation_speed ( float aspeed ) {
            animator.SetFloat ( parameter_hash[AnimatorParamter.Aspeed], aspeed );
        }

        private void set_animation_angle ( float angle ) {
            float trim = Angle.trim ( angle ) / 360f;
            animator.SetFloat ( parameter_hash[AnimatorParamter.Angle], trim );
        }


        private IEnumerator Eattack ( ) {
            if(smb_attack) {
                yield break;
            }
            smb_attack = true;
            animator.SetTrigger ( parameter_hash[AnimatorParamter.Attack] );

            while (smb_attack) {
                set_animation_speed ( owner.unit_status.rhythm );
                set_animation_angle ( owner.unit_status.angle );

                yield return null;
            }
            animator.SetFloat ( parameter_hash[AnimatorParamter.Aspeed], 1f );
        }

        ////////////////////////////////////////////////////////////////////////////
        ///                               Unity                                  ///
        ////////////////////////////////////////////////////////////////////////////

        private void Start ( ) {
            Animation.SceneLinkedSMB<WCypressItem>.Initialize ( animator, this );
        }
    }
}
