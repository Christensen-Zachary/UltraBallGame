using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BrickCountText : MonoBehaviour
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }

    [field: SerializeField]
    public TextMeshProUGUI TextMeshPro { get; set; }

    private int _maxBricks = 0;
    public int MaxBricks { get { return _maxBricks; } set { _maxBricks = value; TextMeshPro.text = $"{value}"; } }
    private int _currentBricks = 0;
    public int CurrentBricks { get { return _currentBricks; } set { _currentBricks = value; TextMeshPro.text = $"{MaxBricks - value}"; _progressImage.fillAmount = (float)value / MaxBricks; } }

    public Image _progressImage; // reference set in editor

    private void Awake()
    {
        ResourceLocator.AddResource("BrickCountText", this);
    }

}
