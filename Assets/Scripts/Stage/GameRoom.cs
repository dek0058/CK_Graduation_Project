using UnityEngine;
using System.Collections.Generic;

namespace Game.Stage {
    using Unit;
    using Management;
    using User;
    using Obj;

    public abstract class GameRoom : MonoBehaviour{

        public PolygonCollider2D confiner_area;




        [Header("Confiner Area")]
        [SerializeField]
        private Vector2 confiner_box_right = Vector2.zero;
        [SerializeField]
        private Vector2 confiner_box_left = Vector2.zero;


        public void join ( UUnit unit ) {
            //if ( unit.player.is_local ) {
            //    Preferences.instance.event_resolution_change += set_confiner_area;
            //    set_confiner_area ( );
            //    PlayerManager.instance.game_camera.confiner_transform.position = confiner_area.transform.position;
            //}
        }


        public void quit ( UUnit unit ) {
            

            if(unit.player.is_local) {
                Preferences.instance.event_resolution_change -= set_confiner_area;
            }
        }

        public virtual void save ( ) {

        }

        public virtual void load ( ) {

        }


        public void set_confiner_area ( ) {
            Vector2 cam_point = Camera.main.ViewportToWorldPoint ( new Vector2 ( 0, 0 ) );

            float cam_x = Mathf.Abs ( cam_point.x );
            float cam_y = Mathf.Abs ( cam_point.y );

            Vector2 right = Vector2.zero;
            Vector2 left = Vector2.zero;

            if ( cam_x > confiner_box_right.x ) {
                right.x = 0f;
            } else {
                right.x = confiner_box_right.x - cam_x;
            }

            if ( cam_y > confiner_box_right.y ) {
                right.y = 0f;
            } else {
                right.y = confiner_box_right.y - cam_y;
            }

            if (cam_x > confiner_box_left.x) {
                left.x = 0f;
            } else {
                left.x = confiner_box_left.x - cam_x;
            }

            if(cam_y > confiner_box_left.y) {
                left.y = 0f;
            } else {
                left.y = confiner_box_left.y - cam_y;
            }

            Vector2[] points = new Vector2[] { 
                new Vector2(-right.x, -right.y),
                new Vector2(left.x, -right.y),
                new Vector2(left.x, left.y),
                new Vector2(-right.x, left.y),
            };

            //PlayerManager.instance.game_camera.confiner_area.points = points;
            //PlayerManager.instance.game_camera.cv_confiner.InvalidatePathCache ( );
        }


        protected virtual void active ( ) {

        }


        protected virtual void inactive ( ) {

        }


        protected virtual void update ( ) {
           
        }


        /// <summary>
        /// Room을 검증합니다.
        /// </summary>
        public virtual void confirm ( ) {
            if( confiner_area  == null) {
                confiner_area = GetComponent<PolygonCollider2D> ( );
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        ///                               Unity                                  ///
        ////////////////////////////////////////////////////////////////////////////

        private void Awake ( ) {
            confirm ( );
        }


        private void Update ( ) {
            update ( );
        }
    }
}
