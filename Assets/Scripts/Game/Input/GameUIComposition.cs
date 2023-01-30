using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameUIType
{
    Game,
    MKB,
    Empty
}

public class GameUIComposition : MonoBehaviour, IResetGame, INextLevel, IOpenMainMenu, ICloseMainMenuPanel, IOpenMainMenuPanel, IOpenOptions, ICloseOptionsPanel, IStartSliderAim, IEndSliderAim, IStartFireUI, IGiveExtraBalls, IGiveFloorBricks
{

    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }

    public GameUIType GameUIType = GameUIType.Game;


    private GameUI _gameUI;
    private EmptyGameUI _emptyGameUI;
    private GameUIMKB _gameUIMKB;

    private IResetGame GResetGame;
    private INextLevel GNextLevel;
    private IOpenMainMenu GOpenMainMenu;
    private ICloseMainMenuPanel GCloseMainMenuPanel;
    private IOpenMainMenuPanel GOpenMainMenuPanel;
    private IOpenOptions GOpenOptions;
    private ICloseOptionsPanel GCloseOptionsPanel;
    private IStartSliderAim GStartSliderAim;
    private IEndSliderAim GEndSliderAim;
    private IStartFireUI GStartFireUI;
    private IGiveExtraBalls GGiveExtraBalls;
    private IGiveFloorBricks GGiveFloorBricks;
    private ISetBallsOnFire GSetBallsOnFire;

    private void Awake()
    {
        ResourceLocator.AddResource("GameUIComposition", this);

        _gameUI = ResourceLocator.GetResource<GameUI>("GameUI");
        _emptyGameUI = ResourceLocator.GetResource<EmptyGameUI>("EmptyGameUI");
        _gameUIMKB = ResourceLocator.GetResource<GameUIMKB>("GameUIMKB");

        switch (GameUIType)
        {
            case GameUIType.Game:
                GResetGame = _gameUI;
                GNextLevel = _gameUI;
                GOpenMainMenu = _gameUI;
                GCloseMainMenuPanel = _gameUI;
                GOpenMainMenuPanel = _gameUI;
                GOpenOptions = _gameUI;
                GCloseOptionsPanel = _gameUI;
                GStartSliderAim = _gameUI;
                GEndSliderAim = _gameUI;
                GStartFireUI = _gameUI;
                GGiveExtraBalls = _gameUI;
                GGiveFloorBricks = _gameUI;
                GSetBallsOnFire = _gameUI;
                break;
            case GameUIType.MKB:
                GResetGame = _gameUIMKB;
                GNextLevel = _gameUIMKB;
                GOpenMainMenu = _gameUIMKB;
                GCloseMainMenuPanel = _gameUIMKB;
                GOpenMainMenuPanel = _gameUIMKB;
                GOpenOptions = _gameUIMKB;
                GCloseOptionsPanel = _gameUIMKB;
                GStartSliderAim = _gameUIMKB;
                GEndSliderAim = _gameUIMKB;
                GStartFireUI = _gameUIMKB;
                GGiveExtraBalls = _gameUIMKB;
                GGiveFloorBricks = _gameUIMKB;
                break;
            default:
                GResetGame = _emptyGameUI;
                GNextLevel = _emptyGameUI;
                GOpenMainMenu = _emptyGameUI;
                GCloseMainMenuPanel = _emptyGameUI;
                GOpenMainMenuPanel = _emptyGameUI;
                GOpenOptions = _emptyGameUI;
                GCloseOptionsPanel = _emptyGameUI;
                GStartSliderAim = _emptyGameUI;
                GEndSliderAim = _emptyGameUI;
                GStartFireUI = _emptyGameUI;
                GGiveExtraBalls = _emptyGameUI;
                GGiveFloorBricks = _emptyGameUI;
                break;
        }   
    }

    public bool CloseMainMenuPanel()
    {
        return GCloseMainMenuPanel.CloseMainMenuPanel(); ;
    }

    public bool CloseOptionsPanel()
    {
        return GCloseOptionsPanel.CloseOptionsPanel(); ;
    }

    public bool EndSliderAim()
    {
        return GEndSliderAim.EndSliderAim();
    }

    public bool GiveExtraBalls()
    {
        return GGiveExtraBalls.GiveExtraBalls();
    }

    public bool GiveFloorBricks()
    {
        return GGiveFloorBricks.GiveFloorBricks();
    }

    public bool SetBallsOnFire()
    {
        return GSetBallsOnFire.SetBallsOnFire();
    }

    public bool NextLevel()
    {
        return GNextLevel.NextLevel();
    }

    public bool OpenMainMenu()
    {
        return GOpenMainMenu.OpenMainMenu();
    }

    public bool OpenMainMenuPanel()
    {
        return GOpenMainMenuPanel.OpenMainMenuPanel();
    }

    public bool OpenOptions()
    {
        return GOpenOptions.OpenOptions();
    }

    public bool ResetGame()
    {
        return GResetGame.ResetGame();
    }

    public bool StartFire()
    {
        return GStartFireUI.StartFire();
    }

    public bool StartSliderAim()
    {
        return GStartSliderAim.StartSliderAim();
    }
}
