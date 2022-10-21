using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputSource
{
    Player,
    PlayerMKB,
    Robot,
    Empty
}

public class GameInput : MonoBehaviour, IStartFire, IGetFireDirection, IGetMousePosition, IStartAim, IEndAim, IReturnFire, IStartMove, IEndMove, IGetMovePosition
{
    [field: SerializeField]
    private ResourceLocator ResourceLocator { get; set; }
    public InputSource _inputSource = InputSource.Player;
    [field: SerializeField]
    public float RobotSpeed { get; set; } = 4;

    private IStartFire GStartFire { get; set; }
    private IGetFireDirection GGetFireDirection { get; set; }
    private IGetMousePosition GGetMousePosition { get; set; }
    private IStartAim GStartAim { get; set; }
    private IEndAim GEndAim { get; set; }
    private IReturnFire GReturnFire { get; set; }
    private IStartMove GStartMove { get; set; }
    private IEndMove GEndMove { get; set; }
    private IGetMovePosition GGetMovePosition { get; set; }

    public void Awake()
    {
        ResourceLocator.AddResource("GameInput", this);

        PlayerInput playerInput = ResourceLocator.GetResource<PlayerInput>("PlayerInput");
        RobotInput robotInput = ResourceLocator.GetResource<RobotInput>("RobotInput");
        GameUIInput gameUIInput = ResourceLocator.GetResource<GameUIInput>("GameUIInput");
        EmptyInput emptyInput = ResourceLocator.GetResource<EmptyInput>("EmptyInput");
        RandomFireDirection randomFireDirection = ResourceLocator.GetResource<RandomFireDirection>("RandomFireDirection");
        switch (_inputSource)
        {
            case InputSource.Player:
                GStartFire = playerInput;
                GGetFireDirection = playerInput;
                GGetMousePosition = playerInput;
                GStartAim = playerInput;
                GEndAim = playerInput;
                GReturnFire = gameUIInput;
                GStartMove = gameUIInput;
                GEndMove = gameUIInput;
                GGetMovePosition = playerInput;
                break;
            case InputSource.PlayerMKB:
                GStartFire = playerInput;
                GGetFireDirection = playerInput;
                GGetMousePosition = playerInput;
                GStartAim = playerInput;
                GEndAim = playerInput;
                GReturnFire = playerInput;
                GStartMove = playerInput;
                GEndMove = playerInput;
                GGetMovePosition = playerInput;
                break;
            case InputSource.Robot:
                GStartFire = robotInput;
                GGetFireDirection = robotInput;
                GGetMousePosition = robotInput;
                GStartAim = robotInput;
                GEndAim = robotInput;
                GReturnFire = playerInput;
                GStartMove = robotInput;
                GEndMove = robotInput;
                GGetMovePosition = robotInput;

                Time.timeScale = RobotSpeed;
                break;
            case InputSource.Empty:
                GStartFire = emptyInput;
                GGetFireDirection = emptyInput;
                GGetMousePosition = emptyInput;
                GStartAim = emptyInput;
                GEndAim = emptyInput;
                GReturnFire = playerInput;
                GStartMove = emptyInput;
                GEndMove = emptyInput;
                GGetMovePosition = emptyInput;

                break;
        }
    }

    public Vector2 GetMovePosition()
    {
        return GGetMovePosition.GetMovePosition();
    }

    public bool EndMove()
    {
        return GEndMove.EndMove();
    }

    public bool StartMove()
    {
        return GStartMove.StartMove();
    }

    public bool ReturnFire()
    {
        return GReturnFire.ReturnFire();
    }

    public bool EndAim()
    {
        return GEndAim.EndAim();
    }

    public bool StartAim()
    {
        return GStartAim.StartAim();
    }

    public Vector3 GetMousePosition()
    {
        return GGetMousePosition.GetMousePosition();
    }

    public bool StartFire()
    {
        return GStartFire.StartFire();
    }

    public Vector2 GetFireDirection()
    {
        return GGetFireDirection.GetFireDirection();
    }
}
