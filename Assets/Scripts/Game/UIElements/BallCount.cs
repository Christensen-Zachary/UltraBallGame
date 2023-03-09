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
        
        if (TextMesh.text.Split("+").Length == 1)
            TextMesh.text = number.ToString();
        else
            TextMesh.text = number.ToString() + "+" + TextMesh.text.Split("+")[1];
    }

    public void SetExtraNumber(int number)
    {
        if (number == 0)
        {
            TextMesh.text = TextMesh.text.Split("+")[0];
        }
        else if (TextMesh.text.Split("+").Length == 1)
        {
            TextMesh.text = TextMesh.text + "+" + number.ToString();
        }
        else if (TextMesh.text.Split("+").Length == 2)
        {
            if (number != 0)
            {
                TextMesh.text = TextMesh.text.Split("+")[0] + "+" + number.ToString();
            }
            else
                TextMesh.text = TextMesh.text.Split("+")[0];
        }
    }

    public void Subtract(int number)
    {
        try
        {

            if (TextMesh.text.Split("+").Length == 1)
            {
                int current = System.Convert.ToInt32(TextMesh.text.Split("+")[0]);
                SetNumber(current - number); 
            }
            else
            {
                int current = System.Convert.ToInt32(TextMesh.text.Split("+")[1]);
                SetExtraNumber(current - number);
            }
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
