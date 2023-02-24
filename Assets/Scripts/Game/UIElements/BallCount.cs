using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BallCount : MonoBehaviour
{
    private TextMeshPro TextMesh { get; set; }
    
    private int _returnedBallCount = 0;
    public int ReturnedBallCount { get { return _returnedBallCount; } set {  } }

    private Color32 _originalColor = new Color32(0xff, 0xff, 0xff, 0xff); // white

    private void Awake()
    {
        TextMesh = GetComponent<TextMeshPro>();
    }

    public void SetNumber(int number)
    {
        // if 0 then hide text
        if (number == 0)
        {
            _originalColor = TextMesh.color;
            TextMesh.color = new Color32(0xff, 0xff, 0xff, 0x00); // transparent
        }
        else
            TextMesh.color = _originalColor;
        
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
