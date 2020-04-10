using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Game.Management {
    using JToolkit.Utility;

    public class ResourceLoader : Singleton<ResourceLoader> {

        /// <summary>
        /// Resource 종류
        /// </summary>
        public enum Resource {
            None = 0,

            // Management
            Canvas_Blank,
            Canvas_Loading,

            // Unit
            // Character
            Protagonist,
            Piano_Man,
            Paint_Man,

            // Missile
            BlueNote1,
            BlueNote2,
            BlueNote3,
            RedNote1,
            RedNote2,
            RedNote3,
            //

            // Audio
            // GameObject
            Game_Audio,

            // Clip
            Stage1_Joy_Music,
            Stage1_Boss_Music,
            //
        }
        private EnumDictionary<Resource, PrefabData> prefabs = new EnumDictionary<Resource, PrefabData> ( );

        private EnumDictionary<Resource, string> paths = new EnumDictionary<Resource, string> {
            {Resource.None, "none" },

            {Resource.Canvas_Blank, "Prefab/Management/SceneFader/Canvas_Blank" },
            {Resource.Canvas_Loading, "Prefab/Management/SceneFader/Canvas_Loading" },

            {Resource.Protagonist, "Unit/Character/Protagonist/Protagonist" },
            {Resource.Piano_Man, "Unit/Character/PianoMan/PianoMan" },
            {Resource.Paint_Man, "Unit/Character/PaintMan/PaintMan" },

            {Resource.BlueNote1, "Unit/Missile/MelodyMissile/BlueNote1_Missile" },
            {Resource.BlueNote2, "Unit/Missile/MelodyMissile/BlueNote2_Missile" },
            {Resource.BlueNote3, "Unit/Missile/MelodyMissile/BlueNote3_Missile" },
            {Resource.RedNote1, "Unit/Missile/MelodyMissile/RedNote1_Missile" },
            {Resource.RedNote2, "Unit/Missile/MelodyMissile/RedNote2_Missile" },
            {Resource.RedNote3, "Unit/Missile/MelodyMissile/RedNote3_Missile" },

            {Resource.Game_Audio, "Audio/Prefab/GameAudio" },

            {Resource.Stage1_Joy_Music, "Audio/Music/OST - Inconsolable" }, // Temporary
            {Resource.Stage1_Boss_Music, "Audio/Music/The Moon Over the Lake Composed by Hemio - New age piano" },
        };

        private EnumDictionary<Resource, string> prefabs_name = new EnumDictionary<Resource, string> ( ) {
            // Characters
            {Resource.Protagonist, "UProtagonist" },
            {Resource.Piano_Man, "UPianoMan" },
            {Resource.Paint_Man, "UPaintMan" },

            // Missiles
            {Resource.BlueNote1, "UBlueNote1" },
            {Resource.BlueNote2, "UBlueNote2" },
            {Resource.BlueNote3, "UBlueNote3" },
            {Resource.RedNote1, "URedNote1" },
            {Resource.RedNote2, "URedNote2" },
            {Resource.RedNote3, "URedNote3" },
        };
        

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
            data.path = paths[res];
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
            if(res == Resource.None) {
                return null;
            }

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


        public GameObject get_unit ( string prefab_name ) {
            Resource res = Resource.None;
            foreach(var name in prefabs_name) {
                if(name.Value.Equals(prefab_name)) {
                    res = (Resource)name.Key;
                    break;
                }
            }
            return get_prefab ( res ) as GameObject;
        }


        private PrefabData create_data ( Resource res ) {
            PrefabData data = new PrefabData ( );
            data.obj = Resources.Load ( paths[res] );
            return data;
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
