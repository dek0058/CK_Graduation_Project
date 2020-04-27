using UnityEngine;
using System.Collections.Generic;
using PathCreation;

namespace Game.Obj.Puzzle {
    using JToolkit.Utility;
    using Management;
    using Unit.Product;

    [DisallowMultipleComponent ( ), RequireComponent (typeof(PathCreator))]
    public class BeatboxPuzzle : MonoBehaviour {

        public enum Clip {
            // SFX
            Stage1_Piano_Do_SFX = ResourceLoader.Resource.Stage1_Piano_Do_SFX,
            Stage1_Piano_Re_SFX = ResourceLoader.Resource.Stage1_Piano_Re_SFX,
            Stage1_Piano_Mi_SFX = ResourceLoader.Resource.Stage1_Piano_Mi_SFX,
            Stage1_Piano_Pa_SFX = ResourceLoader.Resource.Stage1_Piano_Pa_SFX,
            Stage1_Piano_Sol_SFX = ResourceLoader.Resource.Stage1_Piano_Sol_SFX,
            Stage1_Piano_Ra_SFX = ResourceLoader.Resource.Stage1_Piano_Ra_SFX,
            Stage1_Piano_Si_SFX = ResourceLoader.Resource.Stage1_Piano_Si_SFX,

            Stage1_Drum_Do_SFX = ResourceLoader.Resource.Stage1_Drum_Do_SFX,
            Stage1_Drum_Re_SFX = ResourceLoader.Resource.Stage1_Drum_Re_SFX,
            Stage1_Drum_Mi_SFX = ResourceLoader.Resource.Stage1_Drum_Mi_SFX,
            Stage1_Drum_Pa_SFX = ResourceLoader.Resource.Stage1_Drum_Pa_SFX,
            Stage1_Drum_Sol_SFX = ResourceLoader.Resource.Stage1_Drum_Sol_SFX,
            Stage1_Drum_Ra_SFX = ResourceLoader.Resource.Stage1_Drum_Ra_SFX,
            Stage1_Drum_Si_SFX = ResourceLoader.Resource.Stage1_Drum_Si_SFX,

            Stage1_Guitar_Do_SFX = ResourceLoader.Resource.Stage1_Guitar_Do_SFX,
            Stage1_Guitar_Re_SFX = ResourceLoader.Resource.Stage1_Guitar_Re_SFX,
            Stage1_Guitar_Mi_SFX = ResourceLoader.Resource.Stage1_Guitar_Mi_SFX,
            Stage1_Guitar_Pa_SFX = ResourceLoader.Resource.Stage1_Guitar_Pa_SFX,
            Stage1_Guitar_Sol_SFX = ResourceLoader.Resource.Stage1_Guitar_Sol_SFX,
            Stage1_Guitar_Ra_SFX = ResourceLoader.Resource.Stage1_Guitar_Ra_SFX,
            Stage1_Guitar_Si_SFX = ResourceLoader.Resource.Stage1_Guitar_Si_SFX,

            Stage1_Bass_Do_SFX = ResourceLoader.Resource.Stage1_Bass_Do_SFX,
            Stage1_Bass_Re_SFX = ResourceLoader.Resource.Stage1_Guitar_Re_SFX,
            Stage1_Bass_Mi_SFX = ResourceLoader.Resource.Stage1_Guitar_Mi_SFX,
            Stage1_Bass_Pa_SFX = ResourceLoader.Resource.Stage1_Guitar_Pa_SFX,
            Stage1_Bass_Sol_SFX = ResourceLoader.Resource.Stage1_Guitar_Sol_SFX,
            Stage1_Bass_Ra_SFX = ResourceLoader.Resource.Stage1_Guitar_Ra_SFX,
            Stage1_Bass_Si_SFX = ResourceLoader.Resource.Stage1_Guitar_Si_SFX,
        }


        [System.Serializable]
        public class Platform {
            [System.NonSerialized]
            public UBeatboxPlatform component;
            [System.NonSerialized]
            public GameObject game_object;
            [System.NonSerialized]
            public Transform transform;
            public Clip clip;
            public float time;

            public Platform ( ) {}
            public Platform ( UBeatboxPlatform c, GameObject g, Transform t ) =>
                (component, game_object, transform) = (c, g, t);
        }
        public List<Platform> platforms = new List<Platform> ( );
        [Tooltip("총 시간")]
        public float amount = 0f;

        [SerializeField]
        private PathCreator path_creator;


        public void add ( Platform p ) {
            GameObject piece_obj = Instantiate (
                ResourceLoader.instance.get_prefab ( ResourceLoader.Resource.Beatbox_Platform ) as GameObject,
                transform.position, Quaternion.identity, transform );

            p.game_object = piece_obj;
            p.transform = piece_obj.transform;
            p.component = piece_obj.GetComponent<UBeatboxPlatform> ( );
        }


        public void clear ( ) {
            while(transform.childCount > 0) {
#if UNITY_EDITOR
                DestroyImmediate ( transform.GetChild ( 0 ).gameObject );
#else
            Destroy ( transform.GetChild ( 0 ).gameObject );
#endif
            }
        }


        public void check ( ) {
            bool flag = false;
            foreach ( var p in platforms ) {
                if(p.game_object == null) {
                    flag = true;
                    break;
                }

                if( p.transform == null ) {
                    p.transform = p.game_object.transform;
                }

                if( p.component == null ) {
                    p.component = p.game_object.GetComponent<UBeatboxPlatform> ( );
                    if(p == null) {
                        flag = true;
                        break;
                    }
                }
            }
            if ( flag ) {
                clear ( );

                for ( int i = transform.childCount; i < platforms.Count; ++i ) {
                    add ( platforms[i] );
                }
            }
        }


        /// <summary>
        /// 발판들의 총 시간을 계산합니다. 만약 초과할 경우 그 시간으로 총 시간을 변경합니다.
        /// </summary>
        public void time_caculate ( ) {
            foreach ( var p in platforms ) {
                if(p.time > amount) {
                    amount = p.time;
                }
            }
        }

        /// <summary>
        /// 발판들을 시간에 맞춰 퍼즐에 배치합니다.
        /// </summary>
        public void arrangement ( ) {
            if ( Mathf.Approximately ( amount, 0f ) ) {
                return;
            }

            foreach(var p in platforms) {
                p.transform.position = way_position ( p.time / amount );
            }
        }


        public void confirm ( ) {
            if ( path_creator == null ) {
                path_creator = GetComponent<PathCreator> ( );
            }

            for(int i = transform.childCount; i < platforms.Count; ++i ) {
                add ( platforms[i] );
            }

            check ( );
        }


        private void initialize ( ) {
            time_caculate ( );
            arrangement ( );
        }


        public Vector2 way_position ( float t ) {
            return path_creator.path.GetPointAtTime ( t, EndOfPathInstruction.Stop );
        }

        ////////////////////////////////////////////////////////////////////////////
        ///                               Unity                                  ///
        ////////////////////////////////////////////////////////////////////////////

        private void Awake ( ) {
            confirm ( );
        }

        private void Start ( ) {
            initialize ( );
        }
    }
}
