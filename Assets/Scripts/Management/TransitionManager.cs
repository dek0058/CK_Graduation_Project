using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Game.Management {
    using JToolkit.Utility;

    public class TransitionManager : Singleton<TransitionManager> {

        public enum SceneType {
            Intro = 1,
            Menu = 2,
            Game = 3,
        }
        private EnumDictionary<SceneType, string> scene_names = new EnumDictionary<SceneType, string> {
            {SceneType.Intro, "Scene01-Intro" },
            {SceneType.Menu, "Scene02-Menu" },
            {SceneType.Game, "Scene03-Game" },
        };


        private bool do_transition = false;
        public bool is_transition {
            get => do_transition;
        }


        public void load ( SceneType scene ) {
            StartCoroutine ( load_scenes_in_order ( scene ) );
        }


        private IEnumerator load_scenes_in_order ( SceneType scene ) {
            do_transition = true;

            yield return StartCoroutine ( SceneFader.Efade_out ( SceneFader.FadeType.Blank ) );

            yield return StartCoroutine ( Eload_scene ( scene ) );

            while(loading_progress < 0.9f) {
                yield return null;
            }

            yield return StartCoroutine ( SceneFader.Efade_in ( ) );

            do_transition = false;

            
        }


        private float loading_progress;
        public float progress {
            get => loading_progress;
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
