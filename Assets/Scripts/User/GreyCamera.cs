using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Game.User {
    public class GreyCamera : MonoBehaviour {
        public Camera sub_camera;
        private RenderTexture render_target;

        [SerializeField]
        private CanvasGroup grey_group;

        [SerializeField]
        private RawImage grey_image = null;
        [SerializeField]
        private RawImage color_image = null;

        [Range(0.1f, 10f)]
        public float fade_duration = 0.5f;

        private bool do_fading = false;
        public bool is_fading {
            get => do_fading;
        }


        public void initialize ( ) {
            grey_group.alpha = 0;

            if ( sub_camera.targetTexture != null )
                sub_camera.targetTexture.Release ( );

            render_target = new RenderTexture ( Screen.width, Screen.height, 24 );
            sub_camera.targetTexture = render_target;

            grey_image.texture = render_target;
            color_image.texture = render_target;
        }


        public void active ( ) {
            StartCoroutine ( Efade_out ( ) );
        }


        public void inactive ( ) {
            StartCoroutine ( Efade_in ( ) );
        }


        public void turn_off ( ) {
            grey_group.alpha = 0f;
            grey_group.gameObject.SetActive ( false );
        }


        public void confirm ( ) {
            if(sub_camera == null) {
                sub_camera = GetComponentInChildren<Camera> ( );
            }

            if ( grey_group == null ) {
                grey_group = grey_group.GetComponentInChildren<CanvasGroup> ( );
            }
        }


        private IEnumerator Efade ( float alpha, CanvasGroup group ) {
            if(do_fading) {
                yield break;
            }
            do_fading = true;
            float speed = Mathf.Abs ( group.alpha - alpha ) / fade_duration;
            while ( !Mathf.Approximately ( group.alpha, alpha ) ) {
                group.alpha = Mathf.MoveTowards ( group.alpha, alpha, speed * Time.deltaTime );
                yield return new WaitForEndOfFrame ( );
            }
            group.alpha = alpha;
            do_fading = false;
        }


        private IEnumerator Efade_in ( ) {
            yield return StartCoroutine ( Efade ( 0f, grey_group ) );
            grey_group.gameObject.SetActive ( false );
        }


        private IEnumerator Efade_out ( ) {
            grey_group.gameObject.SetActive ( true );
            yield return StartCoroutine ( Efade ( 1f, grey_group ) );
        }


        ////////////////////////////////////////////////////////////////////////////
        ///                               Unity                                  ///
        ////////////////////////////////////////////////////////////////////////////

        private void Awake ( ) {
            confirm ( );
        }

        private void Start ( ) {
            initialize ( );
            grey_group.gameObject.SetActive ( false );
        }

        private void Update ( ) {
            if(Input.GetKeyDown(KeyCode.B)) {
                if(grey_group.isActiveAndEnabled) {
                    inactive ( );
                } else {
                    active ( );
                }
            }   
        }
    }
}
