using UnityEngine;
using System.Collections;

namespace Game.Unit.Character {
    using JToolkit.Utility;
    using JToolkit.Math;
    using Game.Unit;
    using Game.Unit.Type;
    using Game.Unit.Missile;

    public class UPianoMan : Unit {


        /// <summary>
        /// PianoMan이 가진 Animator Paramter
        /// </summary>
        public enum AnimatorParameter {
            Channeling,
        }

        private EnumDictionary<AnimatorParameter, int> parameter_hash = new EnumDictionary<AnimatorParameter, int> {
            { AnimatorParameter.Channeling, Animator.StringToHash("Channeling") },
        };

        private PianoManType my_type;



        /// <summary>
        /// 피아노맨은 가만히 서 있지만, 시점 또한 앞에만 바라보면서 연주만 한다.
        /// </summary>
        public void attention ( ) {
            unit_status.look_at = 180f;
        }


        /// <summary>
        /// 타겟이 있는 방향부터 시계방향으로 차례대로 음표를 발사한다.
        /// </summary>
        public void attack ( ) {
            StartCoroutine ( Erotation_attack ( ) );
        }


        /// <summary>
        /// 타겟을 향해서 음표가 발사되며, 음표는 무조건
        /// 타겟이 있는 위치로 일직선으로 날아온다.
        /// </summary>
        public void attack_guidedmissile ( ) {
            float angle = 0f;

            GameObject missile_prefab = Resources.Load<GameObject> ( "Unit/Missile/MelodyMissile/Prefab/MelodyMissile" );

            GameObject missile = Instantiate ( missile_prefab, transform.position, Quaternion.identity );
            UMelodyMissile unit = missile.GetComponent<UMelodyMissile> ( );
            missile.transform.eulerAngles = new Vector3 ( 0f, 0f, angle );
            unit.rotate ( angle );
        }


        protected override void active_rotate ( ) {
            float y = get_rotation ( ).y;
            float gap = Mathf.DeltaAngle ( y, unit_status.look_at );
            unit_model.transform.Rotate ( 0f, gap * unit_status.rspeed * Time.fixedDeltaTime, 0f );
            unit_status.angle = get_rotation ( ).y;
        }


        protected override void order ( ) {
            if(Input.GetKeyDown(KeyCode.Alpha1)) {
                attention ( );
            }

            if ( Input.GetKeyDown ( KeyCode.Alpha2 ) ) {
                attack ( );
            }

            if ( Input.GetKeyDown ( KeyCode.Alpha3 ) ) {
                attack_guidedmissile ( );
            }
        }


        public override void confirm ( ) {
            base.confirm ( );

            my_type = unit_type as PianoManType;
        }




        private IEnumerator Erotation_attack ( ) {
            int tick = 36;
            float delta = 360f / tick;
            float angle = 360f;
            float start = unit_model.transform.eulerAngles.y;


            // HACK
            GameObject missile_prefab = Resources.Load<GameObject> ( "Unit/Missile/MelodyMissile/Prefab/MelodyMissile" );

            bool loop = true;
            while(loop) {
                GameObject missile = Instantiate ( missile_prefab, transform.position, Quaternion.identity);
                UMelodyMissile unit = missile.GetComponent<UMelodyMissile> ( );
                missile.transform.eulerAngles = new Vector3 ( 0f, 0f, angle );
                unit.rotate ( angle );

                angle -= delta;
                if(angle <= 0f ) {
                    loop = false;
                }
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
    }
}
