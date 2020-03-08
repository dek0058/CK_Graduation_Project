using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Game.Management {
    using JToolkit.Utility;

    public class ResourceLoader : Singleton<ResourceLoader> {

        #region Management
        private const string Path_Canvas_Blank = "Prefab/Management/SceneFader/Canvas_Blank";
        #endregion


        #region Unit
        private const string Path_Protagonist = "Unit/Character/Protagonist/Prefab/Protagonist";
        private const string Path_Piano_Man = "Unit/Character/PianoMan/Prefab/PianoMan";

        private const string Path_Melody_Missile = "Unit/Missile/MelodyMissile/Prefab/MelodyMissile";

        #endregion

        /// <summary>
        /// Resource 종류
        /// </summary>
        public enum Resource {
            // Management
            Canavs_Blank,

            // Unit
            // Character
            Protagonist,
            Piano_Man,

            // Missile
            Melody_Missile,
        }
        private EnumDictionary<Resource, PrefabData> prefabs = new EnumDictionary<Resource, PrefabData> ( );
        

        private struct PrefabData {
            public Resource type;
            public GameObject obj;
            public string path;
        }

        private bool is_loading;

        private bool do_complete;
        public bool is_complete {
            get => do_complete;
        }
        private int current_resource_index = 0;
        public int current {
            get => current_resource_index;
        }
        private int max_resource_index = 0;
        private int max {
            get => max_resource_index;
        }

        private Queue<PrefabData> load_queue = new Queue<PrefabData> ( );


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


        private void confirm ( ) {
            do_complete = false;
            add ( Resource.Canavs_Blank );
            load ( );
        }


        public GameObject get_prefab ( Resource res ) {
            if ( !prefabs.ContainsKey ( res ) ) {
                return null;
            }

            GameObject obj = prefabs[res].obj;
            if(obj == null) {
                PrefabData data = prefabs[res];
                obj = Resources.Load<GameObject> ( data.path );
                data.obj = obj;
                prefabs.Remove ( res );
                prefabs.Add ( res, data );
            }
            return obj;
        }


        public string get_path ( Resource res ) {
            string path = "none";
            switch ( res ) {
                // Management
                case Resource.Canavs_Blank:     path = Path_Canvas_Blank; break;

                // Unit
                // Character
                case Resource.Protagonist:      path = Path_Protagonist;break;
                case Resource.Piano_Man:        path = Path_Piano_Man; break;

                // Missile
                case Resource.Melody_Missile:   path = Path_Melody_Missile; break;
            }
            return path;
        }


        private IEnumerator Erequest_prefab ( ) {
            PrefabData data = load_queue.Dequeue ( );
            ResourceRequest request = Resources.LoadAsync(data.path);
            yield return request;
            data.obj = request.asset as GameObject;
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

        private void Awake ( ) {
            confirm ( );
        }


        private void OnEnable ( ) {
            if ( instance == null ) {
                instance = this;
            } else {
                if ( instance != this ) {
                    Destroy ( gameObject );
                }
            }

            if ( instance == this ) {
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
