using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class screenshot : MonoBehaviour
{
    private Camera pokeCamera;
    public GameObject screen;
    public RawImage imagePreview;
    public scrDetector analizar;
    private bool takePhoto;

    private void Start()
    {
        pokeCamera = this.GetComponent<Camera>();
        takePhoto = true;
        
    }
    Texture2D RTImage(Camera camera)
    {
        RenderTexture.active = camera.targetTexture;
        camera.Render();
        Texture2D image = new Texture2D(camera.targetTexture.width, camera.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
        image.Apply();
        return image;

    }

    
    private void Update()
    {

        if (OVRInput.GetDown(OVRInput.Button.SecondaryThumbstick))
        {
            if (takePhoto)
            {
                imagePreview.texture = RTImage(pokeCamera);
                screen.SetActive(false);
                analizar.Analize();
                takePhoto = false;
            }
            
        }
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {

            screen.SetActive(true);
            takePhoto = true;
        }
    }
}
