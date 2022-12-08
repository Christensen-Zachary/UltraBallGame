using Mono.Cecil;
using UnityEngine;

public enum GameType
{
    Normal
}

public class FSMGameComposition : MonoBehaviour, IGetState, IEmpty, ISetupLevel, IWaitingForPlayerInput, IMovingPlayer, IAiming, ISliderAiming, IFiring, IEndTurn, IGameOver, IWin, IOptionsPanel
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }

    public IGetState GGetState { get; set; }
    public IEmpty GEmpty { get; set; }
    public ISetupLevel GSetupLevel { get; set; }
    public IWaitingForPlayerInput GWaitingForPlayerInput { get; set; }
    public IMovingPlayer GMovingPlayer { get; set; }
    public IAiming GAiming { get; set; }
    public ISliderAiming GSliderAiming { get; set; }
    public IFiring GFiring { get; set; }
    public IEndTurn GEndTurn { get; set; }
    public IGameOver GGameOver { get; set; }
    public IWin GWin { get; set; }
    public IOptionsPanel GOptionsPanel { get; set; }

    [field: SerializeField]
    public GameType GameType { get; set; } = GameType.Normal;

    private NormalGame _normalGame;

    private void Awake() 
    {
        _normalGame = ResourceLocator.GetResource<NormalGame>("NormalGame");

        switch (GameType)
        {
            case GameType.Normal:
                GGetState = _normalGame;
                GEmpty = _normalGame;
                GSetupLevel = _normalGame;
                GWaitingForPlayerInput = _normalGame;
                GMovingPlayer = _normalGame;
                GAiming = _normalGame;
                GSliderAiming = _normalGame;
                GFiring = _normalGame;
                GEndTurn = _normalGame;
                GGameOver = _normalGame;
                GWin = _normalGame;
                GOptionsPanel = _normalGame;
                break;
            default:
                GGetState = _normalGame;
                GEmpty = _normalGame;
                GSetupLevel = _normalGame;
                GWaitingForPlayerInput = _normalGame;
                GMovingPlayer = _normalGame;
                GAiming = _normalGame;
                GSliderAiming = _normalGame;
                GFiring = _normalGame;
                GEndTurn = _normalGame;
                GGameOver = _normalGame;
                GWin = _normalGame;
                GOptionsPanel = _normalGame;
                break;
        }       
    }

    public void Aiming()
    {
        GAiming.Aiming();
    }

    public void Empty()
    {
        GEmpty.Empty();
    }

    public void EndTurn()
    {
        GEndTurn.EndTurn();
    }

    public void Firing()
    {
        GFiring.Firing();
    }

    public void GameOver()
    {
        GGameOver.GameOver();
    }

    public void MovingPlayer()
    {
        GMovingPlayer.MovingPlayer();
    }

    public void OptionsPanel()
    {
        GOptionsPanel.OptionsPanel();
    }

    public void SetupLevel()
    {
        GSetupLevel.SetupLevel();
    }

    public void SliderAiming()
    {
        GSliderAiming.SliderAiming();
    }

    public void WaitingForPlayerInput()
    {
        GWaitingForPlayerInput.WaitingForPlayerInput();
    }
    public void Win()
    {
        GWin.Win();
    }

    public GState GetState()
    {
        return GGetState.GetState();
    }
}