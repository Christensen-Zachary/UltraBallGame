using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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

    public static void SetFrameRate()
    {
        Application.targetFrameRate = Mathf.Clamp(30, 60, PlayerPrefs.GetInt("FPS", 60));
    }

    public static void SetFrameRate(int frameRate)
    {
        PlayerPrefs.SetInt("FPS", Mathf.Clamp(30, 60, frameRate));
        SetFrameRate();
    }
}
