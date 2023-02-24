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

    private void Awake() 
    {
        ResourceLocator.AddResource("BallCounter", this);    
    }
}
