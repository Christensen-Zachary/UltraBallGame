using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIInput : MonoBehaviour, IStartMove, IEndMove, IReturnFire
{
    [field: SerializeField]
    private ResourceLocator ResourceLocator { get; set; }

    private bool _startMove = false;
    private int _startMoveCounter = 0;

    private bool _endMove = false;
    private int _endMoveCounter = 0;

    private bool _returnFire = false;
    private int _returnFireCounter = 0;
    public bool StartMove()
    {
        return _startMove;   
    }

    public bool EndMove()
    {
        return _endMove;
    }

    private void Awake()
    {
        ResourceLocator.AddResource("GameUIInput", this);
    }
    
    

    public void DoStartMove()
    {
        _startMove = true;
    }

    public void DoEndMove()
    {
        _endMove = true;
    }

    public void DoReturnFire()
    {
        _returnFire = true;
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

        if (_endMove)
        {
            if (_endMoveCounter++ > 1)
            {
                _endMoveCounter = 0;
                _endMove = false;
            }
        }

        if (_returnFire)
        {
            if (_returnFireCounter++ > 1)
            {
                _returnFireCounter = 0;
                _returnFire = false;
            }
        }
    }

    public bool ReturnFire()
    {
        return _returnFire;
    }
}
