using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Game.Management {
    using JToolkit.Utility;

    public class ResourceLoader : Singleton<ResourceLoader> {

        #region Management
        private const string Path_Canvas_Blank = "Prefab/Management/SceneFader/Canvas_Blank";
        private const string Path_Canvas_Loading = "Prefab/Management/SceneFader/Canvas_Loading";
        #endregion

        #region Unit
        private const string Path_Protagonist = "Unit/Character/Protagonist/Prefab/Protagonist";
        private const string Path_Piano_Man = "Unit/Character/PianoMan/Prefab/PianoMan";

        private const string Path_Melody_Missile = "Unit/Missile/MelodyMissile/Prefab/MelodyMissile";
        #endregion

        #region Auido
        private const string Path_Game_Audio = "Audio/Prefab/GameAudio";

        //Clip
        private const string Path_Stage1_Boss_Music = "Audio/Music/The Moon Over the Lake Composed by Hemio - New age piano";
        #endregion

        /// <summary>
        /// Resource 종류
        /// </summary>
        public enum Resource {
            // Management
            Canvas_Blank,
            Canvas_Loading,

            // Unit
            // Character
            Protagonist,
            Piano_Man,

            // Missile
            Melody_Missile,
            //

            // Audio
            // GameObject
            Game_Audio,

            // Clip
            Stage1_Boss_Music,
            //
        }
        private EnumDictionary<Resource, PrefabData> prefabs = new EnumDictionary<Resource, PrefabData> ( );
        

        private struct PrefabData {
            public Resource type;
            public Object obj;
            public string path;
        }

        private bool is_loading = false;

        private bool do_complete = false;
        public bool is_complete {
            get => do_complete;
        }
        private int current_resource_index = 0;
        public int current {
            get => current_resource_index;
        }
        private int max_resource_index = 0;
        public int max {
            get => max_resource_index;
        }

        private Queue<PrefabData> load_queue = new Queue<PrefabData> ( );


        public void initialize ( ) {
            do_complete = false;
        }


        public void load ( ) {
            StartCoroutine ( Eupdate ( ) );
        }


        public void add ( Resource res ) {
            bool flag = prefabs.ContainsKey ( res );
            if(flag) {
                if( prefabs[res].obj == null ) {
                    flag = false;
                }
            }
            if(flag) {  // 이미 값이 있으므로 스킵
                return;
            }
            PrefabData data = new PrefabData ( );
            data.type = res;
            data.path = get_path ( res );
            data.obj = null;
            load_queue.Enqueue ( data );
            max_resource_index++;
        }


        public void confirm ( ) {
            TransitionManager transition = TransitionManager.instance;
            if (!transition.is_transition) {
                StartCoroutine ( transition.Eload_resource ( transition.current ) );
            }
        }


        public Object get_prefab ( Resource res ) {
            if ( !prefabs.ContainsKey ( res ) ) {
                PrefabData data = create_data ( res );
                prefabs.Add ( res, data );
                return data.obj;
            }

            Object obj = prefabs[res].obj;
            if( obj == null) {  // 예외처리 : 만약 키값은 존재하는데 내용물이 없을 경우
                PrefabData data = create_data ( res );
                prefabs.Remove ( res );
                prefabs.Add ( res, data );
                return data.obj;
            }
            return obj;
        }


        private PrefabData create_data ( Resource res ) {
            PrefabData data = new PrefabData ( );
            data.obj = Resources.Load ( get_path ( res ) );
            return data;
        }


        public string get_path ( Resource res ) {
            string path = "none";
            switch ( res ) {
                // Management
                case Resource.Canvas_Blank:         { path = Path_Canvas_Blank;         } break;
                case Resource.Canvas_Loading:       { path = Path_Canvas_Loading;       } break;

                // Unit
                // Character
                case Resource.Protagonist:          { path = Path_Protagonist;          } break;
                case Resource.Piano_Man:            { path = Path_Piano_Man;            } break;

                // Missile
                case Resource.Melody_Missile:       { path = Path_Melody_Missile;       } break;
                //

                // Audio
                case Resource.Game_Audio:           { path = Path_Game_Audio;           } break;

                //Clip
                case Resource.Stage1_Boss_Music:    { path = Path_Stage1_Boss_Music;    } break;
            }
            return path;
        }


        private IEnumerator Erequest_prefab ( ) {
            PrefabData data = load_queue.Dequeue ( );
            ResourceRequest request = Resources.LoadAsync(data.path);
            yield return request;
            data.obj = request.asset;
            prefabs.Add ( data.type, data );
            current_resource_index++;
        }
        

        private IEnumerator Eupdate ( ) {
            if( is_loading ) {
                yield break;
            }

            is_loading = true;
            do_complete = false;
            while ( load_queue.Count > 0 ) {
                yield return Erequest_prefab ( );
            }
            is_loading = false;
            do_complete = true;
        }

        ////////////////////////////////////////////////////////////////////////////
        ///                               Unity                                  ///
        ////////////////////////////////////////////////////////////////////////////

        private void OnEnable ( ) {
            if ( instance == null ) {
                instance = this;
            } else {
                if ( instance != this ) {
                    Destroy ( gameObject );
                }
            }

            if ( instance == this ) {
                confirm ( );
                DontDestroyOnLoad ( gameObject );
            }
        }

        private void OnDestroy ( ) {
            if ( instance == this ) { _app_is_close = true; }
        }

        private void OnApplicationQuit ( ) {
            if ( instance == this ) { _app_is_close = true; }
        }
    }
}
