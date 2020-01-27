using UnityEngine;

namespace JToolkit.Math {
    public class Polar {

        public static float radian_x ( float distance, float angle) {
            return distance * (float)System.Math.Cos ( angle );
        }

        public static float radian_y ( float distance, float angle ) {
            return distance * (float)System.Math.Sin ( angle );
        }

        public static float x ( float distance, float angle ) {
            return distance * (float)System.Math.Cos ( angle * Mathf.Deg2Rad );
        }

        public static float y ( float distance, float angle ) {
            return distance * (float)System.Math.Sin ( angle * Mathf.Deg2Rad );
        }

        public static Vector2 location ( float distance, float angle ) {
            return new Vector2 ( x ( distance, angle ), y ( distance, angle ) );
        }

    }
}
