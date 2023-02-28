using System.Collections;
using System.Collections.Generic;
using FastMobileBlurURP2023;
using UnityEngine;

public class UnblurOnAwake : MonoBehaviour
{
    private void Awake() 
    {
        BlurURP.Settings.IsActive = false;    
    }
}
