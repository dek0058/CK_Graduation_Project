using UnityEngine;

namespace Game.User {
    using Game.Management;
    using JToolkit.Utility;
    using JToolkit.Math;
    using Game.Unit;

    public class Player : MonoBehaviour {

        public Unit unit;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///                                                                 Unity                                                                ///
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void Update ( ) {

            Vector2 axis = new Vector2 (
                Singleton<PlayerInput>.instance.horizontal.value,
                Singleton<PlayerInput>.instance.vertical.value
            );
            unit.move ( axis );

            // 키보드 회전
            if ( !(Mathf.Approximately ( axis.x, 0f ) && Mathf.Approximately ( axis.y, 0f )) ) {
                float angle = Angle.target_to_angle ( Vector2.zero, axis ) * Mathf.Rad2Deg;
                unit.rotate ( angle );
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
