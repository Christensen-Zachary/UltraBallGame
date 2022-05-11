using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BrickNumber : MonoBehaviour
{
    private TextMeshPro TextMesh { get; set; }


    private void Awake()
    {
        TextMesh = GetComponent<TextMeshPro>();
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

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
