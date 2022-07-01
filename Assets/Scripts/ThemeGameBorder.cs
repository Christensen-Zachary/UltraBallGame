using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeGameBorder : MonoBehaviour
{
    private void Awake()
    {
        ThemeVisitor.Visit(this);
    }
}
