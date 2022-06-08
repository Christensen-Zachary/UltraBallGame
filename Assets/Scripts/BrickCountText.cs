using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BrickCountText : MonoBehaviour
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }

    [field: SerializeField]
    public TextMeshProUGUI TextMeshPro { get; set; }

    private int _maxBricks = 0;
    public int MaxBricks { get { return _maxBricks; } set { _maxBricks = value; TextMeshPro.text = $"{CurrentBricks}/{value}"; } }
    private int _currentBricks = 0;
    public int CurrentBricks { get { return _currentBricks; } set { _currentBricks = value; TextMeshPro.text = $"{value}/{MaxBricks}"; } }


    private void Awake()
    {
        ResourceLocator.AddResource("BrickCountText", this);
    }

}
