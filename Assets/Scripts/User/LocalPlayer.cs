using UnityEngine;
using System.Collections;

namespace Game.User {
    using Unit;
    using Management;
    using JToolkit.Utility;
    using JToolkit.Math;

    public class LocalPlayer : Player {

        [Header ( "Control Unit" )]
        public UUnit unit = null;
        public UnitOrder player_order = null;

        private Vector2 axis = new Vector2 ( );
        private float releas_both_input_time = 0f;
        [SerializeField, Range ( 1f, 10f )]
        private float releas_input_time_speed = 5f;
        private const float Releas_Both_Input_Time = 0.56f;

        private bool do_purgatory_wait = false;
        private bool do_purgatory = false;
        public bool is_purgatory {
            get => do_purgatory;
        }


        public void active_purgatory_area ( ) {
            if ( unit == null || unit.unit_status.is_dead ) {
                return;
            }
            StartCoroutine ( Eactive_purgatory ( ) );
        }


        public void inactive_purgatory_area ( ) {
            do_purgatory = false;
        }



        /// <summary>
        /// Local Player일 경우 유저는 컨트롤러를 조종할 수 있습니다.
        /// </summary>
        public void controller ( ) {
            if ( !is_local || unit == null ) {
                return;
            }

            // 이동
            float x = Singleton<PlayerInput>.instance.horizontal.value;
            float y = Singleton<PlayerInput>.instance.vertical.value;
            bool is_both_receiving = Singleton<PlayerInput>.instance.horizontal.receiving_input && Singleton<PlayerInput>.instance.vertical.receiving_input;
            bool is_receiving = Singleton<PlayerInput>.instance.horizontal.receiving_input || Singleton<PlayerInput>.instance.vertical.receiving_input;


            float time = Time.deltaTime * releas_input_time_speed;
            if ( is_both_receiving ) {
                releas_both_input_time = Releas_Both_Input_Time;
                axis.x = x;
                axis.y = y;

            } else if ( is_receiving ) {
                if ( releas_both_input_time > 0f ) {

                    releas_both_input_time = releas_both_input_time - time > 0f ? releas_both_input_time - time : 0f;
                    if ( x == 0f ) {
                        axis.x = axis.x != 0 ? Mathf.MoveTowards ( axis.x, 0f, time ) : 0f;
                    } else {
                        axis.x = x;
                    }
                    if ( y == 0f ) {
                        axis.y = axis.y != 0 ? Mathf.MoveTowards ( axis.y, 0f, time ) : 0f;
                    } else {
                        axis.y = y;
                    }
                } else {
                    axis.x = x;
                    axis.y = y;
                }

            } else if ( !is_receiving ) {
                if ( releas_both_input_time > 0f ) {
                    axis.x = axis.x != 0 ? Mathf.MoveTowards ( axis.x, 0f, time ) : 0f;
                    axis.y = axis.y != 0 ? Mathf.MoveTowards ( axis.y, 0f, time ) : 0f;
                    if ( axis.x == 0 && axis.y == 0 ) {
                        releas_both_input_time = 0f;
                    }
                } else {
                    axis.x = x;
                    axis.y = y;
                }
            }

            // 이동 조정
            Vector2 temp_axis = axis;
            if ( temp_axis.x > 0f ) {
                temp_axis.x = 1f;
            } else if ( temp_axis.x < 0f ) {
                temp_axis.x = -1f;
            }
            if ( temp_axis.y > 0f ) {
                temp_axis.y = 1f;
            } else if ( temp_axis.y < 0f ) {
                temp_axis.y = -1f;
            }


            // 이동 키를 누르고 있으므로 유닛에게 이동 명령을 알림
            unit.set_order ( Order_Id.Move, is_receiving );
            player_order.set_order ( Order_Id.Move, is_receiving );
            unit.move ( temp_axis );

            // 키보드 회전
            if ( !(Mathf.Approximately ( axis.x, 0f ) && Mathf.Approximately ( axis.y, 0f )) ) {
                float angle = (Angle.target_to_angle ( Vector2.zero, new Vector2 ( -axis.x, axis.y ) ) * Mathf.Rad2Deg) - 90f;
                unit.rotate ( angle );
            }


            if ( Singleton<PlayerInput>.instance.attack.down ) {
                unit.set_order ( Order_Id.Attack, true );
                player_order.set_order ( Order_Id.Attack, true );
            } else if ( !Singleton<PlayerInput>.instance.attack.held ) {
                player_order.set_order ( Order_Id.Attack, false );
            }

            if(Singleton<PlayerInput>.instance.purgatory.down) {
                if ( !PlayerManager.instance.game_camera.grey_camera.is_cancel ) {
                    if ( !is_purgatory ) {
                        active_purgatory_area ( );
                    } else {
                        inactive_purgatory_area ( );
                    }
                }
            }


            if ( player_order.layer == 0 ) {
                unit.set_order ( Order_Id.Stop, true );
            }


            // 마우스 회전
            //Camera main_camera = Camera.main;
            //Vector2 mouse_pos = Input.mousePosition;
            //Vector3 point = new Vector3 ( mouse_pos.x, mouse_pos.y, -main_camera.transform.parent.position.z );
            //Vector2 dir = main_camera.ScreenToWorldPoint ( point );
            //float angle = (Angle.target_to_angle ( unit.get_position ( ), dir ) * Mathf.Rad2Deg);
            //unit.rotate ( angle );
        }


        public override void confirm ( ) {
            base.confirm ( );
            if ( player_order == null ) {
                player_order = new UnitOrder ( );
            }

            //HACK
            //ShaderBlackBoard.instance.range = 0f;
        }

        protected override void update ( ) {
            base.update ( );
            controller ( );
        }


        private IEnumerator Eactive_purgatory ( ) {
            if ( do_purgatory_wait ) {
                yield break;
            }
            do_purgatory_wait = true;
            do_purgatory = true;
            unit.game_space = GameSpace.Both;
            // Hack
            PlayerManager.instance.game_camera.grey_camera.fade_duration = 1f;
            PlayerManager.instance.game_camera.grey_camera.active ( );



            /*
            PlayerManager.instance.game_camera.grey_camera.grey_area.SetParent ( unit.transform );
            PlayerManager.instance.game_camera.grey_camera.grey_area.localScale = new Vector2 ( 0.001f, 0.001f );

            float max = 0.15f;
            float range = 0f;
            float scale = 0.001f;
            float time = 0f;
            */

            float minX = 0.001f;
            float maxX = 12.38803f;
            float scaleX = minX;

            float minY = 0.001f;
            float maxY = 6.659164f;
            float scaleY = minY;

            float time = 0f;
            PlayerManager.instance.game_camera.grey_camera.grey_area.localScale = new Vector2 ( minX, minY );

            while (do_purgatory) {

                time += Time.deltaTime;
                if(time > 1f) {
                    time = 1f;
                }
                scaleX = Mathf.Lerp ( minX, maxX, time );
                scaleY = Mathf.Lerp ( minY, maxY, time );
                PlayerManager.instance.game_camera.grey_camera.grey_area.localScale = new Vector2 ( scaleX, scaleY );

                /*
                ShaderBlackBoard.instance.set_position ( unit.get_position() );

                time += Time.deltaTime;
                if(time > 1f) {
                    time = 1f;
                }
                range = Mathf.Lerp ( 0f, max, time );
                scale = Mathf.Lerp ( 0.001f, 13.17637f, time );

                ShaderBlackBoard.instance.set_range ( range );
                PlayerManager.instance.game_camera.grey_camera.grey_area.localPosition = new Vector2 ( 0f, unit.unit_type.transform.localPosition.y );
                PlayerManager.instance.game_camera.grey_camera.grey_area.localScale = new Vector2 ( scale, scale );
                */

                yield return null;
            }

            PlayerManager.instance.game_camera.grey_camera.fade_duration = 1f;
            PlayerManager.instance.game_camera.grey_camera.inactive ( );
            maxX = PlayerManager.instance.game_camera.grey_camera.grey_area.localScale.x;
            maxY = PlayerManager.instance.game_camera.grey_camera.grey_area.localScale.y;

            time = 0f;
            while ( PlayerManager.instance.game_camera.grey_camera.is_fading ) {
                time += Time.deltaTime;
                if ( time > 1f ) {
                    time = 1f;
                }
                scaleX = Mathf.Lerp ( maxX, minX, time );
                scaleY = Mathf.Lerp ( maxY, minY, time );
                PlayerManager.instance.game_camera.grey_camera.grey_area.localScale = new Vector2 ( scaleX, scaleY );

                yield return null;
            }

            /*
            while(ShaderBlackBoard.instance.range > 0f) {
                ShaderBlackBoard.instance.set_position ( unit.get_position ( ) );
                range = Mathf.MoveTowards ( range, 0f, Time.deltaTime );
                ShaderBlackBoard.instance.set_range ( range );
                PlayerManager.instance.game_camera.grey_camera.grey_area.localPosition = new Vector2 ( 0f, unit.unit_type.transform.localPosition.y );
                yield return null;
            }*/

            //PlayerManager.instance.game_camera.grey_camera.grey_area.SetParent ( null );
            


            unit.game_space = GameSpace.Origin;
            do_purgatory_wait = false;
        }
    }
}
