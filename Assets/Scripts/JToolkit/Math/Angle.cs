using UnityEngine;

namespace JToolkit.Math {
    public class Angle {

        public static float target_to_angle ( float x, float y, float tx, float ty ) {
            float dx = tx - x;
            float dy = ty - y;
            return (float)System.Math.Atan2 ( dx, dy );
        }


        public static float target_to_angle ( Vector2 v, Vector2 tv ) {
            Vector2 dv = new Vector2 ( tv.x - v.x, tv.y - v.y );
            return (float)System.Math.Atan2 ( dv.x, dv.y );
        }


        public static float trim ( float angle ) {
            if(angle > 360f) {
                angle -= 360f;
            } else if(angle < 0f) {
                angle += 360f;
            }
            return angle;
        }
    }
}
