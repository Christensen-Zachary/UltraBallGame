using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BtnLoadLevel : MonoBehaviour
{
    [field: SerializeField]
    private TextMeshProUGUI TextMeshPro { get; set; }


    public void SetText(string str)
    {
        TextMeshPro.text = str;
    }
}
