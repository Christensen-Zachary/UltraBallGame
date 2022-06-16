using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GState
{
    EmptyState,
    SetupLevel,
    WaitingForPlayerInput,
    MovingPlayer,
    Aiming,
    Firing,
    EndTurn,
    GameOver,
    Win,
    OptionsPanel
}


public class FSMGame : MonoBehaviour
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }
    private GState _state = GState.SetupLevel;
    private GState _stateBeforeOpeningOptions = GState.EmptyState;

    private Grid _grid;
    private Player _player;
    private GameInput _gameInput;
    private LevelService _levelService;
    private BrickFixCollision _brickFixCollision;
    private FacBrick _facBrick;
    private FacBall _facBall;
    private AdvanceService _advanceService;
    private EndTurnDestroyService _endTurnDestroyService;
    private GameUI _gameUI;
    private GameUISwitcher _gameUISwitcher;
    private WinService _winService;

    private bool _isRunningSetupLevel = false;
    private bool _isEndingTurn = false;

    void Start()
    {
        _grid = ResourceLocator.GetResource<Grid>("Grid");
        _player = ResourceLocator.GetResource<Player>("Player");
        _gameInput = ResourceLocator.GetResource<GameInput>("GameInput");
        _levelService = ResourceLocator.GetResource<LevelService>("Level");
        _brickFixCollision = ResourceLocator.GetResource<BrickFixCollision>("BrickFixCollision");
        _facBrick = ResourceLocator.GetResource<FacBrick>("FacBrick");
        _facBall = ResourceLocator.GetResource<FacBall>("FacBall");
        _advanceService = ResourceLocator.GetResource<AdvanceService>("AdvanceService");
        _endTurnDestroyService = ResourceLocator.GetResource<EndTurnDestroyService>("EndTurnDestroyService");
        _gameUI = ResourceLocator.GetResource<GameUI>("GameUI");
        _gameUISwitcher = ResourceLocator.GetResource<GameUISwitcher>("GameUISwitcher");
        _winService = ResourceLocator.GetResource<WinService>("WinService");

        //Time.timeScale = 0.3f;
    }


    void Update()
    {
        //print($"state: {_state}");

        /*
         * Use effectors to create directional collisions to create 'doors' for balls to go through and get trapped for maximum fun zone
         * 
         */
        switch (_state)
        {
            case GState.EmptyState:
                break;
            case GState.SetupLevel:
                if (!_isRunningSetupLevel)
                {
                    _isRunningSetupLevel = true;
                    _state = GState.EmptyState;
                    StartCoroutine(SetupLevel());
                }
                break;
            case GState.WaitingForPlayerInput:
                if (_gameInput.StartAim())
                {
                    _state = GState.Aiming;
                }
                else if (_gameInput.StartMove())
                {
                    _gameUISwitcher.ShowMoveSlider(true);
                    _state = GState.MovingPlayer;
                }
                else if (_gameUI.OpenOptions)
                {
                    OpenOptions();
                }
                break;
            case GState.MovingPlayer:
                if (_gameInput.EndMove())
                {
                    _gameUISwitcher.ShowMoveSlider(false);
                    _state = GState.WaitingForPlayerInput;
                }
                else if (_gameUI.OpenOptions)
                {
                    OpenOptions();
                }
                else
                {
                    _player.MovePlayer(_gameInput.GetMovePosition());
                }
                break;
            case GState.Aiming:
                if (_gameInput.StartFire())
                {
                    _player.HideAim();
                    _player.RunFire(_gameInput.GetFireDirection());
                    _gameUISwitcher.StartFire();
                    _state = GState.Firing;
                }
                else if (_gameInput.EndAim())
                {
                    _player.HideAim();
                    _state = GState.WaitingForPlayerInput;
                }
                else
                {
                    _player.ShowAim(_gameInput.GetFireDirection());
                }
                break;
            case GState.Firing:
                if (_gameInput.ReturnFire() || _player.IsFireComplete())
                {
                    _player.EndFire();
                    _state = GState.EndTurn;
                }
                break;
            case GState.EndTurn:
                if (!_isEndingTurn)
                {
                    _state = GState.EmptyState;
                    StartCoroutine(EndTurnRoutine());
                }
                break;
            case GState.GameOver:
                if (_gameUI.ResetGame)
                {
                    _state = GState.SetupLevel;
                }
                else if (_gameUI.OpenMainMenu)
                {
                    SceneManager.LoadScene("MainMenu");
                }
                break;
            case GState.Win:
                if (_gameUI.NextLevel)
                {
                    ES3.Save(BGStrings.ES_LEVELNUM, _levelService._levelNumber + 1);
                    SceneManager.LoadScene("Game");
                }
                break;
            case GState.OptionsPanel:
                if (_gameUI.OpenMainMenuPanel)
                {
                    _gameUI.ShowMainMenuOkayPanel();
                }
                else if (_gameUI.CloseMainMenuPanel)
                {
                    _gameUI.HideMainMenuOkayPanel();

                    _state = _stateBeforeOpeningOptions;
                }
                else if (_gameUI.CloseOptionsPanel)
                {
                    _gameUI.HideOptions();

                    _state = _stateBeforeOpeningOptions;
                }
                else if (_gameUI.ResetGame)
                {
                    _state = GState.SetupLevel;
                }
                else if (_gameUI.OpenMainMenu)
                {
                    _gameUI.LoadMainMenu();
                }
                break;
        }

    }

    private void OpenOptions()
    {
        _gameUI.ShowOptions();

        _stateBeforeOpeningOptions = _state;
        _state = GState.OptionsPanel;
    }

    private IEnumerator EndTurnRoutine()
    {
        _isEndingTurn = true;

        _facBrick.DisableCompositeCollider();
        yield return StartCoroutine(_advanceService.Advance());
        _facBrick.EnableCompositeCollider();

        _endTurnDestroyService.DestroyGameObjects();

        if (_player.Health <= 0)
        {
            _gameUI.HideGame();
            _gameUI.ShowGameOver();

            _state = GState.GameOver;
        }
        else if (_winService.HasWon())
        {
            _gameUI.HideGame();
            _gameUI.ShowWin();

            _state = GState.Win;
        }
        else
        {
            _gameUISwitcher.StartTurn();
            _state = GState.WaitingForPlayerInput;
        }

        _isEndingTurn = false;
    }

    private IEnumerator SetupLevel()
    {
        _levelService.ResetLevelService();
        _gameUI.ShowGame();
        _gameUISwitcher.StartTurn();
        _player.Health = 100;
        _player.MovePlayer(_grid.GetPosition((_grid.NumberOfDivisions - 1) / 2f, _grid.NumberOfDivisions - 1));
        _facBrick.DestroyBricks();
        _facBall.DestroyBalls();
        _winService.NumberOfBricksDestroyed = 0;
        _winService.NumberOfBricksToWin = _levelService.Bricks.Where(x => Brick.IsDamageable(x.BrickType)).Count();

        _facBrick.MaxHealth = _levelService.Bricks.Select(x => x.Health).Max();

        for (int i = 0; i < _levelService.NumberOfDivisions - 1; i++)
        {
            _levelService.GetNextRow().ForEach(x => { x.Row--; _facBrick.Create(x); x.Row++; }); // subtract row so will advance down into position
        }

        _levelService.Balls.ForEach(x => _facBall.Create(x));
        _player.SetRadius();

        _facBrick.DisableCompositeCollider();
        yield return StartCoroutine(_advanceService.Advance());
        _facBrick.EnableCompositeCollider();

        // change states first so variable will always be true until after state change to avoid race condition, unless this happens atomically then it doesn't matter
        _state = GState.WaitingForPlayerInput;
        _isRunningSetupLevel = false;
    }
}
