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
    SliderAiming,
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
    private EndTurnAttackService _endTurnAttackService;
    private GameUI _gameUI;
    private GameUISwitcher _gameUISwitcher;
    private GameUIComposition _gameUIComposition;
    private WinService _winService;

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
        _endTurnAttackService = ResourceLocator.GetResource<EndTurnAttackService>("EndTurnAttackService");
        _gameUI = ResourceLocator.GetResource<GameUI>("GameUI");
        _gameUISwitcher = ResourceLocator.GetResource<GameUISwitcher>("GameUISwitcher");
        _gameUIComposition = ResourceLocator.GetResource<GameUIComposition>("GameUIComposition");
        _winService = ResourceLocator.GetResource<WinService>("WinService");

        //Time.timeScale = 2f;
    }


    void Update()
    {
        //print($"state: {_state}");

        switch (_state)
        {
            case GState.EmptyState:
                break;
            case GState.SetupLevel:
                _state = GState.EmptyState;
                StartCoroutine(SetupLevel());
                break;
            case GState.WaitingForPlayerInput:
                if (_gameInput.StartAim())
                {
                    _state = GState.Aiming;
                }
                else if (_gameInput.StartMove())
                {
                    if (_gameUISwitcher != null) _gameUISwitcher.ShowMoveSlider(true);
                    _state = GState.MovingPlayer;
                }
                else if (_gameUIComposition.OpenOptions())
                {
                    OpenOptions();
                }
                else if (_gameUIComposition.StartSliderAim())
                {
                    if (_gameUISwitcher != null) _gameUISwitcher.ShowAimSlider(true);
                    _state = GState.SliderAiming;
                }
                else if (_gameUIComposition.GiveExtraBalls())
                {
                    if (_levelService.ExtraBallPowerUpCount > 0)
                    {
                        _levelService.ExtraBallPowerUpCount--;
                        for (int i = 0; i < _levelService.Balls.Count; i++)
                        {
                            _endTurnDestroyService.AddGameObject(
                                _facBall.Create(_levelService.Balls[i])
                            );
                            _levelService.BallCounter++;
                        }
                    }
                }
                else if (_gameUIComposition.GiveFloorBricks())
                {
                    if (_levelService.FloorBricksPowerUpCount > 0)
                    {
                        _levelService.FloorBricksPowerUpCount--;
                        AddFloorBricks(); 
                    }
                }
                break;
            case GState.MovingPlayer:
                if (_gameInput.EndMove())
                {
                    if (_gameUISwitcher != null) _gameUISwitcher.ShowMoveSlider(false);
                    _state = GState.WaitingForPlayerInput;
                }
                else if (_gameUIComposition.OpenOptions())
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
                    if (_gameUISwitcher != null) _gameUISwitcher.StartFire();
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
            case GState.SliderAiming:
                if (_gameUIComposition.StartFire())
                {
                    _player.HideAim();
                    _player.RunFire(_gameUI.GetFireDirection());
                    _gameUISwitcher.StartFire();
                    _state = GState.Firing;
                }
                else if (_gameUIComposition.EndSliderAim())
                {
                    _gameUISwitcher.ShowAimSlider(false);
                    _player.HideAim();
                    _state = GState.WaitingForPlayerInput;
                }
                else
                {
                    _player.ShowAim(_gameUI.GetFireDirection());
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
                _state = GState.EmptyState;
                StartCoroutine(EndTurnRoutine());
                break;
            case GState.GameOver:
                if (_gameUIComposition.ResetGame())
                {
                    _state = GState.SetupLevel;
                }
                else if (_gameUIComposition.OpenMainMenu())
                {
                    SceneManager.LoadScene("MainMenu");
                }
                break;
            case GState.Win:
                if (_gameUIComposition.NextLevel())
                {
                    
                    SceneManager.LoadScene("Game");
                }
                break;
            case GState.OptionsPanel:
                if (_gameUIComposition.OpenMainMenuPanel())
                {
                    _gameUI.ShowMainMenuOkayPanel();
                }
                else if (_gameUIComposition.CloseMainMenuPanel())
                {
                    _gameUI.HideMainMenuOkayPanel();

                    _state = _stateBeforeOpeningOptions;
                }
                else if (_gameUIComposition.CloseOptionsPanel())
                {
                    _gameUI.HideOptions();

                    _state = _stateBeforeOpeningOptions;
                }
                else if (_gameUIComposition.ResetGame())
                {
                    _state = GState.SetupLevel;
                }
                else if (_gameUIComposition.OpenMainMenu())
                {
                    _gameUI.LoadMainMenu();
                }
                break;
        }

    }

    private void AddFloorBricks()
    {
        int closestColumn = 0;
        float closestColumnDistance = 100;
        Vector2 playerPosition = new Vector2(_player.transform.position.x, 0);
        for (int i = 0; i < _grid.NumberOfDivisions; i++)
        {
            Vector2 position = new Vector2(_grid.GetPosition(i, 0).x, 0);
            if (Vector2.Distance(playerPosition, position) < closestColumnDistance)
            {
                closestColumn = i;
                closestColumnDistance = Vector2.Distance(playerPosition, position);
            }

        }
        _player.MovePlayer(new Vector2(_grid.GetPosition(closestColumn, 0).x, _player.transform.position.y));

        for (int i = 0; i < _grid.NumberOfDivisions; i++)
        {
            BrickType brickType = BrickType.Square;
            if (i == closestColumn - 1) brickType = BrickType.Triangle0;
            else if (i == closestColumn + 1) brickType = BrickType.Triangle90;

            if (i != closestColumn)
            {
                GameObject obj = _facBrick.Create(new Brick { BrickType = brickType, Col = i, Row = _grid.NumberOfDivisions - 1, Health = 250 }, new Type[] { typeof(Advanceable) });
                _endTurnDestroyService.AddGameObject(obj);
                obj.GetComponentInChildren<Damageable>()._doesCountTowardsWinning = false;
            }
        }
    }

    private void OpenOptions()
    {
        if (_gameUI != null) _gameUI.ShowOptions();

        _stateBeforeOpeningOptions = _state;
        _state = GState.OptionsPanel;
    }

    private IEnumerator EndTurnRoutine()
    {
        _levelService.BallCounter = _levelService.Balls.Count;

        yield return StartCoroutine(_endTurnAttackService.Attack());

        CreateNextRow();

        _facBrick.DisableCompositeCollider();
        yield return StartCoroutine(_advanceService.Advance());
        _facBrick.EnableCompositeCollider();

        _endTurnDestroyService.DestroyGameObjects();

        if (_player.Health <= 0)
        {
            if (_gameUI != null) _gameUI.HideGame();
            if (_gameUI != null) _gameUI.ShowGameOver();

            _state = GState.GameOver;
        }
        else if (_winService.HasWon())
        {
            ES3.Save(BGStrings.ES_LEVELNUM, _levelService._levelNumber + 1);

            if (_gameUI != null) _gameUI.HideGame();
            if (_gameUI != null) _gameUI.ShowWin();

            _state = GState.Win;
        }
        else
        {
            if (_gameUISwitcher != null) _gameUISwitcher.StartTurn();
            _state = GState.WaitingForPlayerInput;
        }
    }

    private IEnumerator SetupLevel()
    {
        _endTurnAttackService.ResetAttackService();
        _endTurnDestroyService.DestroyGameObjects(); // this is important so that when the game is reset before the end of turn, then potential objects that had been added will be destroyed. Otherwise this service will cause an error if objects had been added
        _levelService.ResetLevelService();
        if (_gameUI != null) _gameUI.ShowGame();
        if (_gameUISwitcher != null) _gameUISwitcher.StartTurn();
        _player.Health = 100;
        _player.MovePlayer(_grid.GetPosition((_grid.NumberOfDivisions - 1) / 2f, _grid.NumberOfDivisions - 1));
        _facBrick.DestroyBricks();
        _facBall.DestroyBalls();
        _winService.NumberOfBricksDestroyed = 0;
        _winService.NumberOfBricksToWin = _levelService.Bricks.Where(x => Brick.IsDamageable(x.BrickType)).Count();

        _facBrick.MaxHealth = _levelService.Bricks.Select(x => x.Health).Max();

        for (int i = 0; i < _levelService.NumberOfDivisions - 1; i++)
        {
            CreateNextRow();
        }

        _levelService.Balls.ForEach(x => _facBall.Create(x));
        _player.SetRadius();

        _facBrick.DisableCompositeCollider();
        yield return StartCoroutine(_advanceService.Advance());
        _facBrick.EnableCompositeCollider();

        // change states first so variable will always be true until after state change to avoid race condition, unless this happens atomically then it doesn't matter
        _state = GState.WaitingForPlayerInput;
    }

    private void CreateNextRow()
    {
        _levelService.GetNextRow().ForEach(x => { x.Row--; _facBrick.Create(x); x.Row++; }); // subtract row so will advance down into position
    }
}
