using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleHDR : MonoBehaviour
{
    public static readonly string HDR_ENABLED_KEY = "HDREnabled";

    private void Awake() 
    {
        Camera.main.allowHDR = PlayerPrefs.GetInt(HDR_ENABLED_KEY, 1) == 1; // true when enabled
    }
}
