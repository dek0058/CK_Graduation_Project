using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Game.Management {
    using JToolkit.Utility;
    using Game.Stage;

    public class TransitionManager : Singleton<TransitionManager> {

        public enum SceneType {
            None = 0,
            Intro = 1,
            Menu = 2,
            Game_StageJoy = 3,
        }
        private EnumDictionary<SceneType, string> scene_names = new EnumDictionary<SceneType, string> {
            {SceneType.Intro, "Scene01-Intro" },
            {SceneType.Menu, "Scene02-Menu" },
            {SceneType.Game_StageJoy, "Scene03-StageJoy" },
        };

        public SceneType current {
            get; private set;
        }

        private bool do_transition = false;
        public bool is_transition {
            get => do_transition;
        }

        private float loading_progress;
        public float progress {
            get => loading_progress;
        }


        public void load_scene ( SceneType scene ) {
            StartCoroutine ( load_scenes_in_order ( scene ) );
        }


        private void confirm ( ) {
            string name = SceneManager.GetActiveScene ( ).name;

            foreach (var item in scene_names) {
                if(item.Value == name ) {
                    current = (SceneType)item.Key;
                    break;
                }
            }
        }


        private IEnumerator load_scenes_in_order ( SceneType scene ) {
            if(do_transition) {
                yield break;
            }
            do_transition = true;

            yield return StartCoroutine ( SceneFader.Efade_out ( SceneFader.FadeType.Blank ) );

            yield return StartCoroutine ( Eload_scene ( scene ) );

            while (loading_progress < 0.9f) {
                yield return null;
            }
            current = scene;

            yield return StartCoroutine ( Eload_resource ( scene ) );   // 리소스 로더 호출

            yield return StartCoroutine ( SceneFader.Efade_in ( ) );

            do_transition = false;
        }


        private IEnumerator Eload_scene ( SceneType scene ) {
            var async = SceneManager.LoadSceneAsync ( scene_names[scene] );
            async.allowSceneActivation = false;

            while(!async.isDone) {
                loading_progress = Mathf.Clamp01 ( async.progress / 0.9f ) * 100f;
                if(async.progress >= 0.9f) {
                    async.allowSceneActivation = true;
                }
                yield return null;
            }
        }


        public IEnumerator Eload_resource ( SceneType scene ) {
            ResourceLoader loader = ResourceLoader.instance;
            loader.initialize ( );

            // HACK : 나중에 수정하도록 하겠음
            //          게임 씬과 그 외 씬의 로딩 전이는 연출이 달라져야함.

            yield return StartCoroutine ( SceneFader.Efade_out ( SceneFader.FadeType.Loading ) );

            switch ( scene ) {
                case SceneType.Intro: {
                } break;
                case SceneType.Menu: { 
                } break;
                case SceneType.Game_StageJoy: {
                    StageManager.instance.current_stage.load_resource ( );
                    
                } break;
            }

            loader.load ( );

            while(!loader.is_complete) {
                float current = loader.current;
                float max = loader.max;
                // TODO : 프로그래스 바는 여기서
                yield return null;
            }

            yield return StartCoroutine ( SceneFader.Efade_in ( ) );
        }

        ////////////////////////////////////////////////////////////////////////////
        ///                               Unity                                  ///
        ////////////////////////////////////////////////////////////////////////////

        private void Awake ( ) {
            current = SceneType.None;
        }

        private void OnEnable ( ) {
            if ( instance == null ) {
                instance = this;
            } else {
                if ( instance != this ) {
                    Destroy ( gameObject );
                }
            }

            if(instance == this) {
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
