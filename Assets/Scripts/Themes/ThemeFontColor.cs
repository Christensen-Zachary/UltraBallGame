using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ThemeFontColor : MonoBehaviour
{
    public TextMeshProUGUI TextMesh { get; set; }


    private void Awake()
    {
        TextMesh = GetComponent<TextMeshProUGUI>();

        ThemeVisitor.Visit(this);
    }
}
