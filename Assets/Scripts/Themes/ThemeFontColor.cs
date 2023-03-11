using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ThemeFontColor : MonoBehaviour
{
    public TextMeshProUGUI TextMesh { get; set; }
    public TextMeshPro TextMeshOnGameObjects { get; set; }


    private void Awake()
    {
        TextMesh = GetComponent<TextMeshProUGUI>();
        TextMeshOnGameObjects = GetComponent<TextMeshPro>();

        ThemeVisitor.Visit(this);
    }
}
