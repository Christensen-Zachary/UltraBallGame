using System;
using UnityEngine;

public class EmptyInput : MonoBehaviour, ITouchingGameboard, IStartFire, IGetFireDirection, IGetMousePosition, IStartAim, IEndAim, IReturnFire, IStartMove, IEndMove, IGetMovePosition
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }

    private void Awake()
    {
        ResourceLocator.AddResource("EmptyInput", this);
    }

    public Vector2 GetMovePosition()
    {
        return Vector2.zero;
    }

    public bool EndMove()
    {
        return true;
    }

    public bool StartMove()
    {
        return false;
    }

    public bool ReturnFire()
    {
        return false;
    }

    public bool EndAim()
    {
        return true;
    }

    public bool StartAim()
    {
        return false;
    }

    public Vector3 GetMousePosition()
    {
        return Vector2.zero;
    }

    public bool StartFire()
    {
        return false;
    }

    public Vector2 GetFireDirection()
    {
        return Vector2.zero;
    }

    public bool TouchingGameboard()
    {
        return false;
    }


}
