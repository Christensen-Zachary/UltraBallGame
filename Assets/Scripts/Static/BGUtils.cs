using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BGUtils
{
    public static (float height, float width) GetScreenSize()
    {
        float height = Camera.main.orthographicSize * 2f;
        float width = height * Camera.main.aspect; // aspect = width/height
        return (height, width);
    }

    public static float CosineFunction(float input)
    {
        return 0.5f * Mathf.Cos(Mathf.PI * input + Mathf.PI) + 0.5f;
    }
}
