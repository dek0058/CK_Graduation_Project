using UnityEngine;

namespace Game.User {
    using Game.Management;
    using JToolkit.Utility;
    using JToolkit.Math;
    using Game.Unit;

    public class Player : MonoBehaviour {

        public Unit unit;
        public UnitOrder player_order = null;


        private void confirm ( ) {
            if ( player_order == null ) {
                player_order = new UnitOrder ( );
            }
        }


        ////////////////////////////////////////////////////////////////////////////
        ///                               Unity                                  ///
        ////////////////////////////////////////////////////////////////////////////

        private void Awake ( ) {
            confirm ( );
        }


        private void Update ( ) {

            // 이동
            Vector2 axis = new Vector2 (
                Singleton<PlayerInput>.instance.horizontal.value,
                Singleton<PlayerInput>.instance.vertical.value
            );

            bool flag = Singleton<PlayerInput>.instance.horizontal.receiving_input || Singleton<PlayerInput>.instance.vertical.receiving_input;
            // 이동 키를 누르고 있으므로 유닛에게 이동 명령을 알림
            unit.set_order ( Order_Id.Move, flag );
            player_order.set_order ( Order_Id.Move, flag );
            unit.move ( axis );

            // 키보드 회전
            if ( !(Mathf.Approximately ( axis.x, 0f ) && Mathf.Approximately ( axis.y, 0f )) ) {
                float angle = Angle.target_to_angle ( Vector2.zero, axis ) * Mathf.Rad2Deg;
                unit.rotate ( angle );
            }


            if ( Singleton<PlayerInput>.instance.attack.down ) {
                unit.set_order ( Order_Id.Attack, true );
                player_order.set_order ( Order_Id.Attack, true );
            } else if ( !Singleton<PlayerInput>.instance.attack.held ) {
                player_order.set_order ( Order_Id.Attack, false );
            }


            if( player_order.layer == 0) {
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
    }
}
