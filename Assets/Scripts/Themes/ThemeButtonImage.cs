using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeButtonImage : MonoBehaviour
{

    public ThemeButtonSize _themeButtonSize = ThemeButtonSize.Normal;

    private void Awake()
    {
        ThemeVisitor.Visit(this, _themeButtonSize);
    }
}
