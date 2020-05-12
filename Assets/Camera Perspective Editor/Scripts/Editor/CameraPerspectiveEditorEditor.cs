using UnityEditor;
using UnityEngine;
using System.Linq;

[CustomEditor(typeof(CameraPerspectiveEditor))]
[CanEditMultipleObjects]
public class CameraPerspectiveEditorEditor : Editor
{
    private static bool showLensShift = true;
    private static bool showLensTilt = false;
    private static bool showPositionShift = false;
    private static bool showSkew = false;
    private static bool showAspectScale = false;
    private static bool showClippingPlaneSkew = false;
    private static bool showDollyZoom = false;

    private CameraPerspectiveEditor cameraOffsetController;
    private Camera thisCamera;

    public SerializedProperty lensShift;
    public SerializedProperty lensShiftProportionalToAspect;
    public SerializedProperty lensTilt;
    public SerializedProperty positionShift;
    public SerializedProperty skew;
    public SerializedProperty aspectScale;
    public SerializedProperty clippingPlaneSkew;
    public SerializedProperty dollyZoom;
    public SerializedProperty dollyZoomFocalDistance;
    public SerializedProperty dollyZoomFocalTarget;

    private Vector2 utilityVector2;
    private bool showLensShiftHandles = true;

    private void OnEnable()
    {
        lensShift = serializedObject.FindProperty("lensShift");
        lensShiftProportionalToAspect = serializedObject.FindProperty("lensShiftProportionalToAspect");
        lensTilt = serializedObject.FindProperty("lensTilt");
        positionShift = serializedObject.FindProperty("positionShift");
        skew = serializedObject.FindProperty("skew");
        aspectScale = serializedObject.FindProperty("aspectScale");
        clippingPlaneSkew = serializedObject.FindProperty("clippingPlaneSkew");
        dollyZoom = serializedObject.FindProperty("dollyZoom");
        dollyZoomFocalDistance = serializedObject.FindProperty("dollyZoomFocalDistance");
        dollyZoomFocalTarget = serializedObject.FindProperty("dollyZoomFocalTarget");

        cameraOffsetController = target as CameraPerspectiveEditor;
        thisCamera = cameraOffsetController.GetComponent<Camera>();
        SceneView.duringSceneGui += DuringSceneGui;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= DuringSceneGui;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();
        GUILayout.BeginVertical();
        {

            if (showLensShift = EditorGUILayout.Foldout(showLensShift, "Lens Shift", toggleOnLabelClick: true))
            {
                EditorGUI.indentLevel++;

                EditorGUILayout.HelpBox("Similar to \"Shift\" on a Tilt-Shift Camera lens.  Commonly used for oblique perspectives/projections, including 2.5D effects.", MessageType.Info);

                utilityVector2.x = EditorGUILayout.FloatField("Horizontal", lensShift.vector2Value.x);
                utilityVector2.y = EditorGUILayout.FloatField("Vertical", lensShift.vector2Value.y);
                lensShift.vector2Value = utilityVector2;

                lensShiftProportionalToAspect.boolValue = EditorGUILayout.Toggle("Proportional To Aspect", lensShiftProportionalToAspect.boolValue);

                showLensShiftHandles = EditorGUILayout.Toggle("Show Handles", showLensShiftHandles);

                GUILayout.Space(15);

                EditorGUI.indentLevel--;
            }

            if (showLensTilt = EditorGUILayout.Foldout(showLensTilt, "Lens Tilt", toggleOnLabelClick: true))
            {
                EditorGUI.indentLevel++;

                EditorGUILayout.HelpBox("Similar to \"Tilt\" on a Tilt-Shift Camera lens.", MessageType.Info);
                utilityVector2.x = EditorGUILayout.FloatField("Horizontal", lensTilt.vector2Value.x);
                utilityVector2.y = EditorGUILayout.FloatField("Vertical", lensTilt.vector2Value.y);
                lensTilt.vector2Value = utilityVector2;
                GUILayout.Space(15);

                EditorGUI.indentLevel--;
            }

            if (showPositionShift = EditorGUILayout.Foldout(showPositionShift, "Position Shift", toggleOnLabelClick: true))
            {
                EditorGUI.indentLevel++;

                EditorGUILayout.HelpBox("Similar to Truck(horizontal) and Pedestal(vertical) Camera motion, but without actually moving the Transform.", MessageType.Info);
                utilityVector2.x = EditorGUILayout.FloatField("Horizontal (world-units)", positionShift.vector2Value.x);
                utilityVector2.y = EditorGUILayout.FloatField("Vertical (world-units)", positionShift.vector2Value.y);
                positionShift.vector2Value = utilityVector2;

                GUILayout.Space(15);

                EditorGUI.indentLevel--;
            }

            if (showSkew = EditorGUILayout.Foldout(showSkew, "Skew", toggleOnLabelClick: true))
            {
                EditorGUI.indentLevel++;

                utilityVector2.x = EditorGUILayout.FloatField("Horizontal", skew.vector2Value.x);
                utilityVector2.y = EditorGUILayout.FloatField("Vertical", skew.vector2Value.y);
                skew.vector2Value = utilityVector2;
                GUILayout.Space(15);

                EditorGUI.indentLevel--;
            }

            if (showAspectScale = EditorGUILayout.Foldout(showAspectScale, "Aspect Scale", toggleOnLabelClick: true))
            {
                EditorGUI.indentLevel++;

                EditorGUILayout.HelpBox("Adjust per-axis foreshortening - like zooming only on the vertical or horizontal.", MessageType.Info);
                utilityVector2.x = EditorGUILayout.FloatField("Horizontal", aspectScale.vector2Value.x);
                utilityVector2.y = EditorGUILayout.FloatField("Vertical", aspectScale.vector2Value.y);
                aspectScale.vector2Value = utilityVector2;
                GUILayout.Space(15);

                EditorGUI.indentLevel--;
            }

            if (showClippingPlaneSkew = EditorGUILayout.Foldout(showClippingPlaneSkew, "Clipping Plane Skew", toggleOnLabelClick: true))
            {
                EditorGUI.indentLevel++;

                EditorGUILayout.HelpBox("Skews the side-planes of the view frustum in the depth(z) direction.", MessageType.Info);
                utilityVector2.x = EditorGUILayout.FloatField("Horizontal", clippingPlaneSkew.vector2Value.x);
                utilityVector2.y = EditorGUILayout.FloatField("Vertical", clippingPlaneSkew.vector2Value.y);
                clippingPlaneSkew.vector2Value = utilityVector2;
                GUILayout.Space(15);

                EditorGUI.indentLevel--;
            }

            EditorGUI.BeginDisabledGroup(thisCamera.orthographic);
            if (showDollyZoom = EditorGUILayout.Foldout(showDollyZoom, "Dolly Zoom", toggleOnLabelClick: true))
            {
                EditorGUI.indentLevel++;

                if (thisCamera.orthographic)
                {
                    EditorGUILayout.HelpBox("Not Available With Orthographic Cameras", MessageType.Warning);
                }
                EditorGUILayout.HelpBox("Simulates a \"Dolly Zoom\", or \"Trombone\" effect without actually moving or zooming the camera.\n\"Focal Distance\" is the distance at which objects will maintain their screen-relative size.\nIf \"Focal Target\" is set, it will automatically define \"Focal Distance\" based on the target's distance.", MessageType.Info);
                EditorGUILayout.Slider(dollyZoom, 0f, 1f, "Dolly Zoom");
                EditorGUI.BeginDisabledGroup(dollyZoomFocalTarget.objectReferenceValue);
                dollyZoomFocalDistance.floatValue = EditorGUILayout.FloatField("Focal Distance", dollyZoomFocalDistance.floatValue);
                EditorGUI.EndDisabledGroup();
                dollyZoomFocalTarget.objectReferenceValue = EditorGUILayout.ObjectField("Focal Target", dollyZoomFocalTarget.objectReferenceValue, typeof(Transform), true);

                EditorGUI.indentLevel--;
            }
            EditorGUI.EndDisabledGroup();

        }
        GUILayout.EndVertical();

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
            SceneView.RepaintAll();
        }
    }

    private void DuringSceneGui(SceneView sceneView)
    {
        if (thisCamera.enabled && cameraOffsetController.enabled)
        {
            thisCamera.ResetProjectionMatrix();
            var unmodifiedViewportToWorldMatrix = (thisCamera.projectionMatrix * thisCamera.worldToCameraMatrix).inverse;

            // Draw unmodified frustum (red)
            DrawFrustum(unmodifiedViewportToWorldMatrix, new Color(1f, 0f, 0f, 0.65f));

            if (showLensShiftHandles)
            {
                // Draw interactive handles
                DrawInteractiveHandles(unmodifiedViewportToWorldMatrix);
            }

            // Update the Camera's projection matrix with the new values via the target CPE component
            cameraOffsetController.OnPreCull();

            // Draw modified frustum (yellow)
            var modifiedViewportToWorldMatrix = (thisCamera.projectionMatrix * thisCamera.worldToCameraMatrix).inverse;
            DrawFrustum(modifiedViewportToWorldMatrix, new Color(1f, 1f, 0f, 0.65f));
        }
    }

    private void DrawFrustum(Matrix4x4 viewportToWorldMatrix, Color color)
    {
        Vector3 bl = Vector3.zero;
        Vector3 tl = Vector3.zero;
        Vector3 br = Vector3.zero;
        Vector3 tr = Vector3.zero;

        tr.x = tr.y = br.x = tl.y = 1f;
        bl.x = bl.y = tl.x = br.y = -1f;
        bl.z = tl.z = br.z = tr.z = 1f;

        var bl1 = viewportToWorldMatrix.MultiplyPoint(-tr);
        var tl1 = viewportToWorldMatrix.MultiplyPoint(-br);
        var br1 = viewportToWorldMatrix.MultiplyPoint(-tl);
        var tr1 = viewportToWorldMatrix.MultiplyPoint(-bl);
        var bl2 = viewportToWorldMatrix.MultiplyPoint(bl);
        var tl2 = viewportToWorldMatrix.MultiplyPoint(tl);
        var br2 = viewportToWorldMatrix.MultiplyPoint(br);
        var tr2 = viewportToWorldMatrix.MultiplyPoint(tr);

        Handles.color = color;

        Handles.DrawPolyLine(new Vector3[] {
            bl1,
            tl1,
            tr1,
            br1,
            bl1,
            bl2,
            tl2,
            tr2,
            br2,
            bl2
        });
        Handles.DrawLine(tl1, tl2);
        Handles.DrawLine(tr1, tr2);
        Handles.DrawLine(br1, br2);
    }

    private void DrawInteractiveHandles(Matrix4x4 unmodifiedViewportToWorldMatrix)
    {
        var lensShiftValue = cameraOffsetController.lensShift * 2f;
        if (!cameraOffsetController.lensShiftProportionalToAspect)
        {
            lensShiftValue.x /= thisCamera.aspect;
        }

        var lensShift2DWorldPosition = unmodifiedViewportToWorldMatrix.MultiplyPoint(new Vector3(lensShiftValue.x, lensShiftValue.y, 1f));
        var handleSize = HandleUtility.GetHandleSize(lensShift2DWorldPosition) * 0.075f;
        var worldSpaceRight = unmodifiedViewportToWorldMatrix.MultiplyVector(Vector3.right).normalized;
        var worldSpaceUp = unmodifiedViewportToWorldMatrix.MultiplyVector(Vector3.up).normalized;

        Debug.DrawLine(Vector3.zero, worldSpaceRight, Color.red);
        Debug.DrawLine(Vector3.zero, worldSpaceUp, Color.green);

        EditorGUI.BeginChangeCheck();
        var newWorldSpaceValue = Handles.Slider2D(lensShift2DWorldPosition, -thisCamera.transform.forward, worldSpaceRight, worldSpaceUp, handleSize, Handles.DotHandleCap, 0f);
        
        var newLensShiftValue = unmodifiedViewportToWorldMatrix.inverse.MultiplyPoint(newWorldSpaceValue);
        if (!cameraOffsetController.lensShiftProportionalToAspect)
        {
            newLensShiftValue.x *= thisCamera.aspect;
        }

        if (EditorGUI.EndChangeCheck())
        {
            cameraOffsetController.lensShift = new Vector2(newLensShiftValue.x, newLensShiftValue.y) / 2f;
        }
    }
}