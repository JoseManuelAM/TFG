using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAspectRatio : MonoBehaviour
{
    public Vector2 targetAspects = new Vector2(16f, 15f);

    private void Awake()
    {
        float targetAspect = targetAspects.x / targetAspects.y;
        float screenAspect = (float)Screen.width / (float)Screen.height;

        float scaleHeight = screenAspect / targetAspect;
        float scaleWidth = 1f / scaleHeight;

        Camera cam = GetComponent<Camera>();

        Rect rect = cam.rect;
        rect.width = scaleWidth;
        rect.height = 1.0f;
        rect.x = (1.0f - scaleWidth) / 2.0f;
        rect.y = 0f;
        cam.rect = rect;
    }
}
