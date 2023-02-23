using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeDimmer : MonoBehaviour
{

    private void Awake() 
    {
        int hasReturnedFromThemeSwap = PlayerPrefs.GetInt("hasReturnedFromThemeSwap", 0);
        if (hasReturnedFromThemeSwap == 0) // is false
        {
            ThemeVisitor.Visit(this);
        }
        
        // always reset to false
        PlayerPrefs.SetInt("hasReturnedFromThemeSwap", 0);
    }
}
