
using UnityEngine;

public interface IStartFire
{
    bool StartFire();
}

public interface IGetMousePosition
{
    Vector3 GetMousePosition();
}

public interface IStartAim
{
    bool StartAim();
}

public interface IEndAim
{
    bool EndAim();
}

public interface IReturnFire
{
    bool ReturnFire();
}

public interface IStartMove
{
    bool StartMove();
}

public interface IEndMove
{
    bool EndMove();
}

public interface IGetMovePosition
{
    Vector2 GetMovePosition();
}

public interface IGetFireDirection
{
    Vector2 GetFireDirection();
}
