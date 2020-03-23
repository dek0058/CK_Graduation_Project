using UnityEngine;

public class TestGrayCam : JToolkit.Utility.Singleton<TestGrayCam>
{
    [HideInInspector] public RenderTexture render;
    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
            Destroy(this);

        if (cam.targetTexture != null)
            cam.targetTexture.Release();

        render = new RenderTexture(Screen.width, Screen.height, 24);
        cam.targetTexture = render;
    }
}
