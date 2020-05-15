using UnityEngine;
using Game.Management;

[ExecuteInEditMode]
public class ObjectHeightHelper : MonoBehaviour {

    public Transform[] scaling;
    public Transform[] positioning;


    private void Update ( ) {
        float y_axis = GameManager.instance.game_camera.cpe.lensShift.y;
        float far = Camera.main.farClipPlane;

        foreach ( var obj in positioning ) {
            obj.localPosition = new Vector3 ( 0f, -(y_axis / far) * 4, 0f );
        }

        foreach ( var obj in scaling ) {
            obj.localScale = new Vector3 ( 1f, 1f - (y_axis / far), 1f );
        }
    }
}
