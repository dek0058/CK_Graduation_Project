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
            Roa,
            Pomp,

            // Missile


            // Product
            Beatbox_Platform,
            //

            // Effect
            Purgatory_Area,

            // Weapon


            // Audio
            // GameObject
            Game_Audio,

            // Music
            Stage1_Joy_Music,
            Stage1_Boss_Music,

            //////// SFX ////////
            // Room 1
            Stage1_Room01_Do_SFX,
            Stage1_Room01_Re_SFX,
            Stage1_Room01_Mi_SFX,
            Stage1_Room01_Pa_SFX,
            Stage1_Room01_Sol_SFX,
            Stage1_Room01_Ra_SFX,
            Stage1_Room01_Si_SFX,

            // Room 2
            Stage1_Room02_Sound01_SFX,
            Stage1_Room02_Sound02_SFX,
            Stage1_Room02_Sound03_SFX,
            Stage1_Room02_Sound04_SFX,

            // Room 3
            Stage1_Room03_Cello01_SFX,
            Stage1_Room03_Cello02_SFX,
            Stage1_Room03_Cello03_SFX,
            Stage1_Room03_Cello04_SFX,
            Stage1_Room03_Cello05_SFX,

            Stage1_Room03_Piano01_SFX,
            Stage1_Room03_Piano02_SFX,
            Stage1_Room03_Piano03_SFX,
            Stage1_Room03_Piano04_SFX,
            Stage1_Room03_Piano05_SFX,

            Stage1_Room03_Trumpet01_SFX,
            Stage1_Room03_Trumpet02_SFX,
            Stage1_Room03_Trumpet03_SFX,
            Stage1_Room03_Trumpet04_SFX,
            Stage1_Room03_Trumpet05_SFX,

            Stage1_Room03_Violin01_SFX,
            Stage1_Room03_Violin02_SFX,
            Stage1_Room03_Violin03_SFX,
            Stage1_Room03_Violin04_SFX,
            Stage1_Room03_Violin05_SFX,
            //////// SFX ////////
        }
        private EnumDictionary<Resource, PrefabData> prefabs = new EnumDictionary<Resource, PrefabData> ( );

        private EnumDictionary<Resource, string> paths = new EnumDictionary<Resource, string> {
            {Resource.None, "none" },

            // Management
            {Resource.Canvas_Blank, "Prefab/Management/SceneFader/Canvas_Blank" },
            {Resource.Canvas_Loading, "Prefab/Management/SceneFader/Canvas_Loading" },

            // Unit
            // Character
            {Resource.Roa, "Unit/Character/Roa/Roa"},
            {Resource.Pomp, "Unit/Character/Pomp/Pomp" },


            // Missile


            // Product
            {Resource.Beatbox_Platform, "Unit/Product/BeatboxPuzzle/Beatbox Platform" },

            // Effect
            {Resource.Purgatory_Area, "Ability/Prefab/Purgatory Area" },

            // Weapon


            // Audio
            {Resource.Game_Audio, "Audio/Prefab/GameAudio" },

            // Music
            {Resource.Stage1_Joy_Music, "Audio/Music/OST - Inconsolable" }, // Temporary
            {Resource.Stage1_Boss_Music, "Audio/Music/The Moon Over the Lake Composed by Hemio - New age piano" },

            //////// SFX ////////
            // Room 1
            {Resource.Stage1_Room01_Do_SFX, "Audio/SFX/Beat/Room_01/1_violin_1" },
            {Resource.Stage1_Room01_Re_SFX, "Audio/SFX/Beat/Room_01/1_violin_2" },
            {Resource.Stage1_Room01_Mi_SFX, "Audio/SFX/Beat/Room_01/1_violin_3" },
            {Resource.Stage1_Room01_Pa_SFX, "Audio/SFX/Beat/Room_01/1_violin_4" },
            {Resource.Stage1_Room01_Sol_SFX, "Audio/SFX/Beat/Room_01/1_violin_5" },
            {Resource.Stage1_Room01_Ra_SFX, "Audio/SFX/Beat/Room_01/1_violin_6" },
            {Resource.Stage1_Room01_Si_SFX, "Audio/SFX/Beat/Room_01/1_violin_7" },

            // Room 2
            {Resource.Stage1_Room02_Sound01_SFX, "Audio/SFX/Beat/Room_02/puzzle_Sound_1" },
            {Resource.Stage1_Room02_Sound02_SFX, "Audio/SFX/Beat/Room_02/puzzle_Sound_2" },
            {Resource.Stage1_Room02_Sound03_SFX, "Audio/SFX/Beat/Room_02/puzzle_Sound_3" },
            {Resource.Stage1_Room02_Sound04_SFX, "Audio/SFX/Beat/Room_02/puzzle_Sound_4" },

            // Room 3
            {Resource.Stage1_Room03_Cello01_SFX, "Audio/SFX/Beat/Room_03/cello/cello_1" },
            {Resource.Stage1_Room03_Cello02_SFX, "Audio/SFX/Beat/Room_03/cello/cello_2" },
            {Resource.Stage1_Room03_Cello03_SFX, "Audio/SFX/Beat/Room_03/cello/cello_3" },
            {Resource.Stage1_Room03_Cello04_SFX, "Audio/SFX/Beat/Room_03/cello/cello_4" },
            {Resource.Stage1_Room03_Cello05_SFX, "Audio/SFX/Beat/Room_03/cello/cello_5" },

            {Resource.Stage1_Room03_Piano01_SFX, "Audio/SFX/Beat/Room_03/piano/piano_1" },
            {Resource.Stage1_Room03_Piano02_SFX, "Audio/SFX/Beat/Room_03/piano/piano_2" },
            {Resource.Stage1_Room03_Piano03_SFX, "Audio/SFX/Beat/Room_03/piano/piano_3" },
            {Resource.Stage1_Room03_Piano04_SFX, "Audio/SFX/Beat/Room_03/piano/piano_4" },
            {Resource.Stage1_Room03_Piano05_SFX, "Audio/SFX/Beat/Room_03/piano/piano_5" },

            {Resource.Stage1_Room03_Trumpet01_SFX, "Audio/SFX/Beat/Room_03/trumpet/trumpet_1" },
            {Resource.Stage1_Room03_Trumpet02_SFX, "Audio/SFX/Beat/Room_03/trumpet/trumpet_2" },
            {Resource.Stage1_Room03_Trumpet03_SFX, "Audio/SFX/Beat/Room_03/trumpet/trumpet_3" },
            {Resource.Stage1_Room03_Trumpet04_SFX, "Audio/SFX/Beat/Room_03/trumpet/trumpet_4" },
            {Resource.Stage1_Room03_Trumpet05_SFX, "Audio/SFX/Beat/Room_03/trumpet/trumpet_5" },

            {Resource.Stage1_Room03_Violin01_SFX, "Audio/SFX/Beat/Room_03/violin/violin_1" },
            {Resource.Stage1_Room03_Violin02_SFX, "Audio/SFX/Beat/Room_03/violin/violin_2" },
            {Resource.Stage1_Room03_Violin03_SFX, "Audio/SFX/Beat/Room_03/violin/violin_3" },
            {Resource.Stage1_Room03_Violin04_SFX, "Audio/SFX/Beat/Room_03/violin/violin_4" },
            {Resource.Stage1_Room03_Violin05_SFX, "Audio/SFX/Beat/Room_03/violin/violin_5" },
            //////// SFX ////////
        };

        private EnumDictionary<Resource, string> prefabs_name = new EnumDictionary<Resource, string> ( ) {
            // Characters
            {Resource.Roa, typeof(Unit.Character.URoa).Name},
            {Resource.Pomp, typeof(Unit.Character.UPomp).Name},


            // Missiles

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

        public Object get_prefab ( int index ) {
            Resource res = (Resource)index;
            if ( res == Resource.None ) {
                return null;
            }

            if ( !prefabs.ContainsKey ( res ) ) {
                PrefabData data = create_data ( res );
                prefabs.Add ( res, data );
                return data.obj;
            }

            Object obj = prefabs[res].obj;
            if ( obj == null ) {  // 예외처리 : 만약 키값은 존재하는데 내용물이 없을 경우
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
