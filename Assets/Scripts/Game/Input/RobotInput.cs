using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotInput : MonoBehaviour, IStartFire, IGetFireDirection, IGetMousePosition, IStartAim, IEndAim, IReturnFire, IStartMove, IEndMove, IGetMovePosition, IResetGame
{

    [field: SerializeField]
    private ResourceLocator ResourceLocator { get; set; }


    private void Awake()
    {
        ResourceLocator.AddResource("RobotInput", this);
    }

    public bool EndAim()
    {
        return false;
    }

    public bool EndMove()
    {
        return true;
    }

    public Vector2 GetFireDirection()
    {
        float x = Random.Range(-1f, 1f);
        float y = Mathf.Abs(Random.Range(0.1f, 1));
        return new Vector2(x, y);
    }

    public Vector3 GetMousePosition()
    {
        throw new System.NotImplementedException();
    }

    public Vector2 GetMovePosition()
    {
        return Vector2.zero;
    }

    public bool ReturnFire()
    {
        return false;
    }

    public bool StartAim()
    {
        return true;
    }

    public bool StartFire()
    {
        return true;
    }

    public bool StartMove()
    {
        return false;
    }

    public bool ResetGame()
    {
        return true;
    }
}
