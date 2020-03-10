using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Game.Management {
    using JToolkit.Utility;

    public class SceneFader : Singleton<SceneFader> {

        private const string Name_Blank = "Canvas_Blank";
        private const string Name_Loading = "Canvas_Loading";

        public enum FadeType {
            Blank = 0,
            Loading,
        }

        public CanvasGroup blank_group;
        public CanvasGroup loading_group;

        private Stack<CanvasGroup> group_stack = new Stack<CanvasGroup> ( );

        public float fade_duration = 0.5f;

        private bool do_fading = false;
        public bool is_fading {
            get => do_fading;
        }

        private bool is_initilaize = false;

        /// <summary>
        /// Scene Fader를 검증합니다.
        /// </summary>
        private void confirm ( ) {
            is_initilaize = false;
            ResourceLoader loader = ResourceLoader.instance;
            Transform child;
            GameObject prefab;
            GameObject obj;

            if ( blank_group == null ) {
                child = transform.Find ( Name_Blank );
                if ( child == null ) {
                    prefab = loader.get_prefab ( ResourceLoader.Resource.Canvas_Blank );
                    obj = Instantiate ( prefab, transform );
                    obj.name = Name_Blank;
                    blank_group = obj.GetComponent<CanvasGroup> ( );
                } else {
                    blank_group = child.GetComponent<CanvasGroup> ( );
                }
            }

            if ( loading_group == null ) {
                child = transform.Find ( Name_Loading );
                if ( child == null ) {
                    prefab = loader.get_prefab ( ResourceLoader.Resource.Canvas_Loading );
                    obj = Instantiate ( prefab, transform );
                    obj.name = Name_Loading;
                    loading_group = obj.GetComponent<CanvasGroup> ( );
                } else {
                    loading_group = child.GetComponent<CanvasGroup> ( );
                }
            }

            if ( blank_group != null ) {
                blank_group.alpha = 0f;
            }

            is_initilaize = true;
        }


        private IEnumerator Efade ( float alpha, CanvasGroup group ) {
            do_fading = true;
            group.blocksRaycasts = true;
            float speed = Mathf.Abs ( group.alpha - alpha ) / fade_duration;
            while(!Mathf.Approximately(group.alpha, alpha)) {
                group.alpha = Mathf.MoveTowards ( group.alpha, alpha, speed * Time.deltaTime );
                yield return new WaitForEndOfFrame();
            }
            group.alpha = alpha;
            do_fading = false;
            group.blocksRaycasts = false;
        }



        public static IEnumerator Efade_in ( ) {
            while ( !instance.is_initilaize ) {
                yield return null;
            }

            if ( instance.group_stack.Count == 0) {
                yield break;
            }
            CanvasGroup group = instance.group_stack.Pop ( );
            yield return instance.StartCoroutine ( instance.Efade ( 0f, group ) );
            group.gameObject.SetActive ( false );
        }


        public static IEnumerator Efade_out ( FadeType type = FadeType.Blank ) {
            while ( !instance.is_initilaize ) {
                yield return null;
            }

            CanvasGroup group = null;
            switch ( type ) {
                case FadeType.Blank:    { group = instance.blank_group;    } break;
                case FadeType.Loading:  { group = instance.loading_group;  } break;
            }

            instance.group_stack.Push ( group );
            group.gameObject.SetActive ( true );
            yield return instance.StartCoroutine ( instance.Efade ( 1f, group ) );
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
                confirm ( );    // 초기화
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
