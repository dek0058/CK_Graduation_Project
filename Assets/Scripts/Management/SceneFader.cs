using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Game.Management {
    using JToolkit.Utility;

    public class SceneFader : Singleton<SceneFader> {

        public enum FadeType {
            Blank = 0,
        }

        public CanvasGroup blank_group;

        
        private CanvasGroup current_group = null; 


        public float fade_duration = 0.5f;

        private bool do_fading = false;
        public bool is_fading {
            get => do_fading;
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
            CanvasGroup group = instance.current_group;
            yield return instance.StartCoroutine ( instance.Efade ( 0f, group ) );
            instance.current_group = null;
            group.gameObject.SetActive ( false );
        }

        public static IEnumerator Efade_out ( FadeType type = FadeType.Blank ) {
            CanvasGroup group;
            switch ( type ) {
                case FadeType.Blank: {
                    group = instance.blank_group;
                }
                    break;


                // 예외처리
                default: {
                    group = instance.blank_group;
                }
                    break;
            }

            instance.current_group = group;
            group.gameObject.SetActive ( true );
            yield return instance.StartCoroutine ( instance.Efade ( 1f, group ) );
        }


        /// <summary>
        /// Scene Fader를 검증합니다.
        /// </summary>
        private void confirm ( ) {

            if(blank_group == null) {
                Transform child = transform.Find ( "Canvas_Blank" );
                if(child == null) {
                    GameObject prefab = Resources.Load<GameObject> ( "Prefab/Management/SceneFader/Canvas_Blank" );
                    GameObject blank = Instantiate ( prefab, transform );
                    blank_group = blank.GetComponent<CanvasGroup> ( );
                } else {
                    blank_group = child.GetComponent<CanvasGroup> ( );
                }
            }

            if ( blank_group != null ) {
                blank_group.alpha = 0f;
            }

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
