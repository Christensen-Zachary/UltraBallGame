using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FireBallsText : MonoBehaviour
{
    private TextMeshProUGUI TextMesh { get; set; }

    private void Awake()
    {
        TextMesh = GetComponent<TextMeshProUGUI>();
    }

    public void SetNumber(int number)
    {
        TextMesh.text = "x" + number.ToString();
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
