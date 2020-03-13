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
        [SerializeField]
        private Transform missile_transform = null;

        [Header ( "테스트용(HACK)" )]
        public Unit target;


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
            float angle = Angle.target_to_angle ( get_position ( ), target.get_position ( ) ) * Mathf.Rad2Deg - 90f;
            create_missile ( angle );
        }


        public void attack_randommissile ( ) {
            StartCoroutine ( Erandom_attack ( ) );
        }


        public void attack_tripplemissile ( ) {
            StartCoroutine ( Etripple_attack ( ) );
        }


        public void attack_cornermissile ( ) {
            float angle = 45;
            for(int i = 0; i < 4; ++i ) {
                for(int j = 0; j < 2; ++j ) {
                    if(j % 2 == 0) {
                        create_missile ( angle - 5f );
                    } else {
                        create_missile ( angle + 5f );
                    }
                }
                angle += 90;
            }
        }


        public void attack_scattermissile ( ) {
            StartCoroutine ( Escatter_attack ( ) );
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

            if ( Input.GetKeyDown ( KeyCode.Alpha4 ) ) {
                attack_randommissile ( );
            }

            if ( Input.GetKeyDown ( KeyCode.Alpha5 ) ) {
                attack_tripplemissile ( );
            }

            if ( Input.GetKeyDown ( KeyCode.Alpha6 ) ) {
                attack_cornermissile ( );
            }

            if ( Input.GetKeyDown ( KeyCode.Alpha7 ) ) {
                attack_scattermissile ( );
            }
        }


        public override void confirm ( ) {
            base.confirm ( );

            my_type = unit_type as PianoManType;
        }



        public GameObject create_missile ( float angle ) {
            GameObject missile_prefab = Management.ResourceLoader.instance.get_prefab ( Management.ResourceLoader.Resource.Melody_Missile );
            GameObject missile = Instantiate ( missile_prefab, missile_transform.position, Quaternion.identity );
            UMelodyMissile unit = missile.GetComponent<UMelodyMissile> ( );
            unit.rotate ( angle );
            return missile;
        }


        private IEnumerator Erotation_attack ( ) {
            int tick = 50;
            int count = 0;
            float angle = Angle.target_to_angle ( get_position ( ), target.get_position ( ) ) * Mathf.Rad2Deg - 90f;
            float delta = 90f / tick;

            float time = 0.1f;
            bool loop = true;
            while(loop) {
                time += Time.deltaTime;
                if(time >= 0.1f) {
                    for ( int i = 0; i < 4; i++ ) {
                        create_missile ( angle - (i * 90f) );
                    }

                    angle -= delta;
                    count++;
                    time = 0f;
                }
                if(count >= tick) {
                    loop = false; continue;
                }
                yield return new WaitForEndOfFrame ( );
            }
        }

        private IEnumerator Erandom_attack ( ) {
            int tick = Random.Range ( 5, 10 );
            int count = 0;
            

            float time = 0.33f;
            bool loop = true;
            while(loop) {
                time += Time.deltaTime;
                if ( time >= 0.33f ) {
                    create_missile ( Random.Range ( 0f, 360f ) );
                    time = 0f;
                    if ( ++count >= tick ) {
                        loop = false; continue;
                    }
                }
                yield return new WaitForEndOfFrame ( );
            }
        }


        private IEnumerator Etripple_attack ( ) {
            int tick = 3;
            int count = 0;
            float angle = Angle.target_to_angle ( get_position ( ), target.get_position ( ) ) * Mathf.Rad2Deg - 90f;
            float delta = 15f;

            // 빨간색
            create_missile ( angle );

            float time = -0.33f;
            bool loop = true;
            while ( loop ) {
                time += Time.deltaTime;
                if ( time >= 0.33f ) {
                    create_missile ( angle - delta );
                    delta -= 15f;
                    time = 0f;
                    if ( ++count >= tick ) {
                        loop = false; continue;
                    }
                }
                yield return new WaitForEndOfFrame ( );
            }
        }


        private IEnumerator Escatter_attack ( ) {
            int tick = 50;
            int count = 0;
            float angle = Angle.target_to_angle ( get_position ( ), target.get_position ( ) ) * Mathf.Rad2Deg - 85f;
            float delta = 270f / tick;

            create_missile ( angle );
            angle -= delta;

            //float time = 0f;
            bool loop = true;
            while ( loop ) {
                create_missile ( angle );
                angle -= delta;
                if ( ++count >= tick ) {
                    loop = false; continue;
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
