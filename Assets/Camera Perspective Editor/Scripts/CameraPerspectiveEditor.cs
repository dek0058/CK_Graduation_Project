using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class CameraPerspectiveEditor : MonoBehaviour
{
    #region Exposed_Members
    // Lens Shift (Far-Plane Offset)
    public Vector2 lensShift = Vector2.zero;
    public bool lensShiftProportionalToAspect = false;

    // Lens Tilt
    public Vector2 lensTilt = Vector2.zero;

    // Position Shift
    public Vector2 positionShift = Vector2.zero;

    // Skew
    public Vector2 skew = Vector2.zero;

    // Aspect Scale
    public Vector2 aspectScale = Vector2.one;

    // Clipping Plane Skew
    public Vector2 clippingPlaneSkew = Vector2.zero;

    // Dolly Zoom (Trombone)
    [Range(0f, 1f)]
    public float dollyZoom = 0f;
    public float dollyZoomFocalDistance = -1f;
    public Transform dollyZoomFocalTarget;
    #endregion Exposed_Members

    #region Private_Members
    private Matrix4x4 customMatrix;
    private Vector2 unmodifiedAspectScale;
    private Camera _thisCamera;

    private float currentScreenHeight = 0f;
    private float currentScreenWidth = 0f;
    private float currentFOV;
    private float currentOrthographicSize;
    private float currentNearClipPlane;
    private float currentFarClipPlane;
    private bool currentProjectionMode;
    #endregion Private_Members

    #region Properties
    private Camera thisCamera
    {
        get
        {
            if (!_thisCamera)
            {
                _thisCamera = GetComponent<Camera>();
            }

            return _thisCamera;
        }
    }
    #endregion Properties

    #region Initialization
    // Reset() runs when this script is added or the whole component is reset via the context menu
    public void Reset()
    {
        if (dollyZoomFocalDistance < thisCamera.farClipPlane)
        {
            dollyZoomFocalDistance = (thisCamera.farClipPlane - thisCamera.nearClipPlane) * 0.5f;
        }
    }

    public void OnEnable()
    {
        ResetToUnmodifiedProjectionMatrix();

        RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
    }

    public void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;

        thisCamera.ResetProjectionMatrix();
    }

    private void ResetToUnmodifiedProjectionMatrix()
    {
        thisCamera.ResetProjectionMatrix();
        customMatrix = thisCamera.projectionMatrix;

        unmodifiedAspectScale.x = customMatrix.m00;
        unmodifiedAspectScale.y = customMatrix.m11;
    }
    #endregion Initialization

    #region Apply_Customization
    // Workaround to make sure this all still works with the Standard Unity renderer
    public void OnPreCull()
    {
        OnBeginCameraRendering(new ScriptableRenderContext(), thisCamera);
    }

    // HDRP & URP Pre-Rendering Callbacks
    public void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        // Only run this for the camera that is a sibling to this component
        if (camera != thisCamera)
        {
            return;
        }

        if (CheckForCameraUpdate())
        {
            ResetToUnmodifiedProjectionMatrix();
        }

        #region Matrix_Customization
        // Aspect Scale
        if (camera.orthographic)
        {
            customMatrix.m00 = 1f / camera.orthographicSize / aspectScale.x / camera.aspect;
            customMatrix.m11 = 1f / camera.orthographicSize / aspectScale.y;
        }
        else
        {
            customMatrix.m00 = unmodifiedAspectScale.x * aspectScale.x;
            customMatrix.m11 = unmodifiedAspectScale.y * aspectScale.y;
        }

        // Skew
        if (camera.orthographic)
        {
            customMatrix.m01 = skew.x / camera.orthographicSize * 2f;
            customMatrix.m10 = skew.y / camera.orthographicSize * 2f / camera.aspect;
        }
        else
        {
            customMatrix.m01 = skew.x * 2f;
            customMatrix.m10 = skew.y * 2f;
        }

        // Lens Shift (Far-Plane Offset)
        if (camera.orthographic)
        {
            customMatrix.m02 = lensShift.x / camera.farClipPlane * 2f;
            customMatrix.m12 = lensShift.y / camera.farClipPlane * 2f;
        }
        else
        {
            customMatrix.m02 = lensShift.x * 2f;
            customMatrix.m12 = lensShift.y * 2f;
        }

        if (!lensShiftProportionalToAspect)
        {
            customMatrix.m02 /= camera.aspect;
        }

        // Position Shift
        customMatrix.m03 = positionShift.x * customMatrix.m00 * -1f;
        customMatrix.m13 = positionShift.y * customMatrix.m11 * -1f;

        // Clipping Plane Skew
        if (camera.orthographic)
        {
            customMatrix.m20 = clippingPlaneSkew.x / camera.orthographicSize * 2f / camera.aspect;
            customMatrix.m21 = clippingPlaneSkew.y / camera.orthographicSize * 2f;
        }
        else
        {
            customMatrix.m20 = clippingPlaneSkew.x * 2f / camera.aspect;
            customMatrix.m21 = clippingPlaneSkew.y * 2f;
        }

        // Lens Tilt
        if (camera.orthographic)
        {
            customMatrix.m30 = lensTilt.x;
            customMatrix.m31 = lensTilt.y;
        }
        else
        {
            customMatrix.m30 = lensTilt.x;
            customMatrix.m31 = lensTilt.y;
        }

        // Dolly Zoom (Trombone)
        if (!camera.orthographic)
        {
            dollyZoom = Mathf.Clamp01(dollyZoom);

            if (dollyZoomFocalTarget)
            {
                dollyZoomFocalDistance = camera.WorldToViewportPoint(dollyZoomFocalTarget.position).z;
            }

            dollyZoomFocalDistance = Mathf.Clamp(dollyZoomFocalDistance, currentNearClipPlane, currentFarClipPlane);
            customMatrix.m32 = Mathf.Lerp(-1f, 0f, dollyZoom);
            customMatrix.m33 = Mathf.Lerp(0f, dollyZoomFocalDistance, dollyZoom);
            var dollyZoomDenominatorCache = currentFarClipPlane - currentNearClipPlane;
            var invertedNormalizedDollyZoomFocalDistance = 1f - ((dollyZoomFocalDistance - currentNearClipPlane) / dollyZoomDenominatorCache);
            customMatrix.m22 = ((-(currentNearClipPlane + currentFarClipPlane) / dollyZoomDenominatorCache) - dollyZoom) + (2f * invertedNormalizedDollyZoomFocalDistance * dollyZoom);
            customMatrix.m23 = (((-2f * currentNearClipPlane * currentFarClipPlane) / dollyZoomDenominatorCache) - (currentFarClipPlane * dollyZoom)) + ((currentNearClipPlane + currentFarClipPlane) * invertedNormalizedDollyZoomFocalDistance * dollyZoom);
        }
        #endregion Matrix_Customization

        // Apply Matrix Customization
        camera.projectionMatrix = customMatrix;
    }
    #endregion Apply_Customization

    #region Convert_Camera_To_World_Ray
    public Ray ScreenPointToRay(Vector3 position)
    {
        position.x /= thisCamera.pixelWidth;
        position.y /= thisCamera.pixelHeight;

        return ViewportPointToRay(position);
    }

    public Ray ViewportPointToRay(Vector3 position)
    {
        position.x = (position.x - 0.5f) * 2f;
        position.y = (position.y - 0.5f) * 2f;
        position.z = -1f;

        var viewportToWorldMatrix = (customMatrix * thisCamera.worldToCameraMatrix).inverse;

        var rayOrigin = viewportToWorldMatrix.MultiplyPoint(position);

        return new Ray(rayOrigin, viewportToWorldMatrix.MultiplyVector(Vector3.forward));
    }
    #endregion Raycasting

    #region Convert_World_Point_To_Camera
    public Vector3 WorldToScreenPoint(Vector3 position)
    {
        position = WorldToViewportPoint(position);

        position.x = position.x * thisCamera.pixelWidth;
        position.y = position.y * thisCamera.pixelHeight;

        return position;
    }

    public Vector3 WorldToViewportPoint(Vector3 position)
    {
        var worldToViewportMatrix = customMatrix * thisCamera.worldToCameraMatrix;

        position = worldToViewportMatrix.MultiplyPoint(position);

        position.x = (position.x + 1f) * 0.5f;
        position.y = (position.y + 1f) * 0.5f;
        position.z = 0f;

        return position;
    }
    #endregion Convert_World_Point_To_Camera

    #region Change_Detection
    private bool CheckForCameraUpdate()
    {
        var hasChanged = false;

        // Resolution Changes
        hasChanged |= UpdateFloatAndCheckIfChanged(thisCamera.pixelHeight, ref currentScreenHeight);
        hasChanged |= UpdateFloatAndCheckIfChanged(thisCamera.pixelWidth, ref currentScreenWidth);

        // Field of View Changes
        hasChanged |= UpdateFloatAndCheckIfChanged(thisCamera.fieldOfView, ref currentFOV);

        // Orthographic Size Changes
        hasChanged |= UpdateFloatAndCheckIfChanged(thisCamera.orthographicSize, ref currentOrthographicSize);

        // Handle Clipping Plane Changes
        hasChanged |= UpdateFloatAndCheckIfChanged(thisCamera.nearClipPlane, ref currentNearClipPlane);
        hasChanged |= UpdateFloatAndCheckIfChanged(thisCamera.farClipPlane, ref currentFarClipPlane);

        // Projection Mode Changes
        if (thisCamera.orthographic != currentProjectionMode)
        {
            currentProjectionMode = thisCamera.orthographic;
            hasChanged = true;
        }

        return hasChanged;
    }

    private bool UpdateFloatAndCheckIfChanged(float newValue, ref float oldValue)
    {
        if (newValue != oldValue)
        {
            oldValue = newValue;
            return true;
        }

        return false;
    }
    #endregion Change_Detection
}