using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeButtonImage : MonoBehaviour
{
    private void Awake()
    {
        ThemeVisitor.Visit(this);
    }
}
