﻿#define 라상목

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering;

namespace Game.User
{
    using Management;

    public class GreyCamera : MonoBehaviour {
        public Camera sub_camera;
        public Transform target;
        public Transform grey_area;
        public PixelPerfectCamera pixel_perfect_camera;

        private RenderTexture render_target;

        [SerializeField]
        private CanvasGroup grey_group;

        [SerializeField]
        private RawImage grey_image = null;
        [SerializeField]
        private RawImage color_image = null;

        [SerializeField]
        private RectTransform canvas_transform = null;
        [SerializeField]
        private RectTransform mask_transform = null;
        [SerializeField]
        private RectTransform color_transform = null;

        [Range ( 0.1f, 10f )]
        public float fade_duration = 0.5f;

        private bool do_cancel = false;
        public bool is_cancel {
            get => do_cancel;
        }

        private bool do_fading = false;
        public bool is_fading {
            get => do_fading;
        }


        private void pixel_perfect_adjust ( ) {
            pixel_perfect_camera.refResolutionX = Screen.width;
            pixel_perfect_camera.refResolutionY = Screen.height;
        }


        public void initialize ( ) {
            grey_group.alpha = 0;

            if ( sub_camera.targetTexture != null )
                sub_camera.targetTexture.Release ( );

            render_target = new RenderTexture ( sub_camera.pixelWidth, sub_camera.pixelHeight, 24, RenderTextureFormat.DefaultHDR );
            sub_camera.targetTexture = render_target;

            grey_image.texture = render_target;
            color_image.texture = render_target;
#if 라상목
            RenderPipelineManager.beginCameraRendering += (_, sub_camera) =>
            {
                OnPreRender();
            };
#endif
        }
#if 라상목
        private void OnPreRender()
        {
            if (target != null)
            {
                grey_area.position = target.position;
            }

            if (canvas_transform != null)
                color_transform.SetParent(canvas_transform);

            Vector2 screen_point = sub_camera.WorldToScreenPoint(grey_area.position);
            Vector2 to_ui_point = screen_point - new Vector2(Screen.currentResolution.width / 2, Screen.currentResolution.height / 2);
            mask_transform.anchoredPosition = to_ui_point;

            if (mask_transform != null)
            {
                color_transform.SetParent(mask_transform);
                mask_transform.sizeDelta = grey_area.localScale * 64f;
            }
        }
#endif

        public void active ( ) {
            StartCoroutine(Efade_out());
        }


        public void inactive ( ) {
            StartCoroutine(Efade_in());
        }


        public void turn_off ( ) {
            grey_group.alpha = 0f;
            grey_group.gameObject.SetActive ( false );
        }


        public void confirm ( ) {
            if ( sub_camera == null ) {
                sub_camera = GetComponentInChildren<Camera> ( );
            }

            if ( grey_group == null ) {
                grey_group = grey_group.GetComponentInChildren<CanvasGroup> ( );
            }

            if ( pixel_perfect_camera == null ) {
                pixel_perfect_camera = sub_camera.GetComponent<PixelPerfectCamera> ( );
                Preferences.instance.event_resolution_change += pixel_perfect_adjust;
            }
        }


        private IEnumerator Efade ( float alpha, CanvasGroup group ) {
            if ( do_fading ) {
                yield break;
            }
            do_fading = true;
            float speed = Mathf.Abs ( group.alpha - alpha ) / fade_duration;
            while ( !Mathf.Approximately ( group.alpha, alpha ) ) {
                if( do_cancel ) {
                    break;
                }
                group.alpha = Mathf.MoveTowards ( group.alpha, alpha, speed * Time.deltaTime );
                yield return new WaitForEndOfFrame ( );
            }
            group.alpha = alpha;
            do_fading = false;
            
        }

        private IEnumerator Ecancel ( ) {
            do_cancel = true;
            while(do_cancel) {
                if(!do_fading) {
                    break;
                }
                yield return null;
            }
            do_cancel = false;
        }

        private IEnumerator Efade_in ( ) {
            if(do_fading) {
                yield return StartCoroutine ( Ecancel() );
            }
            grey_area.gameObject.SetActive ( true );
            yield return StartCoroutine ( Efade ( 0f, grey_group ) );
            grey_group.gameObject.SetActive ( false );
            grey_area.gameObject.SetActive ( false );
        }


        private IEnumerator Efade_out ( ) {
            if(do_fading) {
                yield return StartCoroutine ( Ecancel ( ) );
            }
            grey_area.gameObject.SetActive ( true );
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
            grey_area.gameObject.SetActive ( false );
        }
    }
}