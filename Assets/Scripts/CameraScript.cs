using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Material PostProcessingMaterial;
    public static CameraScript _instance;
    public bool activateFocusEffect;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (activateFocusEffect)
        {
            Graphics.Blit(source, destination, PostProcessingMaterial);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}
