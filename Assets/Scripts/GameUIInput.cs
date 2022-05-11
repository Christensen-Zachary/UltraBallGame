using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIInput : MonoBehaviour, IStartMove
{
    [field: SerializeField]
    private ResourceLocator ResourceLocator { get; set; }

    private bool _startMove = false;
    private int _startMoveCounter = 0;

    public bool StartMove()
    {
        return _startMove;   
    }

    private void Awake()
    {
        ResourceLocator.AddResource("GameUIInput", this);
    }
    
    public void DoStartMove()
    {
        _startMove = true;
    }

    private void Update()
    {
        if (_startMove)
        {
            if (_startMoveCounter++ > 1)
            {
                _startMoveCounter = 0;
                _startMove = false;
            }
        }
    }

}
