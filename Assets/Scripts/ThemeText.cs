using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ThemeText : MonoBehaviour
{
    private TextMeshProUGUI _textMeshProUGUI;
    private TextMeshPro _textMeshPro;
    private bool _useOther = false;
    private void Awake()
    {
        _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        if (_textMeshProUGUI == null)
        {
            _textMeshPro = GetComponent<TextMeshPro>();
            _useOther = true;
        }

        ThemeVisitor.Visit(this);
    }

    public void SetFont(TMP_FontAsset font)
    {
        if (_useOther)
        {
            _textMeshPro.font = font;
        }
        else
        {
            _textMeshProUGUI.font = font;
        }
    }

    public void SetFontColor(Color color)
    {
        if (_useOther)
        {
            _textMeshPro.color = color;
        }
        else
        {
            _textMeshProUGUI.color = color;
        }
    }
}
