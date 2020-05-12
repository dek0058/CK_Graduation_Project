// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using UnityEditor;
using UnityEngine;

namespace Rotorz.Tile.Editor
{
    /// <summary>
    /// Default inspector for brush assets.
    /// </summary>
    [CustomEditor(typeof(OrientedBrush), true)]
    public class OrientedBrushEditor : UnityEditor.Editor
    {
        /// <summary> 
        /// Indicates whether editor has been initialized.
        /// </summary>
        private bool hasInitialized;
        /// <summary>
        /// Indicates whether brush asset is accessible via brush database.
        /// </summary>
        private bool hasRecord;

        protected override void OnHeaderGUI()
        {
            
        }

        public override void OnInspectorGUI()
        {
            GUILayout.Label(TileLang.Text("Please use designer to edit brush."));
        }

        
        public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
        {
            /*
            var brush = target as Brush;

            var brushDescriptor = BrushUtility.GetDescriptor(brush.GetType());
            if (brushDescriptor != null && brushDescriptor.CanHavePreviewCache(brush)) {
                return BrushUtility.CreateBrushPreview(target as Brush, width, height);
            }
            else {
                return null;
            }

            return null;
            */
            
            try
            {
                var brush = target as Brush;
                //BrushUtility.CreateBrushPreview
                var brushDescriptor = BrushUtility.GetDescriptor(brush.GetType());
                if (brushDescriptor != null && brushDescriptor.CanHavePreviewCache(brush))
                {
                    var orientatedBrush = target as OrientedBrush;
                    if (orientatedBrush != null)
                    {
                        var ori = orientatedBrush.DefaultOrientation;
                        if (ori != null)
                        {
                            if (ori.VariationCount > 0)
                            {
                                var obj = ori.GetVariation(0);
                                if (obj != null)
                                {
                                    return GetPreviewTexture(obj as GameObject, width, height);

                                    //if (!AssetPreview.IsLoadingAssetPreviews())
                                    //{
                                    //    GameObject go = obj as GameObject;
                                    //    Texture2D texture2D  = AssetPreview.GetAssetPreview(go);

                                    //    if (texture2D != null)
                                    //    {
                                    //        var duplicatedTexture = Instantiate(texture2D);
                                    //        if (duplicatedTexture != null)
                                    //            return duplicatedTexture;
                                    //    }
                                    //}
                                }
                            }
                        }
                    }
                } 
            }
            finally
            {

            }
            return null;
            
        }

        private PreviewRenderUtility renderUtility;
        private Bounds bound = new Bounds();
        Vector2 rotation = new Vector2(30.0f, -20.0f);
        float rangeFactor = 3.8f;

        void InitializePreviewRenderUtility()
        {
            renderUtility = new PreviewRenderUtility();

            renderUtility.ambientColor = Color.white;
            renderUtility.cameraFieldOfView = 30;
            renderUtility.camera.clearFlags = CameraClearFlags.SolidColor;
            renderUtility.camera.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1.0f);
            renderUtility.lights[0].intensity = 2.0f;
        }

        Texture2D GetPreviewTexture(GameObject go, int width, int height)
        {
            if (renderUtility == null)
                InitializePreviewRenderUtility();

            var rect = new Rect(0, 0, width, height);

            renderUtility.BeginStaticPreview(rect);

            MeshFilter[] meshFilters = go.GetComponentsInChildren<MeshFilter>();
            MeshRenderer[] meshRenderers = go.GetComponentsInChildren<MeshRenderer>();

            for (int i=0; i < meshRenderers.Length; i ++)
            {
                bound.Encapsulate(meshRenderers[i].bounds);
                renderUtility.DrawMesh(meshFilters[i].sharedMesh, Matrix4x4.identity, meshRenderers[i].sharedMaterials[0], 0);
            }

            float magnitude = bound.extents.magnitude;
            float num = magnitude * rangeFactor;
            var quaternion = Quaternion.Euler(-rotation.y, -rotation.x, 0f);
            var position = bound.center - quaternion * (Vector3.forward * num);

            renderUtility.camera.transform.position = position;
            renderUtility.camera.transform.rotation = quaternion;
            renderUtility.camera.nearClipPlane = num - magnitude * 1.1f;
            renderUtility.camera.farClipPlane = num + magnitude * 1.1f;

            //renderUtility.lights[0].transform.rotation = Quaternion.Euler(40f, 40f, 0f);
            renderUtility.lights[0].transform.rotation = renderUtility.camera.transform.rotation;
            
            //renderUtility.lights[1].intensity = 2.0f;

            renderUtility.Render(true);

            Texture2D result = renderUtility.EndStaticPreview();
            renderUtility.Cleanup();

            return result;
        }

    }
}
