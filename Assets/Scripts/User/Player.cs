using UnityEngine;

namespace User {
    using Management;
    using JToolkit.Utility;

    public class Player : MonoBehaviour {

        public Transform character_transform;
        public Animator character_animator;

        public Vector2 speed;

        private enum _Animation_Parameters {
            forward,
            right,
        }

        private EnumDictionary<_Animation_Parameters, int> animator_hash = new EnumDictionary<_Animation_Parameters, int> {
            {_Animation_Parameters.forward, Animator.StringToHash("forward") },
            {_Animation_Parameters.right, Animator.StringToHash("right") },
        };


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///                                                                 Unity                                                                ///
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void Update ( ) {

            // HACK : 임시
            Vector2 axis = new Vector2 (
                Singleton<PlayerInput>.instance.horizontal.value,
                Singleton<PlayerInput>.instance.vertical.value
            );

            if ( axis.x != 0f || axis.y != 0f ) {
                speed.x = Mathf.MoveTowards ( speed.x, axis.x, Time.deltaTime * 5f );
                speed.y = Mathf.MoveTowards ( speed.y, axis.y, Time.deltaTime * 5f );

                Vector2 target_position = new Vector2 ( transform.position.x - axis.x, transform.position.z - axis.y );
                float angle = (Mathf.Atan2 ( target_position.x, target_position.y ) * Mathf.Rad2Deg) - 180f;

                character_transform.rotation = Quaternion.Lerp ( character_transform.rotation, Quaternion.Euler ( 0f, angle, 0f ), Time.deltaTime * 5f );
            } else {
                speed.x = Mathf.MoveTowards ( speed.x, 0f, Time.deltaTime * 2f);
                speed.y = Mathf.MoveTowards ( speed.y, 0f, Time.deltaTime * 2f);
            }

            if ( speed.x == 0f || speed.y == 0f ) {
                character_transform.Translate ( new Vector3 ( speed.x, 0f, speed.y ) * Time.deltaTime, Space.World );
            } else {
                character_transform.Translate ( new Vector3 ( speed.x, 0f, speed.y ) * Time.deltaTime / Mathf.Sqrt ( 2 ), Space.World );
            }

            character_animator.SetFloat ( animator_hash[_Animation_Parameters.forward], speed.y );
            character_animator.SetFloat ( animator_hash[_Animation_Parameters.right], speed.x );
        }
    }
}
