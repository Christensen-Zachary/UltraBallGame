
using UnityEngine;

public interface ITouchingGameboard
{
    bool TouchingGameboard();
}

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

public interface IResetGame
{
    bool ResetGame();
}

public interface INextLevel
{
    bool NextLevel();
}

public interface IOpenMainMenuPanel
{
    bool OpenMainMenuPanel();
}

public interface ICloseMainMenuPanel
{
    bool CloseMainMenuPanel();
}

public interface IOpenMainMenu
{
    bool OpenMainMenu();
}

public interface IOpenOptions
{
    bool OpenOptions();
}

public interface ICloseOptionsPanel
{
    bool CloseOptionsPanel();
}

public interface IStartSliderAim
{
    bool StartSliderAim();
}

public interface IEndSliderAim
{
    bool EndSliderAim();
}

public interface IStartFireUI
{
    bool StartFire();
}

public interface IGiveExtraBalls
{
    bool GiveExtraBalls();
}

public interface IGiveFloorBricks
{
    bool GiveFloorBricks();
}

public interface ISetBallsOnFire
{
    bool SetBallsOnFire();
}


