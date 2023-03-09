using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCounter : MonoBehaviour
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }

    private int _ballCounter = 0;
    public int Count { get { return _ballCounter; } set { if (_ballCount != null) { _ballCount.SetNumber(value); } _ballCounter = value; } }
    public BallCount _ballCount;
    private int _extraCount = 0;
    public int ExtraCount { get { return _extraCount; } set { if (_ballCount != null) { _ballCount.SetExtraNumber(value); } _extraCount = value; } }

    private void Awake() 
    {
        ResourceLocator.AddResource("BallCounter", this);    
    }

    public void Subtract(int number)
    {
        if (ExtraCount > 0)
        {
            ExtraCount--;
        }
        else
            Count--;
    }
}
