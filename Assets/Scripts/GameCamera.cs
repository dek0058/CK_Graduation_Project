using UnityEngine;
using Cinemachine;
using UnityEngine.Experimental.Rendering.Universal;

[System.Serializable]
public class GameCamera {

    public CameraPerspectiveEditor cpe;
    public PixelPerfectCamera ppc;

    public CinemachineBrain cv_brain;
    public CinemachineVirtualCamera cv_camera;


    public void confirm ( ) {
        cpe = Camera.main.GetComponent<CameraPerspectiveEditor> ( );
        ppc = Camera.main.GetComponent<PixelPerfectCamera> ( );
        cv_brain = Camera.main.GetComponent<CinemachineBrain> ( );
    }
}

