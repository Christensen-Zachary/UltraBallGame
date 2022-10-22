using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameUIType
{
    Game,
    MKB,
    Empty
}

public class GameUIComposition : MonoBehaviour, IResetGame, INextLevel, IOpenMainMenu, ICloseMainMenuPanel, IOpenMainMenuPanel, IOpenOptions, ICloseOptionsPanel, IStartSliderAim, IEndSliderAim, IStartFireUI, IGiveExtraBalls, IGiveFloorBricks, IStartMove, IEndMove, IReturnFire
{

    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }

    public GameUIType GameUIType = GameUIType.Game;


    private GameUI _gameUI;
    private GameUIInput _gameUIInput;
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
    private IStartMove GStartMove;
    private IEndMove GEndMove;
    private IReturnFire GReturnFire;

    private void Awake()
    {
        ResourceLocator.AddResource("GameUIComposition", this);

        _gameUI = ResourceLocator.GetResource<GameUI>("GameUI");
        _gameUIInput = ResourceLocator.GetResource<GameUIInput>("GameUIInput");
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
                GStartMove = _gameUIInput;
                GEndMove = _gameUIInput;
                GReturnFire = _gameUIInput;
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
                GStartMove = _gameUIMKB;
                GEndMove = _gameUIMKB;
                GReturnFire = _gameUIMKB;
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
                GStartMove = _emptyGameUI;
                GEndMove = _emptyGameUI;
                GReturnFire = _emptyGameUI;
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

    // I think this is unused and can be removed
    public bool EndMove()
    {
        return GEndMove.EndMove();
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

    public bool ReturnFire()
    {
        return GReturnFire.ReturnFire();
    }

    public bool StartFire()
    {
        return GStartFireUI.StartFire();
    }

    public bool StartMove()
    {
        return GStartMove.StartMove();
    }

    public bool StartSliderAim()
    {
        return GStartSliderAim.StartSliderAim();
    }
}
