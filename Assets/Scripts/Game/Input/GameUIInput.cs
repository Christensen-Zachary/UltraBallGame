using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// separate class from GameUI because implements interfaces, which is no longer true. Now it should be refactored alongside GameUI. Need to fix references in UI components in game scene
public class GameUIInput : MonoBehaviour, IStartMove, IEndMove, IReturnFire, ISetBallsOnFire
{
    [field: SerializeField]
    private ResourceLocator ResourceLocator { get; set; }

    private bool PStartMove { get { if (_startMove) { _startMove = false; return true; } return false; } set { _startMove = value; }}
    private bool _startMove = false;

    private bool PEndMove { get { if (_endMove) { _endMove  = false; return true; } return false; } set { _endMove = value; }}
    private bool _endMove = false;

    private bool PReturnFire { get { if (_returnFire) { _returnFire = false; return true; } return false; } set { _returnFire = value; } }
    private bool _returnFire = false;

    private bool PSetBallsOnFire { get { if (_setBallsOnFire) { _setBallsOnFire = false; return true; } return false; } set { _setBallsOnFire = value; } }
    private bool _setBallsOnFire = false;

    private IEnumerator SetReturnFireRoutine()
    {
        PReturnFire = true;
        yield return null;
        PReturnFire = false;
    }

    private IEnumerator SetStartMoveRoutine()
    {
        PStartMove = true;
        yield return null;
        PStartMove = false;
    }

    private IEnumerator SetEndMoveRoutine()
    {
        PEndMove = true;
        yield return null;
        PEndMove = false;
    }
    
    private IEnumerator SetBallsOnFireRoutine()
    {
        PSetBallsOnFire = true;
        yield return null;
        PSetBallsOnFire = false;
    }

    private void Awake()
    {
        ResourceLocator.AddResource("GameUIInput", this);
    }

    public bool StartMove()
    {
        return PStartMove;
    }

    public bool EndMove()
    {
        return PEndMove;
    }

    public bool ReturnFire()
    {
        return PReturnFire;
    }

    public bool SetBallsOnFire()
    {
        return PSetBallsOnFire;
    }

    public void DoStartMove()
    {
        if (!PStartMove)
        {
            StartCoroutine(SetStartMoveRoutine());
        }
    }

    public void DoEndMove()
    {
        if (!PEndMove)
        {
            StartCoroutine(SetEndMoveRoutine());
        }
    }

    public void DoReturnFire()
    {
        if (!PReturnFire)
        {
            StartCoroutine(SetReturnFireRoutine());
        }
    }

    public void DoSetBallsOnFire()
    {
        if (!PSetBallsOnFire)
        {
            StartCoroutine(SetBallsOnFireRoutine());
        }
    }
}
