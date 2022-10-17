using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BrickNumber : MonoBehaviour
{
    public TextMeshPro TextMesh { get; private set; }


    private void Awake()
    {
        TextMesh = GetComponent<TextMeshPro>();
        
        ThemeVisitor.Visit(this);
    }

    public void SetNumber(int number)
    {
        TextMesh.text = number.ToString();
    }

    public void Subtract(int number)
    {
        try
        {
            int current = System.Convert.ToInt32(TextMesh.text);
            SetNumber(current - number);
        }
        catch
        {

            return;
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
