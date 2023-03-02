using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeGameboardBackground : MonoBehaviour
{
    private void Awake()
    {
        ThemeVisitor.Visit(this);
    }
}
