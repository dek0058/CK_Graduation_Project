using UnityEngine;

namespace JToolkit.Testing {
    public class DrawFieldOfView : MonoBehaviour {

        public Vector3 position = Vector3.zero;
        public float angle = 0f;

        private int quality = 15;
        private Mesh mesh;
        public Material material;

        public float angle_fov = 40;
        public float dist_min = 5.0f;
        public float dist_max = 15.0f;
        public int layer;


       /// <summary>
       /// 원형 표시기를 생성합니다.
       /// </summary>
       /// <param name="angle_fov">표시기를 보여줄 Delta</param>
       /// <param name="dist_min">최소 거리</param>
       /// <param name="dist_max">최대 거리</param>
       /// <param name="mat">표시기에 입힐 Material</param>
       /// <returns>원형 표시기</returns>
        public static GameObject create ( float angle, float angle_fov, float dist_min, float dist_max, Material mat, int layer = 0, int quality = 15 ) {
            GameObject indicator = new GameObject ( "Circular Sector Indicator", typeof(DrawFieldOfView) );
            DrawFieldOfView component = indicator.GetComponent<DrawFieldOfView>();

            component.angle = angle;
            component.angle_fov = angle_fov;
            component.dist_min = dist_min;
            component.dist_max = dist_max;
            component.material = mat;
            component.layer = layer;
            component.quality = quality;

            return indicator;
        }

        public static GameObject create ( float angle, float angle_fov, float dist_min, float dist_max, Vector3 position, Material mat, int layer = 0, int quality = 15 ) {
            GameObject indicator = create ( angle, angle_fov, dist_min, dist_max, mat, layer, quality );
            indicator.GetComponent<DrawFieldOfView> ( ).position = position;
            return indicator;
        }

        public static GameObject create ( float angle, float angle_fov, float dist_min, float dist_max, Vector3 position, Quaternion rotation, Material mat, int layer = 0, int quality = 15 ) {
            GameObject indicator = create ( angle, angle_fov, dist_min, dist_max, position, mat, layer, quality );
            indicator.transform.rotation = rotation;
            return indicator;
        }


        //private float GetEnemyAngle ( ) {
        //    return 90 - Mathf.Rad2Deg * Mathf.Atan2 ( transform.forward.z, transform.forward.x ); // Left handed CW. z = angle 0, x = angle 90
        //}


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///                                                                 Unity                                                                ///
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        private void Start ( ) {
            mesh = new Mesh ( );
            mesh.vertices = new Vector3[4 * quality];   // Could be of size [2 * quality + 2] if circle segment is continuous
            mesh.triangles = new int[3 * 2 * quality];

            Vector3[] normals = new Vector3[4 * quality];
            Vector2[] uv = new Vector2[4 * quality];

            for ( int i = 0; i < uv.Length; i++ )
                uv[i] = new Vector2 ( 0, 0 );
            for ( int i = 0; i < normals.Length; i++ )
                normals[i] = new Vector3 ( 0, 1, 0 );

            mesh.uv = uv;
            mesh.normals = normals;

        }


        private void Update ( ) {
            if(transform.parent != null) {
                transform.SetParent ( null );
                transform.position = Vector3.zero;
                transform.eulerAngles = Vector3.zero;
            }

            //float angle_lookat = GetEnemyAngle ( );
            float angle_lookat = angle;


            float angle_start = angle_lookat - angle_fov;
            float angle_end = angle_lookat + angle_fov;
            float angle_delta = (angle_end - angle_start) / quality;

            float angle_curr = angle_start;
            float angle_next = angle_start + angle_delta;

            Vector3 pos_curr_min = Vector3.zero;
            Vector3 pos_curr_max = Vector3.zero;

            Vector3 pos_next_min = Vector3.zero;
            Vector3 pos_next_max = Vector3.zero;

            Vector3[] vertices = new Vector3[4 * quality];   // Could be of size [2 * quality + 2] if circle segment is continuous
            int[] triangles = new int[3 * 2 * quality];

            for ( int i = 0; i < quality; i++ ) {
                Vector3 sphere_curr = new Vector3 (
                Mathf.Sin ( Mathf.Deg2Rad * (angle_curr) ), 0,   // Left handed CW
                Mathf.Cos ( Mathf.Deg2Rad * (angle_curr) ) );

                Vector3 sphere_next = new Vector3 (
                Mathf.Sin ( Mathf.Deg2Rad * (angle_next) ), 0,
                Mathf.Cos ( Mathf.Deg2Rad * (angle_next) ) );

                pos_curr_min = transform.position + sphere_curr * dist_min;
                pos_curr_max = transform.position + sphere_curr * dist_max;

                pos_next_min = transform.position + sphere_next * dist_min;
                pos_next_max = transform.position + sphere_next * dist_max;

                int a = 4 * i;
                int b = 4 * i + 1;
                int c = 4 * i + 2;
                int d = 4 * i + 3;

                vertices[a] = pos_curr_min;
                vertices[b] = pos_curr_max;
                vertices[c] = pos_next_max;
                vertices[d] = pos_next_min;

                triangles[6 * i] = a;       // Triangle1: abc
                triangles[6 * i + 1] = b;
                triangles[6 * i + 2] = c;
                triangles[6 * i + 3] = c;   // Triangle2: cda
                triangles[6 * i + 4] = d;
                triangles[6 * i + 5] = a;

                angle_curr += angle_delta;
                angle_next += angle_delta;

            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;

            Graphics.DrawMesh ( mesh, position, transform.rotation, material, layer );
        }
    }
}