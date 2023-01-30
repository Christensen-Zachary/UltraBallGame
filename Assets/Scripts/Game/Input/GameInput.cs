using UnityEngine;

public enum InputSource
{
    Player,
    PlayerMKB,
    Robot,
    Empty
}

public class GameInput : MonoBehaviour, ITouchingGameboard, IStartFire, IGetFireDirection, IGetMousePosition, IStartAim, IEndAim, IReturnFire, IStartMove, IEndMove, IGetMovePosition
{
    [field: SerializeField]
    private ResourceLocator ResourceLocator { get; set; }
    public InputSource _inputSource = InputSource.Player;
    [field: SerializeField]
    public float RobotSpeed { get; set; } = 4;

    private ITouchingGameboard GTouchingGameboard { get; set; }
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
        GameUI gameUI = ResourceLocator.GetResource<GameUI>("GameUI");
        EmptyInput emptyInput = ResourceLocator.GetResource<EmptyInput>("EmptyInput");
        RandomFireDirection randomFireDirection = ResourceLocator.GetResource<RandomFireDirection>("RandomFireDirection");
        switch (_inputSource)
        {
            case InputSource.Player:
                GTouchingGameboard = playerInput;
                GStartFire = playerInput;
                GGetFireDirection = playerInput;
                GGetMousePosition = playerInput;
                GStartAim = playerInput;
                GEndAim = playerInput;
                GReturnFire = gameUI;
                GStartMove = gameUI;
                GEndMove = gameUI;
                GGetMovePosition = playerInput;
                break;
            case InputSource.PlayerMKB:
                GTouchingGameboard = playerInput;
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
                GTouchingGameboard = emptyInput;
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
                GTouchingGameboard = emptyInput;
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

    public bool TouchingGameboard()
    {
        return GTouchingGameboard.TouchingGameboard();
    }
}
