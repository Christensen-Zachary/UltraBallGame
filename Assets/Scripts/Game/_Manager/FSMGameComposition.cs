using Mono.Cecil;
using UnityEngine;

public enum GameType
{
    Normal,
    MKB,
    ThemePreview,
    DesignLevel,
    CSVPreview
}

public class FSMGameComposition : MonoBehaviour, IGetState, IEmpty, ISetupLevel, IWaitingForPlayerInput, IMovingPlayer, IAiming, ISliderAiming, IFiring, IEndTurn, ICheckWinLose, IGameOver, IWin, IOptionsPanel
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
    public ICheckWinLose GCheckWinLose { get; set; }
    public IGameOver GGameOver { get; set; }
    public IWin GWin { get; set; }
    public IOptionsPanel GOptionsPanel { get; set; }

    [field: SerializeField]
    public GameType GameType { get; set; } = GameType.Normal;

    private NormalGame _normalGame;
    private MKBGame _mkbGame;
    private DesignLevelGame _designLevelGame;
    private ThemePreviewGame _themePreviewGame;
    private EmptyGame _emptyGame;
    private ShowCSVSavesGame _showCSVSavesGame;

    private void Awake() 
    {
        _normalGame = ResourceLocator.GetResource<NormalGame>("NormalGame");
        _mkbGame = ResourceLocator.GetResource<MKBGame>("MKBGame");
        _designLevelGame = ResourceLocator.GetResource<DesignLevelGame>("DesignLevelGame");
        _themePreviewGame = ResourceLocator.GetResource<ThemePreviewGame>("ThemePreviewGame");
        _emptyGame = ResourceLocator.GetResource<EmptyGame>("EmptyGame");
        _showCSVSavesGame = ResourceLocator.GetResource<ShowCSVSavesGame>("ShowCSVSavesGame");

        switch (GameType)
        {
            case GameType.DesignLevel:
                GGetState = _normalGame;
                GEmpty = _normalGame;
                GSetupLevel = _designLevelGame;
                GWaitingForPlayerInput = _designLevelGame;
                GMovingPlayer = _designLevelGame;
                GAiming = _normalGame;
                GSliderAiming = _normalGame;
                GFiring = _normalGame;
                GEndTurn = _normalGame;
                GCheckWinLose = _normalGame;
                GGameOver = _normalGame;
                GWin = _normalGame;
                GOptionsPanel = _normalGame;
                break;
            case GameType.ThemePreview:
                GGetState = _normalGame;
                GEmpty = _normalGame;
                GSetupLevel = _normalGame;
                GWaitingForPlayerInput = _mkbGame;
                GMovingPlayer = _emptyGame;
                GAiming = _normalGame;
                GSliderAiming = _normalGame;
                GFiring = _normalGame;
                GEndTurn = _normalGame;
                GCheckWinLose = _themePreviewGame;
                GGameOver = _normalGame;
                GWin = _normalGame;
                GOptionsPanel = _normalGame;
                break;
            case GameType.Normal:
                GGetState = _normalGame;
                GEmpty = _normalGame;
                GSetupLevel = _normalGame;
                GWaitingForPlayerInput = _normalGame;
                GMovingPlayer = _emptyGame;
                GAiming = _normalGame;
                GSliderAiming = _normalGame;
                GFiring = _normalGame;
                GEndTurn = _normalGame;
                GCheckWinLose = _normalGame;
                GGameOver = _normalGame;
                GWin = _normalGame;
                GOptionsPanel = _normalGame;
                break;
            case GameType.CSVPreview:
                GGetState = _normalGame;
                GEmpty = _emptyGame;
                GSetupLevel =_showCSVSavesGame;
                GWaitingForPlayerInput = _showCSVSavesGame;
                GMovingPlayer = _emptyGame;
                GAiming = _emptyGame;
                GSliderAiming = _emptyGame;
                GFiring = _emptyGame;
                GEndTurn = _emptyGame;
                GCheckWinLose = _emptyGame;
                GGameOver = _emptyGame;
                GWin = _emptyGame;
                GOptionsPanel = _emptyGame;
                break;
            case GameType.MKB:
                GGetState = _normalGame;
                GEmpty = _normalGame;
                GSetupLevel = _normalGame;
                GWaitingForPlayerInput = _mkbGame;
                GMovingPlayer = _normalGame;
                GAiming = _normalGame;
                GSliderAiming = _normalGame;
                GFiring = _normalGame;
                GEndTurn = _normalGame;
                GCheckWinLose = _normalGame;
                GGameOver = _normalGame;
                GWin = _normalGame;
                GOptionsPanel = _normalGame;
                break;
            default:
                GGetState = _normalGame;
                GEmpty = _normalGame;
                GSetupLevel = _normalGame;
                GWaitingForPlayerInput = _normalGame;
                GMovingPlayer = _emptyGame;
                GAiming = _normalGame;
                GSliderAiming = _normalGame;
                GFiring = _normalGame;
                GEndTurn = _normalGame;
                GCheckWinLose = _normalGame;
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

    public void CheckWinLose()
    {
        GCheckWinLose.CheckWinLose();
    }
}