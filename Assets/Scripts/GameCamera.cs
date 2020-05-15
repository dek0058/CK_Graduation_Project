using UnityEngine;
using Cinemachine;
using UnityEngine.Experimental.Rendering.Universal;

[System.Serializable]
public class GameCamera {

    // 카메라 스태킹
    public Camera ui_camera;

    public CameraPerspectiveEditor cpe;
    public PixelPerfectCamera ppc;

    public CinemachineBrain cv_brain;
    public CinemachineVirtualCamera cv_camera;


    public void confirm ( ) {
        ui_camera = GameObject.FindGameObjectWithTag ( "UICamera" ).GetComponent<Camera> ( );

        cpe = Camera.main.GetComponent<CameraPerspectiveEditor> ( );
        ppc = Camera.main.GetComponent<PixelPerfectCamera> ( );
        cv_brain = Camera.main.GetComponent<CinemachineBrain> ( );
    }
}

