using UnityEngine;
using UnityEngine.UI;

public class TestGrayImage : MonoBehaviour
{
    public GameObject start_obj;
    private RawImage image;

    private void Start()
    {
        image = GetComponent<RawImage>();
        if (image == null)
            Destroy(this);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (image.texture == null)
                image.texture = TestGrayCam.instance.render;

            if (start_obj != null)
                start_obj.SetActive(!start_obj.activeSelf);

            image.enabled = !image.enabled;
        }
    }
}